using System.Windows;
using ClientGUI.model;

namespace ClientGUI.view.Menus
{
	/// <summary>
	/// Interaction logic for SinglePlayer.xaml
	/// </summary>
	public partial class SinglePlayerMenu : Window
	{
		/// <summary>
		/// a flag to indicate if the game started
		/// </summary>
		private bool _gameStarted;
		/// <summary>
		/// The view model
		/// </summary>
		private SinglePlayerViewModel _viewModel;
		/// <summary>
		/// Initializes a new instance of the <see cref="SinglePlayerMenu"/> class.
		/// </summary>
		public SinglePlayerMenu()
		{
			InitializeComponent();
			_viewModel = new SinglePlayerViewModel(new SinglePlayerModel());
			DataContext = _viewModel;
			_gameStarted = false;
		}
		/// <summary>
		/// Handles the Closed event of the Window control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void Window_Closed(object sender, System.EventArgs e)
		{
			if (!_gameStarted)
			{
				new view.MainWindow().Show();
			}
		}
		/// <summary>
		/// Handles the Click event of the btnStartGame control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
		private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            Games.SinglePlayerGame game = new Games.SinglePlayerGame(_viewModel);
            _viewModel.GenerateMaze();
            _gameStarted = true;
            Close();
        }
		/// <summary>
		/// Handles the Click event of the btnBack control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
		private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
