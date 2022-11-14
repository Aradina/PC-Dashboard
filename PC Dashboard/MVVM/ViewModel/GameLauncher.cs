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
                    LaunchBlizzardGame(game);
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
        /// <summary>
        /// Blizzard games can be launched directly through the exe, but that won't authenticate and the user would have to log in.
        /// This avoids that problem by passing a launch parameter to the battle.net.exe executable.
        /// The parameters vary between games and are magic strings over in the parser. This isn't perfect and may require updating.
        /// It may also abruptly stop working, as older methods seem to have.
        /// It also can't tell the difference between versions of Classic wow. But I don't think I can fix that.
        /// </summary>
        /// <param name="game">Game object for a blizzard game.</param>

        static void LaunchBlizzardGame(Game game)
        {
            ProcessStartInfo processStartInfo = new()
            {
                FileName = game.Executable,
                Arguments = game.LaunchParameters
            };
            if(game.LaunchParameters != "Not found.")
            {
                Process.Start(processStartInfo);
            }
            else
            {
                //put an error here indicating something went wrong. Currently fails silently.
            }
        }
    }
}
