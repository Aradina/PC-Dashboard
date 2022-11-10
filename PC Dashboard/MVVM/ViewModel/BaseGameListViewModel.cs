using PC_Dashboard.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Dashboard.MVVM.ViewModel
{
    public class BaseGameListViewModel
    {
        public BaseGameListViewModel()
        {
            
        }
        public ObservableCollection<Game> AllGames { get; set; } 

    }
}
