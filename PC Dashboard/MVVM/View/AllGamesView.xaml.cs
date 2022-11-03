using PC_Dashboard.MVVM.ViewModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PC_Dashboard.MVVM.View
{
    /// <summary>
    /// Interaction logic for AllGamesView.xaml
    /// </summary>
    public partial class AllGamesView : UserControl
    {
        public AllGamesView()
        {
            InitializeComponent();
        }

        private void SearchPopout_GotFocus(object sender, RoutedEventArgs e)
        {
            StackPanel sourcePanel = (StackPanel)sender;
            sourcePanel.Width = 500;
        }
        private void SearchPopout_LostFocus(object sender, RoutedEventArgs e)
        {
            StackPanel sourcePanel = (StackPanel)sender;
            sourcePanel.Width = 50;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UIElement focusedItem = Keyboard.FocusedElement as UIElement;
            
        }
    }
}
