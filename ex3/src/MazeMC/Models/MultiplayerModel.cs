using System.Collections.Generic;
using MazeAdapterLib;
using MazeGeneratorLib;
using MazeLib;
using Newtonsoft.Json;

namespace MazeMC.Models
{
    public class MultiplayerModel
    {
        public delegate void OnGameStart(string player1, string player2);
        public event OnGameStart GameStart;
        public delegate void OnNewState(string name, string player1, string player2);
        public event OnNewState NewState;
        public delegate void OnGameFinish(string name, string winner, string loser);
        public event OnGameFinish GameFinish;
        private readonly DFSMazeGenerator _generator;

        /// <summary>
        ///     The mazes cache
        /// </summary>
        private readonly Dictionary<string, Maze> _mazes;

        /// <summary>
        ///     The solutions cache
        /// </summary>
        private readonly Dictionary<string, MazeSolution> _solutions;

        /// <summary>
        ///     The multiplayer games cache
        /// </summary>
        private readonly Dictionary<string, Game> _games;

        /// <summary>
        /// Constructor of the <see cref="MultiplayerModel"/> class.
        /// </summary>
        public MultiplayerModel()
        {
            _mazes = new Dictionary<string, Maze>();
            _games = new Dictionary<string, Game>();
            _solutions = new Dictionary<string, MazeSolution>();
            _generator = new DFSMazeGenerator();
        }

        /// <summary>
        ///     Creates list of active game.
        /// </summary>
        /// <returns>
        ///     list of games names
        /// </returns>
        public string CreateList()
        {
            List<string> names = new List<string>(_games.Keys.Count);
            foreach (string name in _games.Keys)
            {
                if (!_games[name].IsStarted()) names.Add(name);
            }
            return JsonConvert.SerializeObject(names, Formatting.Indented);
        }

        /// <summary>
        ///     create new game.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="cols">The cols.</param>
        /// <param name="playerId">clientId.</param>
        /// <returns>
        ///     the maze detailes
        /// </returns>
        public Maze NewGame(string name, int rows, int cols, string playerId, string username)
        {
            // check if the game exist
            if (_games.ContainsKey(name))
                return null;
            // else create new game
            Maze maze = _generator.Generate(rows, cols);
            maze.Name = name;
            Game game = new Game(maze, this);
            // add delegate of new state
            game.NewState += new Game.OnNewState(delegate (string gameName, string player1, string player2)
            {
                NewState(gameName, player1, player2);
            });
            // add delegate of start game
            game.GameStart += new Game.OnGameStart(delegate (string player1, string player2)
            {
                GameStart(player1, player2);
            });
            // add delegate of finish game
            game.GameFinish += new Game.OnGameFinish(delegate (string gameName, string player1, string player2)
            {
                GameFinish(gameName, player1, player2);
            });
            _games.Add(name, game);
            game.AddPlayer(playerId, username);
            game.Start();
            return maze;
        }

        /// <summary>
        ///     player 2 join the game.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="player2">The player2.</param>
        /// <returns>
        ///     the maze detailes
        /// </returns>
        public Maze JoinGame(string name, string player2, string username)
        {
            Game game;
            if (_games.TryGetValue(name, out game))
            {
                if (game.IsStarted())
                {
                    return null;
                }
                game.AddPlayer(player2, username);
                return game.Maze;
            }
            return null;
        }

        /// <summary>
        ///     notify the game to finish.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="client">The clientId.</param>
        public void FinishGame(string name, string client)
        {
            _games[name].Finish(client);
            while (!_games[name].BothFinish())
            {
                System.Threading.Thread.Sleep(10);
            }
            _games.Remove(name);
        }

        /// <summary>
        /// Gets the username by id.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        public string GetUsernameById(string name, string clientId)
        {
            return _games[name].GetUsernameById(clientId);
        }

        /// <summary>
        /// Sets that finish message sent.
        /// </summary>
        /// <param name="name">The name.</param>
        public void SetFinishMessageSent(string name)
        {
            _games[name].SetFinishMessageSent();
        }

        /// <summary>
        /// Adds move to the given game
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="clientId">The clientId.</param>
        /// <returns></returns>
        public Move AddMove(string name, string direction, string clientId)
        {
            return _games[name].AddMove(direction, clientId);
        }

        /// <summary>
        /// Get state from given game
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="clientId">The clientId.</param>
        /// <returns></returns>
        public string GetState(string name, string clientId)
        {
            return _games[name].GetState(clientId);
        }
    }
}
