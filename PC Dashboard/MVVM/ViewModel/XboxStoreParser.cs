using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace PC_Dashboard.MVVM.ViewModel
{
    internal class XboxStoreParser
    {
        /// <summary>
        /// Checks if the json file already exists, if not, creates it.
        /// </summary>
        /// <returns>List of Game.</returns>
        public static List<Game> Parse()
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, @"Resources\json\XboxApps.json")))
            {
                return ReadFromJson();
            }
            else
            {
                List<string> gameRoots = GetXboxRoot();
                var apps = GetAppInfo(gameRoots);
                WriteToJson(apps);
                return ReadFromJson();
            }
        }

        static void WriteToJson(List<Game> e)
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"Resources\json\XboxApps.json");
            var apps = new List<Game>();

            foreach (Game app in e)
            {
                Game xboxApp = new()
                {
                    LauncherId = 2,
                    Id = app.Id,
                    Name = app.Name,
                    OverrideDisplayName = app.OverrideDisplayName,
                    PublisherName = app.PublisherName,
                    Url = app.Url,
                    GameRoot = app.GameRoot,
                    Executable = app.Executable,
                    OtherExecutable = app.OtherExecutable,
                    AppImagePath = app.GameRoot,
                    AppIcon = Path.Combine(app.GameRoot, app.AppIcon),
                    HeaderImage = Path.Combine(app.GameRoot, app.HeaderImage),
                    LibraryCard = Path.Combine(app.GameRoot, app.LibraryCard),

                };
                apps.Add(xboxApp);
            }

            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new();
                serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
                serializer.Serialize(file, apps);
            }
        }
        /// <summary>
        /// Reads the saved json file containing steam app information.
        /// </summary>
        /// <returns>A Steamapp list from the json.</returns>
        public static List<Game> ReadFromJson()
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"Resources\json\XboxApps.json");
            using (StreamReader file = new StreamReader(path))
            {
                string json = file.ReadToEnd();
                List<Game> apps = JsonConvert.DeserializeObject<List<Game>>(json);
                return apps;
            }
        }




        /// <summary>
        /// Gets the directories of installed Xbox packages from the Registry. This is not all games, which are filtered out later. It also includes DLCs and DigitalOwnership files.
        /// </summary>
        /// <returns>A list of Xbox related package directories.</returns>
        static List<string> GetXboxRoot()
        {

            var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            var regKey = hklm.OpenSubKey(@"SOFTWARE\Microsoft\GamingServices\PackageRepository\Root", false);

            List<string> rootList = new();

            foreach (string subKeyName in regKey.GetSubKeyNames())
            {
                using (RegistryKey key = regKey.OpenSubKey(subKeyName))
                {
                    foreach (string subSubKeyName in key.GetSubKeyNames())
                    {
                        using (RegistryKey subSubKey = key.OpenSubKey(subSubKeyName))
                        {
                            if (subSubKey.GetValue("Root") != null)
                            {
                                string root = subSubKey.GetValue("Root").ToString();
                                string rootRemoved = root.Remove(0, 4);
                                string endRemoved = rootRemoved.Remove(rootRemoved.Length - 1, 1);
                                rootList.Add(endRemoved);
                            }
                        }
                    }
                }

            }
            return rootList;
        }

        /// <summary>
        /// Takes a list of install directories and checks the MicrosoftGame.Config file, which contains many things we need.
        /// </summary>
        /// <param name="roots">A list of locations of installed xbox games.</param>
        /// <returns>A list of XboxAppInfo containing information about the installed xbox games, and filtering out non-game entries that we got earlier. </returns>

        static List<Game> GetAppInfo(List<string> roots)
        {
            var appInfo = new List<Game>();

            foreach (string root in roots)
            {
                var xmlFile = new XmlDocument();
                string xmlPath = Path.Combine(root, "MicrosoftGame.Config");
                xmlFile.Load(xmlPath);

                XmlNode executableList = xmlFile.DocumentElement.SelectSingleNode(@"ExecutableList");
                XmlNode shellVisuals = xmlFile.DocumentElement.SelectSingleNode(@"ShellVisuals");

                if (xmlFile.DocumentElement.SelectSingleNode(@"AllowedProducts") == null)
                {
                    Game xboxApps = new()
                    {
                        Id = xmlFile.DocumentElement.SelectSingleNode(@"TitleId").InnerText,
                        Executable = "",
                        OtherExecutable = "",
                        Name = shellVisuals.Attributes.GetNamedItem("DefaultDisplayName").InnerText,
                        PublisherName = shellVisuals.Attributes.GetNamedItem("PublisherDisplayName").InnerText,
                        GameRoot = root,
                        OverrideDisplayName = "",
                        HeaderImage = shellVisuals.Attributes.GetNamedItem("SplashScreenImage").InnerText,
                        AppIcon = shellVisuals.Attributes.GetNamedItem("Square44x44Logo").InnerText,
                        LibraryCard = shellVisuals.Attributes.GetNamedItem("SplashScreenImage").InnerText,
                    };

                    if (xmlFile.DocumentElement.SelectSingleNode(@"ExecutableList").ChildNodes.Count > 1)
                    {
                        foreach (XmlNode ele in executableList.ChildNodes)
                        {
                            if (ele.NodeType != XmlNodeType.Comment)
                            {
                                if (ele.Attributes.GetNamedItem("OverrideDisplayName") != null)
                                {
                                    xboxApps.OtherExecutable = ele.Attributes.GetNamedItem("Name").InnerText;
                                    xboxApps.OverrideDisplayName = ele.Attributes.GetNamedItem("OverrideDisplayName").InnerText;
                                }
                                else
                                {
                                    xboxApps.Executable = ele.Attributes.GetNamedItem("Name").InnerText;
                                }
                            }
                        }
                    }
                    else
                    {
                        xboxApps.Executable = executableList.FirstChild.Attributes.GetNamedItem("Name").InnerText;
                    }
                    appInfo.Add(xboxApps);
                }
                else
                {
                    continue;
                }
            }
            return appInfo;
        }
    }
}
