using PC_Dashboard.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Dashboard.MVVM.Model
{
    public class GamesList
    {
        private ObservableCollection<Game> games = null;

        public ObservableCollection<Game> Games 
        { 
            get 
            { 
                if (games == null) 
                {
                    games = new ObservableCollection<Game>();   
                    PopulateList(); 
                    return games; 
                }
                else
                {
                    return games;
                }
            }
        }

        private void PopulateList()
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
                //Partially implemented.
                gamesList.AddRange(BattleNetParser.Parse());
            }
            if (AvailableLaunchers.EpicGamesStore)
            {
                gamesList.AddRange(EpicGamesParser.Parse());
            }
            if (AvailableLaunchers.Itchio)
            {
                //Not implemented  
            }
            if (AvailableLaunchers.Ubisoft)
            {
                //Not implemented
            }
            gamesList.Sort();
            for (int i = 0; i < gamesList.Count; i++)
            {
                if (Directory.Exists(gamesList[i].GameRoot))
                {
                    games.Add(gamesList[i]);
                }
            }
        }
    }
}
