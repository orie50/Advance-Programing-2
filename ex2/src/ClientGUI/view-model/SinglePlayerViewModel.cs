using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using ClientGUI.model;
using ClientGUI.view;
using Ex1;
using MazeLib;

namespace ClientGUI
{
	/// <summary>
	/// Single Player View Model
	/// </summary>
	/// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
	public class SinglePlayerViewModel :INotifyPropertyChanged
    {
		/// <summary>
		/// The model
		/// </summary>
		private readonly SinglePlayerModel _model;
		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		/// <summary>
		/// direction of the last move
		/// </summary>
		private Direction _lastMove;
        /// <summary>
        /// finish flag property
        /// </summary>

        private bool _connect;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SinglePlayerViewModel"/> is finish.
        /// </summary>
        /// <value>
        ///   <c>true</c> if finish; otherwise, <c>false</c>.
        /// </value>
        public bool Connect
        {
            get { return _connect; }
            set
            {
                _connect = value;
                OnPropertyChanged("Connect");
            }
        }

        private bool _finish;
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SinglePlayerViewModel"/> is finish.
		/// </summary>
		/// <value>
		///   <c>true</c> if finish; otherwise, <c>false</c>.
		/// </value>
		public bool Finish
        {
            get { return _finish; }
            set
            {
	            _finish = value;
				OnPropertyChanged("Finish");
            }
        }
		/// <summary>
		/// start flag property
		/// </summary>
		private bool _start;
	    public bool Start
	    {
		    get { return _start; }
		    set
		    {
			    _start = value;
				OnPropertyChanged("Start");
		    }
	    }
		/// <summary>
		/// Gets or sets the name of the maze.
		/// </summary>
		/// <value>
		/// The name of the maze.
		/// </value>
		public string MazeName
		{
			get { return _model.MazeName; }
			set
			{
				if (_model.MazeName != value)
				{
					_model.MazeName = value;
					OnPropertyChanged("MazeName");
				}
			}
		}
		/// <summary>
		/// The maze serialization
		/// </summary>
		private StringBuilder _mazeSrl;
		/// <summary>
		/// Gets the maze SRL.
		/// </summary>
		/// <value>
		/// The maze SRL.
		/// </value>
		public string MazeSrl
		{
			get
			{
			    if (_mazeSrl == null)
			    {
			        return null;
			    } 
				return _mazeSrl.ToString();
			}
		}
		/// <summary>
		/// Gets or sets the rows.
		/// </summary>
		/// <value>
		/// The rows.
		/// </value>
		public int Rows
		{
			get { return _model.Rows; }
			set
			{
				_model.Rows = value;
				OnPropertyChanged("Rows");
			}
		}
		/// <summary>
		/// Gets or sets the cols.
		/// </summary>
		/// <value>
		/// The cols.
		/// </value>
		public int Cols
		{
			get { return _model.Cols; }
			set
			{
				_model.Cols = value;
				OnPropertyChanged("Cols");
			}
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="SinglePlayerViewModel"/> class.
		/// </summary>
		/// <param name="model">The model.</param>
		public SinglePlayerViewModel(SinglePlayerModel model)
        {
            _model = model;
			_lastMove = Direction.Right;
	        _start = false;
	        _finish = false;
			//subscribe to the model events
			_model.NewMaze += new EventHandler<Maze>(delegate (Object sender, Maze e) {
				if (e != null)
				{
					_mazeSrl = new StringBuilder(e.ToString());
					_mazeSrl[e.InitialPos.Row * (Cols + 2) + e.InitialPos.Col] = '2';
					OnPropertyChanged("MazeSrl");
					Start = true;
				}
				else
				{
					Finish = true;
				}
			});
            Connect = true;
			_model.PlayerMoved += new EventHandler<Position>(delegate (Object sender, Position e) {
				_mazeSrl = new StringBuilder(((SinglePlayerModel)sender).Maze);
				switch (_lastMove)
				{
					case Direction.Right:
						_mazeSrl[e.Row * (Cols + 2) + e.Col] = '2';
						break;
					case Direction.Left:
						_mazeSrl[e.Row * (Cols + 2) + e.Col] = '3';
						break;
				}
				OnPropertyChanged("MazeSrl");
			});
            _model.FinishGame += new EventHandler<string>(delegate(Object sender, string e)
            {
                if (e.Equals("Connection Failed"))
                {
                    Connect = false;
                }
	            Finish = true;
            });
        }
		/// <summary>
		/// Called when [property changed].
		/// </summary>
		/// <param name="name">The name.</param>
		private void OnPropertyChanged(string name)
        {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
		/// <summary>
		/// ask the model to generate a maze
		/// </summary>
		public void GenerateMaze()
        {
            _model.GenerateMaze();
        }
		/// <summary>
		/// ask the model to solves the maze.
		/// </summary>
		/// <returns></returns>
		public MazeSolution SolveMaze()
        {
            return _model.SolveMaze();
        }
		/// <summary>
		/// Restarts the game.
		/// </summary>
		public void RestartGame()
		{
			_model.RestartGame();
		}
		/// <summary>
		/// Moves in a specified direction.
		/// </summary>
		/// <param name="direction">The direction.</param>
		public void Move(Direction direction)
		{
			// update the last orientation of the player, for correct drawing
			if (direction == Direction.Right || direction == Direction.Left)
			{
				_lastMove = direction;
			}
			_model.Move(direction);
		}
    }
}