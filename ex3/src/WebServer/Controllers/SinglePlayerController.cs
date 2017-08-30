using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using MazeAdapterLib;
using MazeLib;
using MazeMC.Models;
using Newtonsoft.Json.Linq;
using WebServer.Models;

namespace WebServer.Controllers
{
	/// <summary>
	/// web api controller for single player
	/// </summary>
	/// <seealso cref="System.Web.Http.ApiController" />
	public class SinglePlayerController : ApiController
    {
		/// <summary>
		/// The Single Player model
		/// </summary>
		private static SingleplayerModel model = new SingleplayerModel();

		/// <summary>
		/// Generates the maze.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="row">The row.</param>
		/// <param name="col">The col.</param>
		/// <returns> maze json object</returns>
		[HttpGet]
		[Route("SinglePlayer/{name}/{row}/{col}")]
	    public JObject GenerateMaze(string name, int row, int col)
	    {
		    Maze maze = model.GenerateMaze(name, row, col);
		    JObject obj;
			if (maze == null)
			{
				return null;
			}
			obj = JObject.Parse(maze.ToJSON());
		    return obj;
	    }

		/// <summary>
		/// Solves the maze.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="algorithm">The algorithm.</param>
		/// <returns> json lists of directions </returns>
		[HttpGet]
	    [Route("SinglePlayer/{name}/{algorithm}")]
		public JObject SolveMaze(string name, Algorithm algorithm)
	    {
		    MazeSolution solution = model.SolveMaze(name, algorithm);
		    JObject obj = JObject.Parse(solution.ToJson());
		    return obj;
	    }
	}
}