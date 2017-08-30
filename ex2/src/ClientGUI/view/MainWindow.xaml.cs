using System;
using System.Windows;

namespace ClientGUI.view
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// constructor of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.CanMinimize;
        }

        /// <summary>
        /// Handles the Click event of the btnSinglePlayer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnSinglePlayer_Click(object sender, RoutedEventArgs e)
        {
            Menus.SinglePlayerMenu singlePlayer = new Menus.SinglePlayerMenu();
            Hide();
            singlePlayer.Show();
	        Close();
		}

        /// <summary>
        /// Handles the Click event of the btnMultiPlayer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnMultiPlayer_Click(object sender, RoutedEventArgs e)
        {
            Menus.MultiPlayerMenu multiPlayer = new Menus.MultiPlayerMenu();
            Hide();
            multiPlayer.Show();
	        Close();
		}

        /// <summary>
        /// Handles the Click event of the btnSettings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Menus.SettingsMenu settings = new Menus.SettingsMenu();
            Hide();
            settings.Show();
	        Close();
		}

        /// <summary>
        /// Handles the Click event of the btnExit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Environment.Exit(0);
        }
    }
}
