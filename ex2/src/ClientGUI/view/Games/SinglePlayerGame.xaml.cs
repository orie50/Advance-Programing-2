using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using ClientGUI.view.Menus;
using ClientGUI.view_model;
using Ex1;
using MazeLib;

namespace ClientGUI.view.Games
{
	/// <summary>
	/// single player game window
	/// </summary>
	/// <seealso cref="System.Windows.Window" />
	/// <seealso cref="System.Windows.Markup.IComponentConnector" />
	public partial class SinglePlayerGame : Window
	{
		/// <summary>
		/// The view-model
		/// </summary>
		private SinglePlayerViewModel _vm;
		/// <summary>
		/// The animation timer
		/// </summary>
		private DispatcherTimer _timer;
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SinglePlayerGame"/> is finish.
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
			DependencyProperty.Register("Finish", typeof(bool), typeof(SinglePlayerGame), new PropertyMetadata(FinishPropertyChanged));
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SinglePlayerGame"/> is start.
		/// </summary>
		/// <value>
		///   <c>true</c> if start; otherwise, <c>false</c>.
		/// </value>
		public bool Start
		{
			get { return (bool)GetValue(StartProperty); }
			set { SetValue(StartProperty, value); }
		}
		/// <summary>
		/// The start property
		/// </summary>
		public static readonly DependencyProperty StartProperty =
			DependencyProperty.Register("Start", typeof(bool), typeof(SinglePlayerGame), new PropertyMetadata(StartPropertyChanged));
		/// <summary>
		/// Initializes a new instance of the <see cref="SinglePlayerGame"/> class.
		/// </summary>
		/// <param name="vm">The vm.</param>
		public SinglePlayerGame(SinglePlayerViewModel vm)
		{
			InitializeComponent();
			_vm = vm;
			_timer = null;
			DataContext = _vm;
			// binding for start boolean in view model
			Binding binding = new Binding();
			binding.Path = new PropertyPath("Start");
			binding.Source = vm;  
			BindingOperations.SetBinding(this ,StartProperty ,binding);
			// binding for finish boolean in view model
			binding = new Binding();
			binding.Path = new PropertyPath("Finish");
			binding.Source = vm;
			BindingOperations.SetBinding(this, FinishProperty, binding);

		    // binding for finish boolean in view model
		    binding = new Binding();
		    binding.Path = new PropertyPath("Connect");
		    binding.Source = vm;
		    BindingOperations.SetBinding(this, ConnectProperty, binding);


            Board.DataContext = _vm;
		}

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SinglePlayerGame"/> is connect.
        /// </summary>
        /// <value>
        ///   <c>true</c> if connect; otherwise, <c>false</c>.
        /// </value>
        public bool Connect
	    {
	        get { return (bool)GetValue(ConnectProperty); }
	        set { SetValue(ConnectProperty, value); }
	    }

        /// <summary>
        /// The finish message property
        /// </summary>
        public static readonly DependencyProperty ConnectProperty =
	        DependencyProperty.Register("ConnectMessage", typeof(bool), typeof(SinglePlayerGame));

        /// <summary>
        /// Starts the game.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void StartPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	    {
			// checks if the game started. and initialize it
		    if ((bool) e.NewValue)
		    {
			    ((SinglePlayerGame)d).StartGame();
			}
		}
		/// <summary>
		/// Finishes the game.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void FinishPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((SinglePlayerGame)d).FinishGame();
		}
		/// <summary>
		/// Finishes the game.
		/// </summary>
		private void FinishGame()
		{
			MessageWindow message;
            // if connection failed
		    if (!Connect)
		    {
		        message = new MessageWindow("Connection Failed");
		        message.Ok.Click += delegate (object sender1, RoutedEventArgs e1)
		        {
		            Close();
		            message.Hide();
		            new MainWindow().Show();
		            message.Close();
		        };
		        message.Cancel.Click += delegate (object sender1, RoutedEventArgs e1)
		        {
		            message.Close();
		        };
		        message.Show();
            }
			// if the game started and ended correctly, show a wining message
			else if (Finish == true && Start == true)
			{
				message = new MessageWindow("You Won!");
				message.Ok.Click += delegate (object sender1, RoutedEventArgs e1)
				{
					Close();
					message.Hide();
					new MainWindow().Show();
					message.Close();
				};
				message.Cancel.Click += delegate (object sender1, RoutedEventArgs e1)
				{
					message.Close();
				};
				message.Show();
			}
			// if the game only ended but was never started, show an error message
			else if (Finish == true && Start == false)
			{
				Close();
				message = new MessageWindow("Name already exist!");
				message.Ok.Click += delegate (object sender1, RoutedEventArgs e1)
				{
					message.Hide();
					new SinglePlayerMenu().Show();
					message.Close();
				};
				message.Cancel.Click += delegate (object sender1, RoutedEventArgs e1)
				{
				    message.Hide();
                    new SinglePlayerMenu().Show();
                    message.Close();
                };
				message.Show();
			}
		}
		/// <summary>
		/// Handles the KeyDown event of the Window control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
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
		/// Starts the game.
		/// </summary>
		private void StartGame()
		{
			Show();
			Board.DrawMaze();
			Board.RefreshMaze();
		}
		/// <summary>
		/// Starts the animation.
		/// </summary>
		private void StartAnimation()
		{
			if (_timer != null)
			{
				KeyDown -= Window_KeyDown;
				_timer.Start();
			}
		}
		/// <summary>
		/// Pauses the animation.
		/// </summary>
		private void PauseAnimation()
		{
			if (_timer != null && _timer.IsEnabled)
			{
				_timer.Stop();
				KeyDown += Window_KeyDown;
			}
		}
		/// <summary>
		/// Handles the Click event of the SolveMaze btn control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
		private void btnSolveMaze_Click(object sender, RoutedEventArgs e)
        {
			//stop listening to key events
            KeyDown -= Window_KeyDown;
            MazeSolution solution = _vm.SolveMaze();
            if (solution == null)
            {
                return;
            }
            _vm.RestartGame();
            IEnumerator<Direction> directions = solution.GetEnumerator();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.1);
            _timer.Tick += delegate (object sender1, EventArgs e1)
            {
                if (directions.MoveNext())
                {
                    switch (directions.Current)
                    {
                        case Direction.Down:
                            _vm.Move(Direction.Down);
                            break;
                        case Direction.Up:
                            _vm.Move(Direction.Up);
                            break;
                        case Direction.Right:
                            _vm.Move(Direction.Right);
                            break;
                        case Direction.Left:
                            _vm.Move(Direction.Left);
                            break;
                    }
                }
                else //the maze is solved
                {
					PauseAnimation();
                    _timer = null;
                }
            };
            _timer.Start();
        }
		/// <summary>
		/// Handles the Click event of the RestartGame btn control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
		private void btnRestartGame_Click(object sender, RoutedEventArgs e)
        {
            MessageWindow msg = new MessageWindow("are you sure you want to restart the game?");
	        PauseAnimation();
			msg.Ok.Click += delegate (object sender1, RoutedEventArgs e1)
            {
                _vm.RestartGame();
                msg.Close();
                Show();
            };
            msg.Cancel.Click += delegate (object sender1, RoutedEventArgs e1)
            {
                msg.Close();
                Show();
                StartAnimation();
            };
            Hide();
            msg.Show();
        }
		/// <summary>
		/// Handles the Click event of the Menu btn control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
		private void btnMenu_Click(object sender, RoutedEventArgs e)
        {

            MessageWindow msg = new MessageWindow("are you sure you want to go back to the main menu?");
            PauseAnimation();
            msg.Ok.Click += delegate (object sender1, RoutedEventArgs e1)
            {
                msg.Close();
				Hide();
                new MainWindow().Show();
	            Close();
			};
            msg.Cancel.Click += delegate (object sender1, RoutedEventArgs e1)
            {
                msg.Close();
                Show();
		        StartAnimation();
	        };
            Hide();
            msg.Show();
        }
    }
}
