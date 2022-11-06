using PC_Dashboard.Core;

namespace PC_Dashboard.MVVM.Model
{
    public class AvailableLaunchers
    {
        public static bool EALauncher { get => RegistryKeyExists.KeyExists(@"SOFTWARE\Electronic Arts"); }

        //Need to find which registry keys are made when using this. May instead check for installed games to determine if it's "available". Returns true every time for now, as I believe it's always going to be available.
        public static bool XboxLauncher { get => true; }

        public static bool Steam { get => RegistryKeyExists.KeyExists(@"SOFTWARE\WOW6432Node\Valve\Steam"); }

        public static bool GOGGalaxy { get => RegistryKeyExists.KeyExists(@"SOFTWARE\WOW6432Node\GOG.com\GalaxyClient"); }

        public static bool BattleNet { get => RegistryKeyExists.KeyExists(@"SOFTWARE\WOW6432Node\Blizzard Entertainment\Battle.net"); }

        public static bool EpicGamesStore { get => RegistryKeyExists.KeyExists(@"SOFTWARE\WOW6432Node\Epic Games\EpicGamesLauncher"); }

        public static bool Itchio { get => false; }

        public static bool Ubisoft { get => RegistryKeyExists.KeyExists(@"SOFTWARE\WOW6432Node\Ubisoft"); }

    }
}
