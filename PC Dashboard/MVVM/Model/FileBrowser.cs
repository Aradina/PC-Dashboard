using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Dashboard.MVVM.Model
{
    public class FileBrowser
    {

        public void Go()
        {
            string winDir = Environment.GetEnvironmentVariable("windir");
            List<string> drives = GetDrives();
            GetFullFileSystem(drives);
        }

        public List<string> GetDrives()
        {
            List<string> results = new();
            string[] drives = Directory.GetLogicalDrives();
            foreach (string drive in drives)
            {
                results.Add(drive);
            }
            return results;
        }

        public List<string> GetDriveContent(string drive)
        {
            string[] dirs = Directory.GetDirectories(drive);
            List<string> folders = new();
            foreach (string dir in dirs)
            {
                folders.Add(dir);
            }
            return folders;
        }

        public void GetFullFileSystem(List<string> drives)
        {
            foreach(string folder in drives)
            {
                var content = GetDriveContent(folder);
                foreach(string dir in content)
                {
                    Debug.WriteLine(dir);
                }
                
            }
        }

    }
}
