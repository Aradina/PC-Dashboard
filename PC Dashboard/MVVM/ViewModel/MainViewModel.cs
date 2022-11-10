using PC_Dashboard.Core;
using PC_Dashboard.MVVM.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace PC_Dashboard.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }

        public RelayCommand DashboardViewCommand { get; set; }

        public RelayCommand AllGamesViewCommand { get; set; }

        public RelayCommand SettingsViewCommand { get; set; }

        public RelayCommand CategoriesViewCommand { get; set; }

        public RelayCommand SearchViewCommand { get; set; }

        public RelayCommand ExitProgramCommand { get; set; }

        public RelayCommand GameLauncherCommand { get; set; }


        public HomeViewModel HomeVM { get; set; }

        public DashboardViewModel DashboardVM { get; set; }

        public AllGamesViewModel AllGamesVM { get; set; }

        public SettingsViewModel SettingsVM { get; set; }

        public CategoriesViewModel CategoriesVM { get; set; }

        public SearchViewModel SearchVM { get; set; }

        public GamesList GamesList { get; set; } = new GamesList();
        
        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {

            var observableList = new ObservableCollection<Game>(GamesList.Games);
            Debug.WriteLine(observableList);

            HomeVM = new HomeViewModel();

            DashboardVM = new DashboardViewModel();

            AllGamesVM = new()
            {
                AllGames = observableList
            };

            SettingsVM = new SettingsViewModel();

            CategoriesVM = new CategoriesViewModel();

            SearchVM = new()
            {
                AllGames = observableList
            };

            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;

            });

            DashboardViewCommand = new RelayCommand(o =>
            {
                CurrentView = DashboardVM;
            });

            AllGamesViewCommand = new RelayCommand(o =>
            {
                CurrentView = AllGamesVM;

            });

            SettingsViewCommand = new RelayCommand(o =>
            {
                CurrentView = SettingsVM;
            });

            CategoriesViewCommand = new RelayCommand(o =>
            {
                CurrentView = CategoriesVM;
            });

            SearchViewCommand = new RelayCommand(o =>
            {
                CurrentView = SearchVM;
            });

            ExitProgramCommand = new RelayCommand(o =>
            {
                Window window = (Window)o;
                window.Close();
            });

            GameLauncherCommand = new RelayCommand(o =>
            {
                if (o is null)
                {
                    return;
                }
                Game game = (Game)o;
                GameLauncher.LaunchGame(game);
            });

        }
    }
}
