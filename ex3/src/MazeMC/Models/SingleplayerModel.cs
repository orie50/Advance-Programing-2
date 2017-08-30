using System;
using System.Collections.Generic;
using System.Net.Sockets;
using MazeAdapterLib;
using MazeGeneratorLib;
using MazeLib;
using Newtonsoft.Json;
using SearchAlgorithmsLib;
using SearchAlgorithmsLib.Algorithms;
	

namespace MazeMC.Models
{
	/// <summary>
	///     algorithm enum according to the exercise specs
	/// </summary>
	public enum Algorithm
	{
		Bfs,
		Dfs
	}

	public class SingleplayerModel
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
		private readonly Dictionary<string, Game> _games;

		public SingleplayerModel()
		{
			_mazes = new Dictionary<string, Maze>();
			_games = new Dictionary<string, Game>();
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
            Maze maze = null;
            _mazes.TryGetValue(name, out maze);
            if (maze != null)
				return maze;
			maze = _generator.Generate(col, row);
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
				ISolution<Position> sol = _algorithms[(int)algorithm].Search(adapter);
				solution = new MazeSolution(name, sol, _algorithms[(int)algorithm].GetNumberOfNodesEvaluated());
				_solutions.Add(name, solution);
				return solution;
			}
			Console.WriteLine("the maze: " + name + " does not exist");
			//TODO: handle a case when the maze does not exist
			return null;
		}
	}
}
