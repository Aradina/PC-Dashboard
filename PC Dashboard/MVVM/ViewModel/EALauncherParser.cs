using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PC_Dashboard.MVVM.ViewModel
{
    internal class EALauncherParser
    {

        public static List<Game> Parse()
        {
            string jsonPath = Path.Combine(Environment.CurrentDirectory, @"Resources\json\EAApps.json");
            if (File.Exists(jsonPath))
            {
                return Utilities.ReadFromJson(jsonPath);
            }
            else
            {
                List<Game> apps = GetEAGames();
                Utilities.WriteToJson(apps, jsonPath);
                return apps;
            }
        }

        public static List<Game> GetEAGames()
        {
            var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            var regKey = hklm.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall", false);
            List<Game> games = new();
            foreach(string subKeyName in regKey.GetSubKeyNames())
            {
                using(RegistryKey subkey = regKey.OpenSubKey(subKeyName))
                {
                    if(subkey.GetValue("UninstallString") != null && subkey.GetValue("UninstallString").ToString().Contains(@"Common Files\EAInstaller"))
                    {
                        Game EAApp = new()
                        {
                            LauncherId = 5,
                            Name = subkey.GetValue("DisplayName").ToString(),
                            PublisherName = subkey.GetValue("Publisher").ToString(),
                            Url = subkey.GetValue("URLInfoAbout").ToString(),
                            GameRoot = subkey.GetValue("InstallLocation").ToString(),
                            Executable = subkey.GetValue("DisplayIcon").ToString(),
                            InstallDir = subkey.GetValue("InstallLocation").ToString(),
                            LibraryCard = Path.Combine(Environment.CurrentDirectory, @"Images\noimage_600x900.png"),
                        };
                        games.Add(EAApp);
                    }
                }
            }
            return games;
        }
    }
}
