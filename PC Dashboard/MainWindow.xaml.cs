using PC_Dashboard.MVVM.ViewModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using XInputium;
using XInputium.XInput;
#nullable enable
namespace PC_Dashboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SourceInitialized += (_, _) => CompositionTarget.Rendering += (_, _) => OnRendering();
            Loaded += MainWindow_Loaded;

            DeviceManager = new();
            DeviceManager.DeviceConnected += DeviceManager_DeviceConnected;
            Gamepad = new(null);
            Gamepad.StateChanged += (_, _) => OnGamepadStateChanged();
            Gamepad.ButtonPressed += Gamepad_ButtonPressed;
        }

        public XGamepad Gamepad
        {
            get => (XGamepad)GetValue(GamepadProperty);
            private set => SetValue(s_GamepadPropertyKey, value);
        }
        private static readonly DependencyPropertyKey s_GamepadPropertyKey
            = DependencyProperty.RegisterReadOnly(nameof(Gamepad), typeof(XGamepad), typeof(MainWindow), new PropertyMetadata(null));

        public static readonly DependencyProperty GamepadProperty
            = s_GamepadPropertyKey.DependencyProperty;

        public XInputDeviceManager DeviceManager
        {
            get => (XInputDeviceManager)GetValue(DeviceManagerProperty);
            private set => SetValue(s_DeviceManagerPropertyKey, value);
        }

        private static readonly DependencyPropertyKey s_DeviceManagerPropertyKey
            = DependencyProperty.RegisterReadOnly(nameof(DeviceManager), typeof(XInputDeviceManager), typeof(MainWindow), new PropertyMetadata(null));

        public static readonly DependencyProperty DeviceManagerProperty = s_DeviceManagerPropertyKey.DependencyProperty;

        /// <summary>
        /// XInputium: Fires whenever a Gamepad button is pressed.
        /// </summary>
        /// <param name="e">The object that fired it.</param>
        private void Gamepad_ButtonPressed(object? sender, DigitalButtonEventArgs<XInputButton> e)
        {
            Debug.WriteLine(e.Button);
            switch (e.Button.ToString())
            {
                case "DPadDown":
                    MoveFocus(FocusNavigationDirection.Down);
                    break;
                case "DPadUp":
                    MoveFocus(FocusNavigationDirection.Up);
                    break;
                case "DPadLeft":
                    MoveFocus(FocusNavigationDirection.Left);
                    break;
                case "DPadRight":
                    MoveFocus(FocusNavigationDirection.Right);
                    break;
                case "Y":
                    searchButton.Focus();
                    searchButton.Command.Execute(searchButton);
                    break;
                case "A":
                    SelectElement();
                    break;
            }

        }


        private static void SelectElement()
        {
            if (Keyboard.FocusedElement is UIElement elementWithFocus)
            {
                if (elementWithFocus is Button button)
                {
                    button.Command.Execute(button.CommandParameter);
                }
            }
        }


        /// <summary>
        /// Moves focus between UI Elements on the MainWindow.
        /// </summary>
        /// <param name="direction">The direction to move focus.</param>
        private void MoveFocus(FocusNavigationDirection direction)
        {
            var request = new TraversalRequest(direction);
            if (Keyboard.FocusedElement is UIElement elementWithFocus)
            {
                bool moved = elementWithFocus.MoveFocus(request);
                if (!moved && direction == FocusNavigationDirection.Left)
                {
                    FrameworkElement focusScope = (FrameworkElement)FocusManager.GetFocusScope(elementWithFocus);
                    MoveToMenu(focusScope);
                }
            }
        }

        private void MoveToMenu(FrameworkElement focusScope)
        {
            switch (focusScope.Name)
            {
                case "TileListView":
                    allButton.Focus();
                    break;
            }
        }

        /// <summary>
        /// Fires whenever the window is rendered.
        /// </summary>
        protected virtual void OnRendering()
        {
            DeviceManager.Update();
            Gamepad.Update();
        }

        protected virtual void OnGamepadStateChanged()
        {
            Gamepad.LeftMotorSpeed = Gamepad.LeftTrigger.Value;
            Gamepad.RightMotorSpeed = Gamepad.RightTrigger.Value;
        }

        /// <summary>
        /// RoutedEvent that fires automatically when the main window loads.
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            updatesButton.Focus();
        }
        /// <summary>
        /// XInputium: Fires when a device is connected.
        /// </summary>
        private void DeviceManager_DeviceConnected(object? sender, XInputDeviceEventArgs e)
        {
            if (!Gamepad.IsConnected)
            {
                Gamepad.Device = DeviceManager.ConnectedDevices.FirstOrDefault();
            }
        }

        /// <summary>
        /// RoutedEvent that fires when a MenuButton gets focus.
        /// </summary>
        /// <param name="sender">The UI element that was focused.</param>
        /// <param name="e">Also the UI element that was focused.</param>
        private void MenuButton_GotFocus(object sender, RoutedEventArgs e)
        {
            RadioButton sourceButton = (RadioButton)sender;
            sourceButton.Command.Execute(sender);
        }
    }
}
