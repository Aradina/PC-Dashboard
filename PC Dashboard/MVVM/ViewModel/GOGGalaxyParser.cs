using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PC_Dashboard.MVVM.ViewModel
{
    internal class GOGGalaxyParser
    {
        public static List<Game> Parse()
        {
            string jsonPath = Path.Combine(Environment.CurrentDirectory, @"Resources\json\GogApps.json");
            if (File.Exists(jsonPath))
            {
                return Utilities.ReadFromJson(jsonPath);
            }
            else
            {
                List<Game> games = GetGogGames();
                Utilities.WriteToJson(games, jsonPath);
                return games;
            }
        }

        static List<Game> GetGogGames()
        {
            var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            var regKey = hklm.OpenSubKey(@"SOFTWARE\WOW6432Node\GOG.com\Games");
            List<Game> gogGames = new();

            foreach(string subKeyName in regKey.GetSubKeyNames())
            {
                using(RegistryKey subKey = regKey.OpenSubKey(subKeyName))
                {   
                    string gogCachePath = GetGogImageCachePath();
                    string libraryCard = GogLibraryCard(Path.Combine(gogCachePath, subKey.GetValue("gameID").ToString()));
                    var game = new Game()
                    {
                        LauncherId = 3,
                        Id = subKey.GetValue("gameID").ToString(),
                        Name = subKey.GetValue("gameName").ToString(),
                        OverrideDisplayName = "",
                        PublisherName = "",
                        GameRoot = subKey.GetValue("path").ToString(),
                        Executable = subKey.GetValue("exe").ToString(),
                        AppImagePath = "",
                        AppIcon = "",
                        HeaderImage = "",
                        LibraryCard = libraryCard,
                    };
                    gogGames.Add(game);
                }   
            }
            return gogGames;
        }

        static string GetGogImageCachePath()
        {
            string gogUserId = GetGogUserId();
            string commonAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string gogCachePath = Path.Combine(commonAppDataPath, @"GOG.com\Galaxy\webcache", gogUserId, @"gog");
            return gogCachePath;
        }

        static string GogLibraryCard(string path)
        {
            string[] files = Directory.GetFiles(path);
            BitmapImage image = new();
            foreach (string file in files)
            {
                Uri uri = new(file);
                image.BeginInit();
                image.UriSource = uri;
                image.CacheOption = BitmapCacheOption.None;
                image.EndInit();
                if(image.PixelHeight == 482 && image.PixelWidth == 342)
                {
                    return file;
                }
                else
                {
                    return "";
                }
                
            }
            return "";
        }

        static string GetGogUserId()
        {
            var hklm = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
            var regKey = hklm.OpenSubKey(@"Software\GOG.com\Galaxy\settings");
            string gogId = regKey.GetValue("userId").ToString();
            return gogId;
        }
    }
}
