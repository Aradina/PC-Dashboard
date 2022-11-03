using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using ValveKeyValue;

namespace PC_Dashboard.MVVM.ViewModel
{
    class SteamLibraryParser
    {
        /// <summary>
        /// Checks if the json file already exists, if not, creates it.
        /// </summary>
        /// <returns>List of Game.</returns>
        public static List<Game> Parse()
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, @"Resources\json\SteamApps.json")))
            {
                return ReadFromJson();
            }
            else
            {
                var steamLibraries = GetSteamLibraries();
                var apps = GetSteamApps(steamLibraries);
                WriteToJson(apps);
                return ReadFromJson();
            }
        }

        /// <summary>
        /// Writes the Steam app information to a json file for later use between program restarts.
        /// </summary>
        /// <param name="e">An AppInfo object.</param>
        static void WriteToJson(List<Game> e)
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"Resources\json\SteamApps.json");
            var apps = new List<Game>();
            string cachePath = GetSteamImageCachePath();

            foreach (Game app in e)
            {
                if(app.Name != "Steamworks Common Redistributables")
                {
                    Game steamapps = new()
                    {
                        LauncherId = 1,
                        Id = app.Id,
                        Name = app.Name,
                        PublisherName = "",
                        LaunchParameters = app.LaunchParameters,
                        GameRoot = app.GameRoot,
                        Executable = app.Executable,
                        InstallDir = app.InstallDir,
                        InstalledDateUnix = app.InstalledDateUnix,
                        InstalledDate = Utilities.UnixToDateTime(app.InstalledDateUnix).Date.ToString("d"),
                        AppImagePath = cachePath,
                        AppIcon = Path.Combine(cachePath, $"{app.Id}_icon.jpg"),
                        HeaderImage = Path.Combine(cachePath, $"{app.Id}_header.jpg"),
                        LibraryCard = Path.Combine(cachePath, $"{app.Id}_library_600x900.jpg")

                    };
                    apps.Add(steamapps);
                }
                
            }

            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, apps);
            }
        }
        /// <summary>
        /// Reads the saved json file containing steam app information.
        /// </summary>
        /// <returns>A Steamapp list from the json.</returns>
        public static List<Game> ReadFromJson()
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"Resources\json\SteamApps.json");
            using (StreamReader file = new(path))
            {
                string json = file.ReadToEnd();
                List<Game> apps = JsonConvert.DeserializeObject<List<Game>>(json);
                return apps;
            }
        }

        static List<Game> GetSteamApps(List<string> steamLibs)
        {
            var apps = new List<Game>();

            foreach (var steamLibraries in steamLibs)
            {
                var appMetaDataPath = Path.Combine(steamLibraries, "SteamApps");
                var files = Directory.GetFiles(appMetaDataPath, "*.acf");
                foreach (var file in files)
                {
                    Game appInfo = GetAppInfo(file);
                    if (appInfo != null)
                    {
                        apps.Add(appInfo);
                    }
                }
            }
            return apps;
        }

        static Game GetAppInfo(string appMetaFile)
        {
            var metaLines = File.ReadAllLines(appMetaFile);
            var dic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var line in metaLines)
            {
                var match = Regex.Match(line, @"\s*""(?<key>\w+)""\s+""(?<val>.*)""");
                if (match.Success)
                {
                    var key = match.Groups["key"].Value;
                    var value = match.Groups["val"].Value;
                    dic[key] = value;
                }
            }

            Game appInfo = null;

            if (dic.Keys.Count > 0)
            {
                appInfo = new Game();
                var appId = dic["appid"];
                var name = dic["name"];
                var installDir = dic["installDir"];
                var installDate = dic["LastUpdated"];

                var path = Path.GetDirectoryName(appMetaFile);
                var libraryGameRoot = Path.Combine(path, "common", installDir);

                if (!Directory.Exists(libraryGameRoot))
                {
                    return null;
                }

                appInfo.Id = appId;
                appInfo.Name = name;
                appInfo.GameRoot = libraryGameRoot;
                appInfo.InstallDir = installDir;
                appInfo.InstalledDateUnix = int.Parse(installDate);
                appInfo.LaunchParameters = $"steam://rungameid/{appId}";
                appInfo.Executable = GetExecutable(appInfo);
            }
            return appInfo;
        }

        static string _appInfoText = null;
        
        static string GetExecutable(Game appInfo)
        {
            if (_appInfoText == null)
            {
                string appInfoFile = Path.Combine(GetSteamPath(), "appcache", "appinfo.vdf");
                byte[] bytes = File.ReadAllBytes(appInfoFile);
                _appInfoText = Encoding.UTF8.GetString(bytes);
            }
            int startIndex = 0;
            int maxAttempts = 20;
            string fullName = "";
            do
            {
                int startOfDataArea = _appInfoText.IndexOf($"\x00\x01name\x00{appInfo.Name}\x00", startIndex);
                if (startOfDataArea < 0 && maxAttempts == 20) startOfDataArea = _appInfoText.IndexOf($"\x00\x01gamedir\x00{appInfo.Name}\x00", startIndex);
                if (startOfDataArea < 0 && maxAttempts == 20) startOfDataArea = _appInfoText.IndexOf($"\x00\x01name\x00{appInfo.Name}\x00", startIndex);
                if (startOfDataArea > 0)
                {
                    startIndex = startOfDataArea + 10;
                    int nextLaunch = -1;
                    do
                    {
                        int executable = _appInfoText.IndexOf($"\x00\x01executable\x00", startOfDataArea);
                        if (executable > -1 && nextLaunch == -1)
                        {
                            nextLaunch = _appInfoText.IndexOf($"\x00\x01launch\x00", executable);
                        }
                        if ((nextLaunch <= 0 || executable < nextLaunch) && executable > 0)
                        {
                            if (executable > 0)
                            {
                                executable += 10;
                                string filename = "";
                                while (_appInfoText[executable] != '\x00')
                                {
                                    filename += _appInfoText[executable];
                                    executable++;
                                }
                                if (filename.Contains("://"))
                                {
                                    return filename;
                                }

                                fullName = Path.Combine(appInfo.GameRoot, filename);
                                startOfDataArea = executable + 1;
                                startIndex = startOfDataArea + 10;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    while (!File.Exists(fullName) && maxAttempts-- > 0);
                }
                else
                {
                    return null;
                }
            } while (!File.Exists(fullName) && maxAttempts-- > 0);
            if (File.Exists(fullName)) return fullName;

            return null;
        }

        /// <summary>
        /// Gets the Steam libraries from the libraryfolders.vdf file in the steam directory.
        /// </summary>
        /// <returns>A list of Steam library paths. </returns>

        static List<string> GetSteamLibraries()
        {
            string libpath = Path.Combine(GetSteamPath(), @"steamapps\libraryfolders.vdf");
            
            var stream = File.OpenRead(libpath);
            var kv = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
            KVObject data = kv.Deserialize(stream);
            List<string> libraries = new();

            foreach (var item in data)
            {
                if (Convert.ToInt64(item["totalsize"]) > 0)
                {
                    libraries.Add(item["path"].ToString());
                }
            }
            return libraries;
        }

        /// <summary>
        /// Gets the steam install path via the registry.
        /// </summary>
        /// <returns>The path to the Steam install folder.</returns>

        public static string GetSteamPath()
        {
            string steamPath = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", null).ToString();
            return steamPath.Replace(@"/", @"\");
        }

        /// <summary>
        /// Gets the path to the steam library image cache.
        /// </summary>
        /// <returns>The path to the Steam library image cache.</returns>

        static string GetSteamImageCachePath()
        {
            string SteamPath = GetSteamPath();
            string ImageCachePath = Path.Combine(SteamPath, @"appcache\librarycache");
            return ImageCachePath;
        }
    }
}
