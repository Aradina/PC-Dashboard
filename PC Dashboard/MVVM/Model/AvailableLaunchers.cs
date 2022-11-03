using PC_Dashboard.Core;

namespace PC_Dashboard.MVVM.Model
{
    internal class AvailableLaunchers
    {
        static bool EALauncher { get => RegistryKeyExists.KeyExists(@"SOFTWARE\Electronic Arts"); }

        //Need to find which registry keys are made when using this. May instead check for installed games to determine if it's "available". Returns true every time for now, as I believe it's always going to be available.
        static bool XboxLauncher { get => true; }

        static bool Steam { get => RegistryKeyExists.KeyExists(@"SOFTWARE\WOW6432Node\Valve\Steam"); }

        static bool GOGGalaxy { get => RegistryKeyExists.KeyExists(@"SOFTWARE\WOW6432Node\GOG.com\GalaxyClient"); }

        static bool BattleNet { get => RegistryKeyExists.KeyExists(@"SOFTWARE\WOW6432Node\Blizzard Entertainment\Battle.net"); }

        static bool EpicGamesStore { get => RegistryKeyExists.KeyExists(@"SOFTWARE\WOW6432Node\Epic Games\EpicGamesLauncher"); }

        static bool Itchio { get => false; }

        static bool Ubisoft { get => RegistryKeyExists.KeyExists(@"SOFTWARE\WOW6432Node\Ubisoft"); }

    }
}
