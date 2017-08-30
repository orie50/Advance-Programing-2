using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ClientGUI.view;

namespace ClientGUI.controls
{
	/// <summary>
	/// user control for maze drawing
	/// </summary>
	/// <seealso cref="System.Windows.Controls.UserControl" />
	/// <seealso cref="System.Windows.Markup.IComponentConnector" />
	public partial class MazeBoard : UserControl
	{
		/// <summary>
		/// The player rectangle picture
		/// </summary>
		private readonly Rectangle _player;

		/// <summary>
		/// Gets or sets the maze.
		/// </summary>
		/// <value>
		/// string represantation of the maze.
		/// </value>
		public string Maze
		{
			get { return (string)GetValue(MazeProperty); }
			set { SetValue(MazeProperty, value); }
		}
		/// <summary>
		/// The maze property
		/// </summary>
		public static readonly DependencyProperty MazeProperty =
			DependencyProperty.Register("Maze", typeof(string), typeof(MazeBoard), new PropertyMetadata(OnMazePropertyChanged));

		/// <summary>
		/// Called when [maze property changed].
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnMazePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((MazeBoard)d).RefreshMaze();
		}
		/// <summary>
		/// Gets or sets the rows.
		/// </summary>
		/// <value>
		/// number of rows.
		/// </value>
		public int Rows
		{
			get { return (int)GetValue(RowsProperty); }
			set { SetValue(RowsProperty, value); }
		}
		/// <summary>
		/// The rows property.
		/// </summary>
		public static readonly DependencyProperty RowsProperty =
			DependencyProperty.Register("Rows", typeof(int), typeof(MazeBoard));
		/// <summary>
		/// Gets or sets the cols.
		/// </summary>
		/// <value>
		/// nomber of columns.
		/// </value>
		public int Cols
		{
			get { return (int)GetValue(ColsProperty); }
			set { SetValue(ColsProperty, value); }
		}
		/// <summary>
		/// The cols property
		/// </summary>
		public static readonly DependencyProperty ColsProperty =
			DependencyProperty.Register("Cols", typeof(int), typeof(MazeBoard));
		/// <summary>
		/// Initializes a new instance of the <see cref="MazeBoard"/> class.
		/// </summary>
		public MazeBoard()
		{
			InitializeComponent();
			_player = new Rectangle();
			_player.Stroke = Brushes.Gray;
		}
		/// <summary>
		/// Refreshes the maze drawing.
		/// Draws only the player.
		/// </summary>
		public void RefreshMaze()
		{
			_player.Height = Canvas.Height / Rows;
			_player.Width = Canvas.Width / Cols;
			double left = 0;
			double top = 0;
			bool newLine = false;
			Canvas.Children.Remove(_player);
			foreach (char c in Maze)
			{
				switch (c)
				{
					case '*':
					case '#':
					case '0':
					case '1':
						left += _player.Width;
						break;
					case '2': //symbols the player location with right orientation
						_player.Fill = (ImageBrush)Resources["DaveRight"];
						Canvas.SetLeft(_player, left);
						Canvas.SetTop(_player, top);
						Panel.SetZIndex(_player, 1);
						Canvas.Children.Add(_player);
						return;
					case '3': //symbols the player location with left orientation
						_player.Fill = (ImageBrush)Resources["DaveLeft"];
						Canvas.SetLeft(_player, left);
						Canvas.SetTop(_player, top);
						Panel.SetZIndex(_player, 1);
						Canvas.Children.Add(_player);
						return;
					default: //step over white space, and draw in a new line
						if (Char.IsWhiteSpace(c) && newLine == false)
						{
							newLine = true;
							left = 0;
							top += _player.Height;
						}
						else
						{
							newLine = false;
						}
						continue;
				}
			}
		}
		/// <summary>
		/// Draws the maze.
		/// </summary>
		public void DrawMaze()
		{
			Canvas.Children.Clear();
			double left = 0;
			double top = 0;
			bool newLine = false;
			foreach (char c in Maze)
			{
				Rectangle rect = new Rectangle();
				rect.Stroke = Brushes.Gray;
				rect.Height = Canvas.Height / Rows;
				rect.Width = Canvas.Width / Cols;
				switch (c)
				{
					case '*':
						rect.Fill = Brushes.Black;
						break;
					case '#':
						rect.Fill = (ImageBrush)Resources["Goal"];
						break;
					case '0':
						rect.Fill = Brushes.Black;
						break;
					case '1':
						rect.Fill = (ImageBrush)Resources["Wall"];
						break;
					case '2': 
						rect.Fill = Brushes.Black;
						break;
					case '3':
						rect.Fill = Brushes.Black;
						break;
					default:
						if (Char.IsWhiteSpace(c) && newLine == false)
						{
							newLine = true;
							left = 0;
							top += rect.Height;
						}
						else
						{
							newLine = false;
						}
						continue;
				}
				Canvas.SetLeft(rect, left);
				Canvas.SetTop(rect, top);
				Canvas.Children.Add(rect);
				left += rect.Width;
			}
		}
		/// <summary>
		/// Handles the SizeChanged event of the Canvas control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="SizeChangedEventArgs"/> instance containing the event data.</param>
		private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (Maze != null)
			{
				DrawMaze();
				RefreshMaze();
			}
		}
	}
}