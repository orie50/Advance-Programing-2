using System.Collections.Generic;
using System.Net.Sockets;
using MazeAdapterLib;
using MazeLib;

namespace MazeMC.Models
{
    /// <summary>
    ///     the model interface
    /// </summary>
    internal interface IModel
    {
        /// <summary>
        ///     Generates the maze.
        /// </summary>
        /// <param name="name">The maze name.</param>
        /// <param name="row">The row.</param>
        /// <param name="col">The col.</param>
        /// <returns>
        ///     maze
        /// </returns>
        Maze GenerateMaze(string name, int row, int col);

        /// <summary>
        ///     Solves the maze.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="algorithm">The algorithm.</param>
        /// <returns> the maze solution</returns>
        MazeSolution SolveMaze(string name, Algorithm algorithm);

        /// <summary>
        ///     create new game.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="cols">The cols.</param>
        /// <param name="player1">client.</param>
        /// <returns> the maze detailes</returns>
        string NewGame(string name, int rows, int cols, TcpClient player1);

        /// <summary>
        ///     player 2 join the game.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="player2">The player2.</param>
        /// <returns> the maze detailes </returns>
        string JoinGame(string name, TcpClient player2);

		/// <summary>
		///     Creates list of active game.
		/// </summary>
		/// <returns> list of games names</returns>
		string CreateList();

        /// <summary>
        ///     Finishes the game.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="client">The client.</param>
        void FinishGame(string name, TcpClient client);

        /// <summary>
        /// Adds move to a specific game
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="client">The client.</param>
        string AddMove(string name, string direction, TcpClient client);

        /// <summary>
        /// Gets the state of a specifc game
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="client">The client.</param>
        string GetState(string name, TcpClient client);
    }
}