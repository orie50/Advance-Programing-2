using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using ClientGUI.model;
using MazeLib;

namespace ClientGUI.view_model
{
    /// <summary>
    /// the multi player view model
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class MultiPlayerViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The model
        /// </summary>
        private readonly MultiPlayerModel _model;
        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// The last move
        /// </summary>
        private Direction _lastMove;
        /// <summary>
        /// The other last move
        /// </summary>
        private Direction _otherLastMove;

        /// <summary>
        /// The games list
        /// </summary>
        private ObservableCollection<String> _gamesList;
        /// <summary>
        /// Gets or sets the games list.
        /// </summary>
        /// <value>
        /// The games list.
        /// </value>
        public ObservableCollection<String> GamesList
        {
            get { return _gamesList; }
            set
            {
                if (_gamesList != value)
                {
                    _gamesList = value;
                    OnPropertyChanged("GamesList");
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the join game.
        /// </summary>
        /// <value>
        /// The name of the join game.
        /// </value>
        public string JoinName
        {
            get { return _model.JoinName; }
            set
            {
                _model.JoinName = value;
                OnPropertyChanged("JoinName");
            }
        }

        /// <summary>
        /// check if the game finished
        /// </summary>
        private bool _finish;
        /// <summary>
        /// Gets or sets finish.
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
        /// The finish message
        /// </summary>
        private string _finishMessage;
        /// <summary>
        /// Gets or sets the finish message.
        /// </summary>
        /// <value>
        /// The finish message.
        /// </value>
        public string FinishMessage
        {
            get { return _finishMessage; }
            set
            {
                _finishMessage = value;
                OnPropertyChanged("FinishMessage");
            }
        }

        /// <summary>
        /// check if the game started
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
        /// The maze SRL
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
        /// The other maze SRL
        /// </summary>
        private StringBuilder _otherMazeSrl;
        /// <summary>
        /// Gets the other maze SRL.
        /// </summary>
        /// <value>
        /// The other maze SRL.
        /// </value>
        public string OtherMazeSrl
        {
            get
            {
                if (_otherMazeSrl == null)
                {
                    return null;
                }
                return _otherMazeSrl.ToString();
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
        /// constructor of the <see cref="MultiPlayerViewModel"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public MultiPlayerViewModel(MultiPlayerModel model)
        {
            _model = model;
            _lastMove = Direction.Right;
            _otherLastMove = Direction.Right;
            // new game event
            _model.NewMaze += new EventHandler<Maze>(delegate (Object sender, Maze e) {
                if (e != null)
                {
                    // create the player position
                    _mazeSrl =
                        new StringBuilder(e.ToString()) {[e.InitialPos.Row * (Cols + 2) + e.InitialPos.Col] = '2'};
                    OnPropertyChanged("MazeSrl");
                    _otherMazeSrl =
                        new StringBuilder(e.ToString()) {[e.InitialPos.Row * (Cols + 2) + e.InitialPos.Col] = '2'};
                    OnPropertyChanged("OtherMazeSrl");
                    // start the game
                    Start = true;
                }
                else
                {
                    // finish the game
                    Finish = true;
                }
            });
            // player moved event
            _model.PlayerMoved += new EventHandler<Position>(delegate (Object sender, Position e) {
                // update the maze SRL
                _mazeSrl = new StringBuilder(((MultiPlayerModel)sender).Maze);
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
            // other player moved event
            _model.OtherPlayerMoved += new EventHandler<Position>(delegate (Object sender, Position e) {
                // update the other player maze SRL
                _otherMazeSrl = new StringBuilder(((MultiPlayerModel)sender).Maze);
                switch (_otherLastMove)
                {
                    case Direction.Right:
                        _otherMazeSrl[e.Row * (Cols + 2) + e.Col] = '2';
                        break;
                    case Direction.Left:
                        _otherMazeSrl[e.Row * (Cols + 2) + e.Col] = '3';
                        break;
                }
                OnPropertyChanged("OtherMazeSrl");
            });
            // finish the game event
            _model.FinishGame += new EventHandler<string>(delegate (Object sender, string e)
            {
                // finish the game
                FinishMessage = e;
                Finish = true;
            });
            // create the list
            _gamesList = _model.CreateList();
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
        /// Move the player.
        /// </summary>
        /// <param name="direction">The direction.</param>
        public void Move(Direction direction)
        {
            if (direction == Direction.Right || direction == Direction.Left)
            {
                _lastMove = direction;
            }
            _model.Move(direction);
        }

        /// <summary>
        /// Start new game.
        /// </summary>
        public void StartGame()
        {
            _model.StartGame();
        }

        /// <summary>
        /// Join existed game.
        /// </summary>
        public void JoinGame()
        {
            _model.JoinGame();
        }

        /// <summary>
        /// Finish the game.
        /// </summary>
        public void FinishGame()
        {
            _model.CloseGame();
        }
    }
}
