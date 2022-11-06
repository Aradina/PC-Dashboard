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
            if (File.Exists(jsonPath) && jsonPath == "no")
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
            var regKey = hklm.OpenSubKey(@"SOFTWARE\WOW6432Node\Origin Games", false);
            List<Game> games = new();
            List<string> installedEAGames = new();
            foreach(string subKeyName in regKey.GetSubKeyNames())
            {
                using(RegistryKey subkey = regKey.OpenSubKey(subKeyName))
                {
                    installedEAGames.Add(subkey.GetValue("DisplayName").ToString());
                    var uninstallRegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    var uninst = uninstallRegistryKey.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall", false);
                    foreach(string uninstSubKey in uninst.GetSubKeyNames())
                    {
                        using (RegistryKey registryKey = uninst.OpenSubKey(uninstSubKey))
                        {
                            Debug.WriteLine(subkey.GetValue("DisplayName").ToString());
                            if (registryKey.GetValue("DisplayName") != null && registryKey.GetValue("DisplayName").ToString() == subkey.GetValue("DisplayName").ToString())
                            {
                                Debug.WriteLine(registryKey.GetValue("DisplayName").ToString());
                            }
                        }
                    }
                }
            }




            return games;
        }



    }
}
