using PC_Dashboard.Core;
using PC_Dashboard.MVVM.Model;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using WindowsDisplayAPI;

namespace PC_Dashboard.MVVM.ViewModel
{
    public class SettingsViewModel : ObservableObject
    {
        private ObservableCollection<Display> _screens = new();

        public ObservableCollection<Display> Screens 
        { 
            get 
            { 
                return _screens; 
            }
            set 
            { 
                _screens = value; 
            }
        }

        public SettingsViewModel() 
        {
            if (Utilities.IsInDesignMode == true)
            {
                var screens = new ObservableCollection<Display>(Utilities.GetMonitors());
                Screens = screens;
            }

        }
    }
}
