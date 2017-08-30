using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MazeLib;
using MazeMC.Models;

namespace MazeMC
{
    public class Game
    {
	    public delegate void OnGameStart(string player1, string player2);
	    public event OnGameStart GameStart;
		public delegate void OnNewState(string name, string player1, string player2);
	    public event OnNewState NewState;
        public delegate void OnGameFinish(string name, string winner, string loser);
        public event OnGameFinish GameFinish;
        private static Dictionary<string, Direction> _directions;

        /// <summary>
        ///     The output changes that have been made in the game
        /// </summary>
        private readonly ConcurrentQueue<Move> _changes;

        private bool _finishMessageSentToPlayers;
        private bool _gameFinished;
        private bool _isPlayer2Connected;
        private string _winner;
        /// <summary>
        ///     The input moves
        /// </summary>
        private readonly ConcurrentQueue<Move> _moves;
        private readonly List<string> _players;
        private readonly Dictionary<string, string> _usernames;
        private readonly List<Position> _positions;

        /// <summary>
        ///     The last client who read rhe changes
        /// </summary>
        private int _lastReaderIndex;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameController" /> class.
        /// </summary>
        /// <param name="maze">The maze.</param>
        /// <param name="model">The model.</param>
        public Game(Maze maze, MultiplayerModel model)
        {
            _finishMessageSentToPlayers = false;
            _directions = new Dictionary<string, Direction>
            {
                {Direction.Up.ToString(), Direction.Up},
                {Direction.Down.ToString(), Direction.Down},
                {Direction.Right.ToString(), Direction.Right},
                {Direction.Left.ToString(), Direction.Left}
            };

            _players = new List<string>();
            _usernames = new Dictionary<string, string>();
            _positions = new List<Position>();
            _moves = new ConcurrentQueue<Move>();
            _changes = new ConcurrentQueue<Move>();

            Maze = maze;
            _isPlayer2Connected = false;
            _gameFinished = false;
            _lastReaderIndex = -1;
        }

        public Maze Maze { get; }

		public List<string> Players => _players;

		/// <summary>
		///     Adds the player.
		/// </summary>
		/// <param name="client">The client.</param>
		public void AddPlayer(string clientId, string username)
        {
            Players.Add(clientId);
            _usernames.Add(clientId, username);
            _positions.Add(Maze.InitialPos);
	        if (Players.Count == 2)
	        {
				_isPlayer2Connected = true;
		        GameStart(_players[0], _players[1]);
			}
        }

        /// <summary>
        /// check if the alredy started.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the game started; otherwise, <c>false</c>.
        /// </returns>
        public bool IsStarted()
        {
            return Players.Count == 2;
        }

        /// <summary>
        ///     Initializes the game.
        ///     waiting for the next player to connect
        /// </summary>
        //todo add this again
        private void Initialize()
        {
            while (!_isPlayer2Connected)
                Thread.Sleep(10);
		}


        /// <summary>
        ///     Adds a move from a client.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="client">The client.</param>
        public Move AddMove(string direction, string client)
        {
            Direction dir = new Direction();
            switch (direction)
            {
                case "Up":
                    dir = Direction.Up;
                    break;
                case "Down":
                    dir = Direction.Down;
                    break;
                case "Right":
                    dir = Direction.Right;
                    break;
                case "Left":
                    dir = Direction.Left;
                    break;
            }
            int clientId = client.Equals(Players[0]) ? 0 : 1;
            Move move = new Move(dir, Maze.Name, clientId);
            _moves.Enqueue(move);
            return move;
        }

        /// <summary>
        ///     Gets the next game state.
        ///     responsible for passing the current state for both players before disposing it
        /// </summary>
        /// <param name="playerId">The player client.</param>
        /// <returns>
        ///     string represantation of the game state
        /// </returns>
        public string GetState(string playerId)
        {
            Move move;
	        _changes.TryDequeue(out move);
			return move.ToJson();
        }

        /// <summary>
        ///     Finishes the game.
        /// </summary>
        public void Finish(string winner)
        {
            _winner = winner;
            _gameFinished = true;
        }

        /// <summary>
        ///     Starts the game.
        ///     update the game state according to players movements
        /// </summary>
        public void Start()
        {
            new Task(() =>
            {
                while (!_gameFinished)
                {
                    while (_moves.IsEmpty && !_gameFinished)
                        Thread.Sleep(10);
                    Move move;
                    if (_moves.TryDequeue(out move))
                    {
                        int row = _positions[move.ClientId].Row;
                        int col = _positions[move.ClientId].Col;
                        // set new position
                        switch (move.MoveDirection)
                        {
                            case Direction.Up:
                                _positions[move.ClientId] = new Position(row - 1, col);
                                break;
                            case Direction.Down:
                                _positions[move.ClientId] = new Position(row + 1, col);
                                break;
                            case Direction.Right:
                                _positions[move.ClientId] = new Position(row, col + 1);
                                break;
                            case Direction.Left:
                                _positions[move.ClientId] = new Position(row, col - 1);
                                break;
                        }
                        _changes.Enqueue(move);
                        // add move event
                        NewState(Maze.Name, _players[0], _players[1]);
					}
                }
                //update _changes with an irelevant Move that closes the game
                _changes.Enqueue(new Move(Direction.Up, null));
                // add finish event
                if (_players[0].Equals(_winner)){
                    GameFinish(Maze.Name, _players[0], _players[1]);
                }
                else
                {
                    GameFinish(Maze.Name, _players[1], _players[0]);
                }
			}).Start();
        }

        /// <summary>
        /// Sets that finish message sent.
        /// </summary>
        public void SetFinishMessageSent()
        {
            _finishMessageSentToPlayers = true;
        }

        /// <summary>
        /// Gets the username by client id.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        public string GetUsernameById(string clientId)
        {
            string username;
            _usernames.TryGetValue(clientId, out username);
            return username;
        }

        /// <summary>
        /// check if both players pull the close message from the moves queue.
        /// </summary>
        /// <returns></returns>
        public bool BothFinish()
        {
            return _finishMessageSentToPlayers;
        }
    }
}
