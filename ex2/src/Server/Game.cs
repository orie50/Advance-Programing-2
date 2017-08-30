using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MazeLib;
using Server.ClientHandlers;
using Server.Controllers;
using Server.Models;

namespace Server
{
    class Game
    {
        private static Dictionary<string, Direction> _directions;

        /// <summary>
        ///     The output changes that have been made in the game
        /// </summary>
        private readonly ConcurrentQueue<Move> _changes;

        private int _playersReadCloseMessage;
        private bool _gameFinished;
        private bool _isPlayer2Connected;
        
        /// <summary>
        ///     The input moves
        /// </summary>
        private readonly ConcurrentQueue<Move> _moves;

        private readonly List<TcpClient> _players;
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
        public Game(Maze maze, MazeModel model)
        {
            _playersReadCloseMessage = 0;
            _directions = new Dictionary<string, Direction>
            {
                {Direction.Up.ToString(), Direction.Up},
                {Direction.Down.ToString(), Direction.Down},
                {Direction.Right.ToString(), Direction.Right},
                {Direction.Left.ToString(), Direction.Left}
            };

            _players = new List<TcpClient>();
            _positions = new List<Position>();
            _moves = new ConcurrentQueue<Move>();
            _changes = new ConcurrentQueue<Move>();

            Maze = maze;
            _isPlayer2Connected = false;
            _gameFinished = false;
            _lastReaderIndex = -1;
        }

        public Maze Maze { get; }

        /// <summary>
        ///     Adds the player.
        /// </summary>
        /// <param name="client">The client.</param>
        public void AddPlayer(TcpClient client)
        {
            _players.Add(client);
            _positions.Add(Maze.InitialPos);
            if (_players.Count == 2)
                _isPlayer2Connected = true;
        }

        /// <summary>
        /// check if the alredy started.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the game started; otherwise, <c>false</c>.
        /// </returns>
        public bool IsStarted()
        {
            return _players.Count == 2;
        }

        /// <summary>
        ///     Initializes the game.
        ///     waiting for the next player to connect
        /// </summary>
        public void Initialize(IClientHandler playerHandler)
        {
            while (!_isPlayer2Connected)
                Thread.Sleep(10);
            playerHandler.HandleClient(_players[0]);
            playerHandler.HandleClient(_players[1]);
        }


        /// <summary>
        ///     Adds a move from a client.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="client">The client.</param>
        public string AddMove(string direction, TcpClient client)
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
            int clientId = client == _players[0] ? 0 : 1;
            Move move = new Move(dir, Maze.Name, clientId);
            _moves.Enqueue(move);
            return move.ToJson();
        }

        /// <summary>
        ///     Gets the next game state.
        ///     responsible for passing the current state for both players before disposing it
        /// </summary>
        /// <param name="playerClient">The player client.</param>
        /// <returns>
        ///     string represantation of the game state
        /// </returns>
        public string GetState(TcpClient playerClient)
        {
            int indexOfClient = _players.IndexOf(playerClient);
            //sleep while the client already read the next state, ot rhe state hasn't changed
            while (_changes.Count == 0 || _lastReaderIndex == indexOfClient)
                Thread.Sleep(10);

            Move move;
            lock (this)
            {
                // if the next state wasn't already read by one the players
                if (_lastReaderIndex == -1)
                {
                    _changes.TryPeek(out move);
                    _lastReaderIndex = indexOfClient;
                }
                else // if the next state was already read by one the players, dispose it
                {
                    _changes.TryDequeue(out move);
                    _lastReaderIndex = -1;
                }
	            //case of closing state represented by irrelevant move
	            if (move.ClientId == -1) _playersReadCloseMessage++;
			}
	        if (move.ClientId == -1) return "close";
			return move.ToJson();
        }

        /// <summary>
        ///     Finishes the game.
        /// </summary>
        public void Finish()
        {
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
                    }
                }
                //update _changes with an irelevant Move that closes the game
                _changes.Enqueue(new Move(Direction.Up, null));
            }).Start();
        }

        /// <summary>
        /// check if both players pull the close message from the moves queue.
        /// </summary>
        /// <returns></returns>
        public bool BothFinish()
        {
            return _playersReadCloseMessage == 2;
        }
    }
}
