using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using ClientGUI.view;
using MazeLib;
using Newtonsoft.Json;

namespace ClientGUI.model
{
    public class MultiPlayerModel : PlayerModel
    {
        /// <summary>
        /// Occurs when [new maze].
        /// </summary>
        public event EventHandler<Maze> NewMaze;
        /// <summary>
        /// Occurs when [player moved].
        /// </summary>
        public event EventHandler<Position> PlayerMoved;
        /// <summary>
        /// Occurs when [other player moved].
        /// </summary>
        public event EventHandler<Position> OtherPlayerMoved;
        /// <summary>
        /// Occurs when [finish game].
        /// </summary>
        public event EventHandler<string> FinishGame;
        /// <summary>
        /// The client
        /// </summary>
        private Client.Client _client;
        /// <summary>
        /// The client id
        /// </summary>
        private int _clientId;
        /// <summary>
        /// if this player close the gmae
        /// </summary>
        private bool _closed;

        private bool _connectToServer;

        /// <summary>
        /// The join game name
        /// </summary>
        private string _joinName;
        /// <summary>
        /// Gets or sets the join game name.
        /// </summary>
        /// <value>
        /// The join game name.
        /// </value>
        public string JoinName
        {
            get
            {
                return _joinName;
            }
            set
            {
                _joinName = value;
            }
        }

        /// <summary>
        /// constructor of the <see cref="MultiPlayerModel"/> class.
        /// </summary>
        public MultiPlayerModel()
        {
            _mazeName = "name";
            Rows = Properties.Settings.Default.MazeRows;
            Cols = Properties.Settings.Default.MazeCols;
            _client = new Client.Client(_port, _ip);
            _closed = false;
            _connectToServer = true;
        }

        /// <summary>
        /// The player position
        /// </summary>
        private Position _playerPos;
        /// <summary>
        /// Gets or sets the player position.
        /// </summary>
        /// <value>
        /// The player position.
        /// </value>
        private Position PlayerPos
        {
            get
            {
                return _playerPos;
            }
            set
            {
                _playerPos = value;
                PlayerMoved(this, _playerPos);
            }
        }

        /// <summary>
        /// The other player position
        /// </summary>
        private Position _otherPlayerPos;
        /// <summary>
        /// Gets or sets the other player position.
        /// </summary>
        /// <value>
        /// The other player position.
        /// </value>
        private Position OtherPlayerPos
        {
            get
            {
                return _otherPlayerPos;
            }
            set
            {
                _otherPlayerPos = value;
                OtherPlayerMoved(this, _otherPlayerPos);
            }
        }

        /// <summary>
        /// Starts new game.
        /// </summary>
        public void StartGame()
        {
            MessageWindow waitMessage = new MessageWindow("Wait to second player");
            try
            {
                _client.Initialize();
                // send start message
                string msg = CreateStartMessage();
                _client.Send(msg);
                waitMessage.Show();
                // recieve aanswer from server
                string answer = _client.Recieve();
                // if the game name alredy exist
                if (answer.Equals("name: " + MazeName + " alredy taken"))
                {
                    FinishGame(this, answer);
                }
                else
                {
                    // create maze from the answer
                    _maze = MazeLib.Maze.FromJSON(answer);
                    _playerPos = _maze.InitialPos;
                    _otherPlayerPos = _maze.InitialPos;
                    // the host player
                    _clientId = 0;
                    // new task for listenning to server
                    new Task(() => Listen()).Start();
                    // new maze event
                    NewMaze(this, _maze);
                }
            }
            catch
            {
                _client.Close();
                FinishGame(this, "Coneection Failed");
            }
            waitMessage.Close();
        }

        /// <summary>
        /// Join to given game.
        /// </summary>
        public void JoinGame()
        {
            try
            {
                _client.Initialize();
                // send join message
                string msg = CreateJoinMessage();
                _client.Send(msg);
                // recieve aanswer from server
                string answer = _client.Recieve();
                // if the game not exist
                if (_joinName == null || answer.Equals("the name: " + _joinName + " does not exist"))
                {
                    FinishGame(this, "the name: " + _joinName + " does not exist");
                }
                // if the game is full
                else if (answer.Equals("game: " + _joinName + " is full"))
                {
                    FinishGame(this, answer);
                }
                else
                {
                    // create maze from the answer
                    _maze = MazeLib.Maze.FromJSON(answer);
                    Rows = _maze.Rows;
                    Cols = _maze.Cols;
                    _playerPos = _maze.InitialPos;
                    _otherPlayerPos = _maze.InitialPos;
                    // the geust player
                    _clientId = 1;
                    // new task for listenning to server
                    new Task(() => Listen()).Start();
                    // new maze event
                    NewMaze(this, _maze);

                }
            }
            catch
            {
                _client.Close();
                FinishGame(this, "Coneection Failed");
            }
        }

        /// <summary>
        /// Listen to server.
        /// </summary>
        public void Listen()
        {
            try
            {
                // while the connection 
                while (true)
                {
                    // recieve maessage from server
                    string answer = _client.Recieve();
                    try
                    {
                        // check if it's a move
                        Move move = ClientGUI.Move.FromJson(answer);
                        if (move.ClientId == _clientId)
                        {
                            // move the player
                            PlayerPos = ChangePosition(move.MoveDirection, PlayerPos);
                            if (PlayerPos.Equals(_maze.GoalPos))
                            {
                                // if get to end position
                                CloseGame();
                            }
                        }
                        else
                        {
                            // move the other player
                            OtherPlayerPos = ChangePosition(move.MoveDirection, OtherPlayerPos);
                        }
                    }
                    // if the connection failed
                    catch
                    {
                        if (answer.Contains("close"))
                        {
                            // close the connection
                            _client.Close();
                            // if the player won
                            if (PlayerPos.Equals(_maze.GoalPos))
                            {
                                FinishGame(this, "You Won!");
                            }
                            // if other player won
                            else if (OtherPlayerPos.Equals(_maze.GoalPos))
                            {
                                FinishGame(this, "You Lose!");
                            }
                            // if the other player close the game
                            else if (_closed == false)
                            {
                                FinishGame(this, "The Second Player Disconnect!");
                            }
                            // if the player close the game
                            else
                            {
                                FinishGame(this, "");
                            }
                            break;
                        }
                        _connectToServer = false;
                        FinishGame(this, "Connection failed");
                    }
                }
            }
            catch
            {
                _connectToServer = false;
                _client.Close();
                FinishGame(this, "Connection failed");
            }
        }

        /// <summary>
        /// Create the start message.
        /// </summary>
        /// <returns></returns>
        public string CreateStartMessage()
        {
            return "start " + _mazeName + " " + _rows.ToString() + " " + _cols.ToString();
        }

        /// <summary>
        /// Create the join message.
        /// </summary>
        /// <returns></returns>
        public string CreateJoinMessage()
        {
            return "join " + _joinName;
        }

        /// <summary>
        /// send move message.
        /// </summary>
        /// <param name="direction">The direction.</param>
        public void Move(Direction direction)
        {
            string msg = "play " + direction.ToString();
            _client.Send(msg);
        }

        /// <summary>
        /// Create the list of games.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<string> CreateList()
        {
            try
            {
                // create client nd send list request
                Client.Client client = new Client.Client(_port, _ip);
                client.Initialize();
                client.Send("list");
                string answer = client.Recieve();
                client.Close();
                // return the list
                if (!answer.Equals("no games avaliable"))
                {
                    return JsonConvert.DeserializeObject<ObservableCollection<string>>(answer);
                }
            }
            catch
            {
                _connectToServer = false;
                _client.Close();
                FinishGame(this, "Coneection Failed");
            }
            return null;
        }

        /// <summary>
        /// send Close message.
        /// </summary>
        public void CloseGame()
        {
            _closed = true;
            if(_connectToServer)
                _client.Send("close");
        }
    }
}
