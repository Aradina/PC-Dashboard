
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace PC_Dashboard.MVVM.ViewModel
{
    public class SearchViewModel : BaseGameListViewModel
    {
        private ObservableCollection<Game> _FilteredList = new ObservableCollection<Game>();
        public ObservableCollection<Game> FilteredList 
        { 
            get => _FilteredList; 
            set 
            {
                _FilteredList = value;
                OnPropertyChanged("FilteredList");
            }
        }

        private string _StringFilter;
        public string StringFilter
        { 
            get { return _StringFilter; }
            set 
            {
                _StringFilter = value;
                FilterResults(value);
                OnPropertyChanged("FilteredList");
            } 
        }

        void FilterResults(string filter)
        {
            FilteredList = new ObservableCollection<Game>(AllGames.Where(g => g.Name.ToLower().Contains(filter.ToLower())));
            Debug.WriteLine(FilteredList);
        }

        public SearchViewModel() : base()
        {
            
        }
    }
}
