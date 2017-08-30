using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;

namespace ClientGUI.model
{
	/// <summary>
	/// interface of games model
	/// </summary>
	interface IPlayerModel
    {
		/// <summary>
		/// Gets or sets the name of the maze.
		/// </summary>
		/// <value>
		/// The name of the maze.
		/// </value>
		string MazeName { get; set; }
		/// <summary>
		/// Gets or sets the rows.
		/// </summary>
		/// <value>
		/// The rows.
		/// </value>
		int Rows { get; set; }
		/// <summary>
		/// Gets or sets the cols.
		/// </summary>
		/// <value>
		/// The cols.
		/// </value>
		int Cols { get; set; }
		/// <summary>
		/// Gets the maze.
		/// </summary>
		/// <value>
		/// The maze.
		/// </value>
		string Maze { get;}
		/// <summary>
		/// Changes the position of the player.
		/// </summary>
		/// <param name="direction">The direction.</param>
		/// <param name="playerPosition">The current player position.</param>
		/// <returns></returns>
		Position ChangePosition(Direction direction, Position playerPosition);
		/// <summary>
		/// Determines whether [is valid move] [the specified direction] [from the current player position].
		/// </summary>
		/// <param name="direction">The direction.</param>
		/// <param name="playerPosition">The player position.</param>
		/// <returns>
		///   <c>true</c> if [is valid move] [the specified direction]; otherwise, <c>false</c>.
		/// </returns>
		bool IsValidMove(Direction direction, Position playerPosition);

    }
}
