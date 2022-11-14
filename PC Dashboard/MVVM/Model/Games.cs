using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PC_Dashboard
{
    public class Game : IComparable<Game>
    {
        public int LauncherId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string OverrideDisplayName { get; set; }
        public string PublisherName { get; set; }
        public string Url { get; set; }
        public string GameRoot { get; set; }
        public string Executable { get; set; }
        public string OtherExecutable { get; set; }
        public string LaunchParameters { get; set; }
        public string InstallDir { get; set; }
        public int InstallSize { get; set; }
        public string AppImagePath { get; set; }
        public string AppIcon { get; set; }
        public string HeaderImage { get; set; }
        public string LibraryCard { get; set; }
        public int InstalledDateUnix { get; set; }
        public string InstalledDate { get; set; }

        public int CompareTo(Game other)
        {
            if(null == other)
            {
                return 1;
            }
            return string.Compare(this.Name, other.Name);
        }


        //Gets the image for use in binding. This is to fix an obscure error in the XAML.
        public object LibraryCardImageSource 
        { 
            get 
            {
                BitmapImage image = new();

                try
                {
                    image.BeginInit();
                    image.CacheOption= BitmapCacheOption.OnLoad;
                    image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    image.UriSource = new Uri(LibraryCard);
                    image.DecodePixelHeight = 225;
                    //Not all images are the right aspect ratio for this. 
                    //Setting just the heigh causes it to letterbox, which is prefered over weird stretching
                    //image.DecodePixelWidth = 150;
                    image.EndInit();
                }
                catch
                {
                    return DependencyProperty.UnsetValue;
                }
                return image;
            } 
        }
    }
}
