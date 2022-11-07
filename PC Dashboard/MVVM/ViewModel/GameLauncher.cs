using System.Diagnostics;

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
                    LaunchAppGeneric(game);
                    break;
                case 3:
                    LaunchAppGeneric(game);
                    break;
                case 4:
                    LaunchAppGeneric(game);
                    break;
                case 5:
                    LaunchAppGeneric(game);
                    break;
                case 6:
                    //LaunchBlizzardGame
                    break;
            }
        }

        /// <summary>
        /// Launches an exe. Use if all is required is starting the exe. If special parameters are required, make another.
        /// </summary>
        /// <param name="game">game object.</param>
        static void LaunchAppGeneric(Game game)
        {
            ProcessStartInfo processStartInfo = new()
            {
                FileName = game.Executable,
            };
            Process.Start(processStartInfo);
        }
        /// <summary>
        /// Steam games are launched by launching the steam executable with the games uri as a parameter. So we do that.
        /// </summary>
        /// <param name="game">game object.</param>
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

    }
}
