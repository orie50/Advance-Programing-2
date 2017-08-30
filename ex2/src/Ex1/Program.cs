using System;
using MazeGeneratorLib;
using MazeLib;
using SearchAlgorithmsLib;
using SearchAlgorithmsLib.Algorithms;

namespace Ex1
{
    /// <summary>
    ///     main class: create maze and solve it with BFS and DFS algorithms
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            // the maze size 100x100
            int col = 100;
            int row = 100;

            // create maze with DFSMazeGenerator
            DFSMazeGenerator generator = new DFSMazeGenerator();
            Maze maze = generator.Generate(col, row);

            // print the maze
            string mazeString = maze.ToString();
            Console.WriteLine(mazeString);

            // adapt the maze and solve it with BFS
            ISearchable<Position> adapter = new MazeAdapter(maze);
            ISearcher<Position> bfsSearcher = new BestFirstSearch<Position>();
            bfsSearcher.Search(adapter);

            int bfsNumOfStases = bfsSearcher.GetNumberOfNodesEvaluated();

            // solve the maze with DFS
            ISearcher<Position> dfsSearcher = new DepthFirstSearch<Position>();
            dfsSearcher.Search(adapter);

            int dfsNumOfStases = dfsSearcher.GetNumberOfNodesEvaluated();

            // print the num of evalueted nodes for BFS and DFS
            Console.WriteLine("number of BFS states:" + bfsNumOfStases);
            Console.WriteLine("number of DFS states:" + dfsNumOfStases);

            Console.Read();
        }
    }
}