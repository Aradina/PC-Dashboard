using PC_Dashboard.MVVM.Model;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Forms;

namespace PC_Dashboard.MVVM.View
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : System.Windows.Controls.UserControl
    {
        public SettingsView()
        {
            //Dirty but quick code-behind to set the checkboxes based on what's available.
            //Works. No real need to change it.
            InitializeComponent();
            SteamCheckBox.IsChecked = AvailableLaunchers.Steam;
            XboxCheckBox.IsChecked = AvailableLaunchers.XboxLauncher;
            EACheckBox.IsChecked = AvailableLaunchers.EALauncher;
            BlizzardCheckBox.IsChecked = AvailableLaunchers.BattleNet;
            EpicCheckBox.IsChecked = AvailableLaunchers.EpicGamesStore;
            GOGCheckBox.IsChecked = AvailableLaunchers.GOGGalaxy;
            ItchCheckBox.IsChecked = AvailableLaunchers.Itchio;
            UbisoftCheckBox.IsChecked = AvailableLaunchers.Ubisoft;
        }
    }
}
