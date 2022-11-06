using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace PC_Dashboard.MVVM.ViewModel
{
    internal class EpicGamesParser
    {
        public static List<Game> Parse()
        {
            string jsonPath = Path.Combine(Environment.CurrentDirectory, @"Resources\json\EpicApps.json");
            if (File.Exists(jsonPath))
            {
                return Utilities.ReadFromJson(jsonPath);
            }
            else
            {
                List<Game> games = GetEpicGames();
                return games;
            }
        }

        static List<Game> GetEpicGames()
        {
            string epicManifestPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Epic\EpicGamesLauncher\Data\Manifests");
            string[] files = Directory.GetFiles(epicManifestPath);
            List<Game> games = new();

            foreach (string file in files)
            {
                if(Path.GetExtension(file) == ".item")
                {
                    using (StreamReader manifest = new (file))
                    {
                        string json = manifest.ReadToEnd();
                        var epicManifest = JsonConvert.DeserializeObject<EpicManifest>(json);
                        Game epicApps = new()
                        {
                            LauncherId = 4,
                            Id = "",
                            Name = epicManifest.DisplayName,
                            OverrideDisplayName = "",
                            PublisherName = "",
                            Url = "",
                            GameRoot = epicManifest.InstallLocation,
                            Executable = Path.Combine(epicManifest.InstallLocation, epicManifest.LaunchExecutable),
                            OtherExecutable = "",
                            LaunchParameters = "",
                            InstallDir = epicManifest.InstallLocation,
                            InstallSize = epicManifest.InstallSize,
                            AppImagePath = "",
                            AppIcon = "",
                            HeaderImage = "",
                            LibraryCard = Path.Combine(Environment.CurrentDirectory, @"Images\noimage_600x900.png"),
                            InstalledDateUnix = 0,
                            InstalledDate = "",
                        };
                        games.Add(epicApps);
                    }
                }
            }
            return games;
        }
    }
}
