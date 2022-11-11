using PC_Dashboard.Core;
using PC_Dashboard.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Dashboard.MVVM.ViewModel
{
    public class BaseGameListViewModel : ObservableObject
    {
        public BaseGameListViewModel()
        {
            if(IsInDesignMode == true)
            {
                var gameList = new GamesList();
                AllGames = gameList.Games;
            }
        }
        public ObservableCollection<Game> AllGames { get; set; } 

        public static bool IsInDesignMode
        { 
            get
            {
                return DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject());
            } 
        }

    }
}
