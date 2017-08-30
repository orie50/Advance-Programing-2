using System.Windows;
using ClientGUI.model;

namespace ClientGUI.view.Menus
{
    /// <summary>
    /// Interaction logic for SettingsMenu.xaml
    /// </summary>
    public partial class SettingsMenu : Window
    {
        /// <summary>
        /// The view model
        /// </summary>
        private SettingViewModel _vm;

        /// <summary>
        /// constructor of the <see cref="SettingsMenu"/> class.
        /// </summary>
        public SettingsMenu()
        {
            InitializeComponent();
            _vm = new SettingViewModel(new SettingsModel());
            DataContext = _vm;
        }

        /// <summary>
        /// Handles the Click event of the btnBack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Properties.Settings.Default.Reload();
            new MainWindow().Show();
            Close();
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _vm.SaveSetting();
            Hide();
            new MainWindow().Show();
            Close();
        }
    }
}