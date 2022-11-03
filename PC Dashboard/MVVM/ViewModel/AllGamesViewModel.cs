using PC_Dashboard.Core;
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
            gamesList.AddRange(SteamLibraryParser.Parse());
            gamesList.AddRange(XboxStoreParser.Parse());
            
            for (int i = 0; i < gamesList.Count; i++)
            {
                AllGames.Add(gamesList[i]);
            }
        }
    }
}
