using ClientGUI.view_model;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ClientGUI.view.Menus;
using MazeLib;

namespace ClientGUI.view.Games
{
    /// <summary>
    /// Interaction logic for MultiPlayerGame.xaml
    /// </summary>
    public partial class MultiPlayerGame : Window
    {
        /// <summary>
        /// The view model
        /// </summary>
        private readonly MultiPlayerViewModel _vm;
		/// <summary>
		/// constructor of the <see cref="MultiPlayerGame"/> class.
		/// </summary>
		/// <param name="vm">The vm.</param>
		public MultiPlayerGame(MultiPlayerViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;

            // binding for start boolean in view model
            Binding binding = new Binding();
            binding.Path = new PropertyPath("Start");
            binding.Source = vm;
            BindingOperations.SetBinding(this, StartProperty, binding);

            // binding for finish boolean in view model
            binding = new Binding();
            binding.Path = new PropertyPath("Finish");
            binding.Source = vm;
            BindingOperations.SetBinding(this, FinishProperty, binding);

            // binding for finish message string in view model
            binding = new Binding();
            binding.Path = new PropertyPath("FinishMessage");
            binding.Source = vm;
            BindingOperations.SetBinding(this, FinishMessageProperty, binding);

            MyBoard.DataContext = _vm;
            OtherBoard.DataContext = _vm;
		}

        /// <summary>
        /// Start the game.
        /// </summary>
        public void StartGame()
        {
            Show();
            MyBoard.DrawMaze();
            MyBoard.RefreshMaze();
            OtherBoard.DrawMaze();
            OtherBoard.RefreshMaze();
        }

        /// <summary>
        /// Gets or sets if the game finished.
        /// </summary>
        /// <value>
        ///   <c>true</c> if finish; otherwise, <c>false</c>.
        /// </value>
        public bool Finish
        {
            get { return (bool)GetValue(FinishProperty); }
            set { SetValue(FinishProperty, value); }
        }

        /// <summary>
        /// The finish property
        /// </summary>
        public static readonly DependencyProperty FinishProperty =
            DependencyProperty.Register("Finish", typeof(bool), typeof(MultiPlayerGame), new PropertyMetadata(FinishPropertyChanged));

        /// <summary>
        /// Gets or sets the finish message.
        /// </summary>
        /// <value>
        /// The finish message.
        /// </value>
        public string FinishMessage
        {
            get { return (string)GetValue(FinishMessageProperty); }
            set { SetValue(FinishMessageProperty, value); }
        }

        /// <summary>
        /// The finish message property
        /// </summary>
        public static readonly DependencyProperty FinishMessageProperty =
            DependencyProperty.Register("FinishMessage", typeof(string), typeof(MultiPlayerGame));

        /// <summary>
        /// Gets or sets if the game started.
        /// </summary>
        /// <value>
        ///   <c>true</c> if start; otherwise, <c>false</c>.
        /// </value>
        public bool Start
        {
            get { return (bool)GetValue(StartProperty); }
            set { SetValue(FinishProperty, value); }
        }

        /// <summary>
        /// The start property
        /// </summary>
        public static readonly DependencyProperty StartProperty =
            DependencyProperty.Register("Start", typeof(bool), typeof(MultiPlayerGame), new PropertyMetadata(StartPropertyChanged));

        /// <summary>
        /// Start property changed method.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void StartPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                ((MultiPlayerGame)d).StartGame();
            }
        }

        /// <summary>
        /// Finish property changed method.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void FinishPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MultiPlayerGame)d).FinishGame();
        }

        /// <summary>
        /// Finish the game method.
        /// </summary>
        private void FinishGame()
        {
            MessageWindow message;
            // if close the game
            if (FinishMessage == null)
            {
                message = new MessageWindow("Connection Failed");
                message.Ok.Click += delegate (object sender1, RoutedEventArgs e1)
                {
                    // close the window and open main window
                    Close();
                    message.Hide();
                    new MainWindow().Show();
                    message.Close();
                };
                message.Cancel.Click += delegate (object sender1, RoutedEventArgs e1)
                {
                    // close the window and open main window
                    Close();
                    message.Hide();
                    new MainWindow().Show();
                    message.Close();
                };
                message.Show();
            }
            else if(FinishMessage.Equals(""))
            {
                new MainWindow().Show();
                Close();
                return;
            }
            // if the game alredy start
             else if (Finish == true && Start == true)
            {
                message = new MessageWindow(FinishMessage);
                message.Ok.Click += delegate (object sender1, RoutedEventArgs e1)
                {
                    // close the window and open main window
                    Close();
                    message.Hide();
                    new MainWindow().Show();
                    message.Close();
                };
                message.Cancel.Click += delegate (object sender1, RoutedEventArgs e1)
                {
                    // close the window and open main window
                    Close();
                    message.Hide();
                    new MainWindow().Show();
                    message.Close();
                };
                message.Show();
            }
            // if the game didn't start
            else if (Finish == true && Start == false)
            {
                // close the game
                Close();
                message = new MessageWindow(FinishMessage);
                message.Ok.Click += delegate (object sender1, RoutedEventArgs e1)
                {
                    message.Hide();
                    new MainWindow().Show();
                    message.Close();
                };
                message.Cancel.Click += delegate (object sender1, RoutedEventArgs e1)
                {
                    message.Hide();
                    new MainWindow().Show();
                    message.Close();
                };
                message.Show();
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the game.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // check whick key was pressed
            switch (e.Key)
            {
                case Key.Down:
                    _vm.Move(Direction.Down);
                    break;
                case Key.Up:
                    _vm.Move(Direction.Up);
                    break;
                case Key.Right:
                    _vm.Move(Direction.Right);
                    break;
                case Key.Left:
                    _vm.Move(Direction.Left);
                    break;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            MessageWindow msg = new MessageWindow("are you sure you want to go back to the main menu?");
            msg.Ok.Click += delegate (object sender1, RoutedEventArgs e1)
            {
                msg.Close();
                _vm.FinishGame();
            };
            msg.Cancel.Click += delegate (object sender1, RoutedEventArgs e1)
            {
                msg.Close();
                Show();
            };
            Hide();
            msg.Show();
        }
    }
}
