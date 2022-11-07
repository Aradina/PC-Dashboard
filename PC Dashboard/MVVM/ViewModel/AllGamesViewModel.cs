using PC_Dashboard.Core;
using PC_Dashboard.MVVM.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace PC_Dashboard.MVVM.ViewModel
{
    internal class AllGamesViewModel : ObservableObject
    {
        public ObservableCollection<Game> AllGames { get; set; }

        public AllGamesViewModel()
        {
            AllGames = new ObservableCollection<Game>();
            PopulateList();
        }

        void PopulateList()
        {
            List<Game> gamesList = new();
            if (AvailableLaunchers.EALauncher) 
            {
                gamesList.AddRange(EALauncherParser.Parse());
            }
            if (AvailableLaunchers.XboxLauncher)
            {
                gamesList.AddRange(XboxStoreParser.Parse());
            }
            if (AvailableLaunchers.Steam)
            {
                gamesList.AddRange(SteamLibraryParser.Parse());
            }
            if (AvailableLaunchers.GOGGalaxy)
            {
                gamesList.AddRange(GOGGalaxyParser.Parse());
            }
            if (AvailableLaunchers.BattleNet)
            {
                gamesList.AddRange(BattleNetParser.Parse());
            }
            if (AvailableLaunchers.EpicGamesStore)
            {
                gamesList.AddRange(EpicGamesParser.Parse());
            }
            if (AvailableLaunchers.Itchio)
            {

            }
            if (AvailableLaunchers.Ubisoft)
            {

            }
            gamesList.Sort();
            for (int i = 0; i < gamesList.Count; i++)
            {
                AllGames.Add(gamesList[i]);
            }
        }
    }
}
