using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsDisplayAPI;

namespace PC_Dashboard
{
    class Utilities
    {


        public static bool IsInDesignMode
        {
            get
            {
                return DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject());
            }
        }

        public static ObservableCollection<Display> GetMonitors()
        {
            ObservableCollection<Display> monitors = new ObservableCollection<Display>(Display.GetDisplays());
            return monitors;
        }

        public static void SearchList(List<Game> list, string searchValue)
        {
            List<Game> filteredList = list.Where(obj => obj.Name == searchValue).ToList();
        }

        public static DateTimeOffset UnixToDateTime(int unixtime)
        {
            DateTimeOffset dt = DateTimeOffset.FromUnixTimeSeconds(unixtime);
            Debug.WriteLine($"DateTimeOffsetFromUnixTimeSeconds: {dt}");
            return dt;
        }


        public static List<Game> ReadFromJson(string path)
        {
            using (StreamReader file = new(path))
            {
                string json = file.ReadToEnd();
                List<Game> apps = JsonConvert.DeserializeObject<List<Game>>(json);
                return apps;
            }
        }

        public static void WriteToJson(List<Game> e, string path)
        {
            var apps = new List<Game>();
            foreach (Game app in e)
            {
                Game game = new()
                {
                    LauncherId = app.LauncherId,
                    Id = app.Id,
                    Name = app.Name,
                    OverrideDisplayName = app.OverrideDisplayName,
                    PublisherName = app.PublisherName,
                    Url = app.Url,
                    GameRoot = app.GameRoot,
                    Executable = app.Executable,
                    OtherExecutable = app.OtherExecutable,
                    LaunchParameters = app.LaunchParameters,
                    InstallDir = app.InstallDir,
                    AppImagePath = app.GameRoot,
                    AppIcon = app.AppIcon,
                    HeaderImage = app.HeaderImage,
                    LibraryCard = app.LibraryCard,
                    InstalledDateUnix = app.InstalledDateUnix,
                    InstalledDate = app.InstalledDate,

                };
                apps.Add(game);
            }

            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new()
                {
                    Formatting = Formatting.Indented
                };
                serializer.Serialize(file, apps);
            }
        }


    }
}
