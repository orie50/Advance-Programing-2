using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Ex1;
using MazeGeneratorLib;
using MazeLib;
using Newtonsoft.Json;
using SearchAlgorithmsLib;
using SearchAlgorithmsLib.Algorithms;
using Server.Controllers;

namespace Server.Models
{
    /// <summary>
    ///     algorithm enum according to the exercise specs
    /// </summary>
    internal enum Algorithm
    {
        Bfs,
        Dfs
    }

    /// <summary>
    ///     encasulates all the logic of maze gaming
    /// </summary>
    /// <seealso cref="IModel" />
    internal class MazeModel : IModel
    {
        private readonly ISearcher<Position>[] _algorithms;
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
        private readonly Dictionary<string, GameController> _games;

        public MazeModel()
        {
            _mazes = new Dictionary<string, Maze>();
            _games = new Dictionary<string, GameController>();
            _solutions = new Dictionary<string, MazeSolution>();
            _generator = new DFSMazeGenerator();
            _algorithms = new ISearcher<Position>[2];
            _algorithms[0] = new BestFirstSearch<Position>();
            _algorithms[1] = new DepthFirstSearch<Position>();
        }

        /// <summary>
        ///     Generates the maze.
        /// </summary>
        /// <param name="name">The maze name.</param>
        /// <param name="row">number of rows.</param>
        /// <param name="col">number of cols.</param>
        /// <returns>
        ///     maze
        /// </returns>
        public Maze GenerateMaze(string name, int row, int col)
        {
            if (_mazes.ContainsKey(name))
                return null;
            Maze maze = _generator.Generate(col, row);
            maze.Name = name;
            //save the maze
            _mazes.Add(name, maze);
            return maze;
        }

        /// <summary>
        ///     Solves the maze.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="algorithm">The algorithm.</param>
        /// <returns>
        ///     the maze solution
        /// </returns>
        public MazeSolution SolveMaze(string name, Algorithm algorithm)
        {
            MazeSolution solution;
            // try searching the solution in the cache
            if (_solutions.TryGetValue(name, out solution))
                return solution;
            Maze maze;
            if (_mazes.TryGetValue(name, out maze))
            {
                ISearchable<Position> adapter = new MazeAdapter(maze);
                ISolution<Position> sol = _algorithms[(int) algorithm].Search(adapter);
                solution = new MazeSolution(name, sol, _algorithms[(int) algorithm].GetNumberOfNodesEvaluated());
                _solutions.Add(name, solution);
                return solution;
            }
            Console.WriteLine("the maze: " + name + "does not exist");
            //TODO: handle a case when the maze does not exist
            return null;
        }

        /// <summary>
        ///     Creates list of active game.
        /// </summary>
        /// <returns>
        ///     list of games names
        /// </returns>
        public string CreateList()
        {
            if (_games.Count == 0)
                return "no games avaliable";
            List<string> names = new List<string>(_games.Keys.Count);
            foreach (string name in _games.Keys)
                names.Add(name);
            return JsonConvert.SerializeObject(names, Formatting.Indented);
        }

        /// <summary>
        ///     create new game.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="cols">The cols.</param>
        /// <param name="player1">client.</param>
        /// <returns>
        ///     the maze detailes
        /// </returns>
        public string NewGame(string name, int rows, int cols, TcpClient player1)
        {
            Maze maze = _generator.Generate(rows, cols);
            maze.Name = name;
            GameController game = new GameController(name, maze, this);
            game.AddPlayer(player1);
            _games.Add(name, game);

            game.Initialize();
            game.Start();

            return maze.ToJSON();
        }

        /// <summary>
        ///     player 2 join the game.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="player2">The player2.</param>
        /// <returns>
        ///     the maze detailes
        /// </returns>
        public string JoinGame(string name, TcpClient player2)
        {
            GameController game;
            if (_games.TryGetValue(name, out game))
            {
                if (game.IsStarted())
                {
                    return "game" + name + "is full";
                }
                game.AddPlayer(player2);
                return game.Maze.ToJSON();
            }
            return "the name: " + name + "does not exist";
        }

        /// <summary>
        ///     notify the game to finish.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="client">The client.</param>
        public void FinishGame(string name, TcpClient client)
        {
            _games[name].Finish();
            _games.Remove(name);
        }
    }
}