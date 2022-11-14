using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
            string battlenetPath = GetBlizzardLauncherExecutable();
            var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            var regKey = hklm.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall", false);
            foreach (string subKeyName in regKey.GetSubKeyNames())
            {
                using (RegistryKey subkey = regKey.OpenSubKey(subKeyName))
                {
                    //Unwieldy and probably better done in another way. Makes sure it exists, checks if the game is a battlenet game by checking for a part of the path to the uninstallers for battlenet games.
                    //This has the issue of also getting the battle.net application itself, so we also filter that out. 
                    if (subkey.GetValue("UninstallString") != null && subkey.GetValue("UninstallString").ToString().Contains(@"Battle.net\Agent") && subkey.GetValue("DisplayName").ToString() != "Battle.net")
                    {
                        Game BlizzardApp = new()
                        {
                            LauncherId = 6,
                            Name = subkey.GetValue("DisplayName").ToString(),
                            PublisherName = subkey.GetValue("Publisher").ToString(),
                            Url = subkey.GetValue("URLInfoAbout").ToString(),
                            GameRoot = subkey.GetValue("InstallLocation").ToString(),
                            Executable = battlenetPath,
                            InstallDir = subkey.GetValue("InstallLocation").ToString(),
                            LibraryCard = Path.Combine(Environment.CurrentDirectory, @"Images\noimage_600x900.png"),
                            LaunchParameters = GetBlizzardLaunchParams(subkey.GetValue("DisplayName").ToString())
                        };
                        games.Add(BlizzardApp);
                    }
                }
            }
            return games;
        }

        /// <summary>
        /// To launch blizzard games we need to use the launch parameter here. This does that. This isn't a good way to do this but it'll work fine. Probably.
        /// </summary>
        /// <param name="GameName">Name of the game</param>
        /// <returns>Launch parameter to launch the game.</returns>
        /// <exception cref="ArgumentException">Game not found.</exception>
        static string GetBlizzardLaunchParams(string GameName)
        {
            
            switch (GameName.ToLower()) 
            { 
                case "world of warcraft": 
                    return @" --exec=""launch WoW""";
                case "wrath of the lich king classic":
                    return @" --exec=""launch WoWC""";
                case "overwatch":
                    return @" --exec""launch PRO""";
                case "blizzard arcade collection":
                    return @" --exec""launch RTRO""";
                case "hearthstone":
                    return @" --exec""launch WTCG""";
                case "geroes of the storm":
                    return @" --exec""launch Hero""";
                case "warcraft 3: reforged":
                    return @" --exec""launch W3""";
                case "starcraft remastered":
                    return @" --exec""launch S1""";
                case "starcraft 2":
                    return @" --exec""launch S2""";
                case "diablo 2: resurrected":
                    return @" --exec""launch OSI""";
                case "diablo 3":
                    return @" --exec""launch D3""";
                case "diablo immortal":
                    return @" --exec""launch ANBS""";
                case "crash bandicoot 4: it's about time":
                    return @" --exec""launch WLBY""";
                case "call of duty: modern warfare 2 campaign remastered":
                    return @" --exec""launch LAZR""";
                case "call of duty: black ops 4":
                    return @" --exec""launch VIPR""";
                case "call of duty: black ops: cold war":
                    return @" --exec""launch ZEUS""";
                case "call of duty: modern warfare":
                    return @" --exec""launch ODIN""";
                case "call of duty: vanguard":
                    return @" --exec""launch FORE""";
                case "call of duty: modern warfare 2":
                    return @" --exec""launch AUKS""";
                default:
                    return "Not found.";
            }
        }

        /// <summary>
        /// Grabs the Battle.net executable from the registry. Hopefully consistent location.
        /// </summary>
        /// <returns>Path to Battle.net.exe.</returns>
        static string GetBlizzardLauncherExecutable()
        {
            var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            var regKey = hklm.OpenSubKey(@"SOFTWARE\WOW6432Node\Blizzard Entertainment\Battle.net\Capabilities", false);
            string rawPath = regKey.GetValue("ApplicationIcon").ToString();
            string bnetPath = rawPath.Remove(rawPath.Length - 2);
            return bnetPath;
        }

    }
}
