using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Dashboard.MVVM.ViewModel
{
    class GameLauncher
    {

        public static void LaunchGame(Game game)
        {
            if(game is null)
            {
                return;
            }
            switch (game.LauncherId)
            {
                case 0:
                    break;
                case 1:
                    LaunchSteamGame(game);
                    break;
                case 2:
                    LaunchXboxGame(game);
                    break;
                case 3:
                    LaunchGogGame(game);
                    break;
            }
            
        }

        static void LaunchGogGame(Game game)
        {
            ProcessStartInfo processStartInfo = new()
            {
                FileName = game.Executable,
            };
            Process.Start(processStartInfo);
        }

        static void LaunchSteamGame(Game game)
        {
            string steamPath = System.IO.Path.Combine(SteamLibraryParser.GetSteamPath(), @"steam.exe");
            ProcessStartInfo processStartInfo = new()
            {
                FileName = steamPath,
                Arguments = game.LaunchParameters
            };
            Process.Start(processStartInfo);
        }

        static void LaunchXboxGame(Game game)
        {
            string exePath = System.IO.Path.Combine(game.GameRoot, game.Executable);
            ProcessStartInfo processStartInfo = new()
            {
                FileName = exePath,
            };
            Process.Start(processStartInfo);
        }

    }
}
