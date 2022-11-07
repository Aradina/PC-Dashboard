using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;

namespace PC_Dashboard.MVVM.ViewModel
{
    internal class BattleNetParser
    {
        public static List<Game> Parse()
        {
            string jsonPath = Path.Combine(Environment.CurrentDirectory, @"Resources\json\BlizzardApps.json");
            if (File.Exists(jsonPath))
            {
                return Utilities.ReadFromJson(jsonPath);
            }
            else
            {
                List<Game> apps = GetBlizzardGames();
                Utilities.WriteToJson(apps, jsonPath);
                return apps;
            }
        }

        static List<Game> GetBlizzardGames()
        {
            List<Game> games = new();
            var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            var regKey = hklm.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall", false);
            foreach (string subKeyName in regKey.GetSubKeyNames())
            {
                using (RegistryKey subkey = regKey.OpenSubKey(subKeyName))
                {
                    if (subkey.GetValue("UninstallString") != null && subkey.GetValue("UninstallString").ToString().Contains(@"Battle.net\Agent"))
                    {
                        Game BlizzardApp = new()
                        {
                            LauncherId = 6,
                            Name = subkey.GetValue("DisplayName").ToString(),
                            PublisherName = subkey.GetValue("Publisher").ToString(),
                            Url = subkey.GetValue("URLInfoAbout").ToString(),
                            GameRoot = subkey.GetValue("InstallLocation").ToString(),
                            Executable = subkey.GetValue("DisplayIcon").ToString(),
                            InstallDir = subkey.GetValue("InstallLocation").ToString(),
                            LibraryCard = Path.Combine(Environment.CurrentDirectory, @"Images\noimage_600x900.png"),
                        };
                        games.Add(BlizzardApp);
                    }
                }
            }

            return games;
        }

    }
}
