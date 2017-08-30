using System;
using System.Windows;
using ClientGUI.view;
using Ex1;
using MazeLib;

namespace ClientGUI.model
{
	/// <summary>
	/// single player model
	/// </summary>
	/// <seealso cref="ClientGUI.model.PlayerModel" />
	public class SinglePlayerModel : PlayerModel
    {
		/// <summary>
		/// Occurs when [new maze] is created.
		/// </summary>
		public event EventHandler<Maze> NewMaze;
		/// <summary>
		/// Occurs when [player moved].
		/// </summary>
		public event EventHandler<Position> PlayerMoved;
		/// <summary>
		/// Occurs when the game should be finished.
		/// </summary>
		public event EventHandler<string> FinishGame;
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

        public bool ConnectToServer { get; set; }

		/// <summary>
		/// Moves to the specified direction.
		/// </summary>
		/// <param name="direction">The direction.</param>
		public void Move(Direction direction)
        {
            PlayerPos = ChangePosition(direction, PlayerPos);
		    if (PlayerPos.Equals(_maze.GoalPos) )
		    {
			    FinishGame(this, "You Won!");
		    }
		}
		/// <summary>
		/// Restarts the game.
		/// </summary>
		public void RestartGame()
		{
			PlayerPos = _maze.InitialPos;
		}

        /// <summary>
        /// Generates the maze.
        /// </summary>
        public void GenerateMaze()
        {
            try
            {
                Client.Client client = new Client.Client(_port, _ip);
                client.Initialize();
                string msg = CreateGenerateMessage();
                client.Send(msg);
                string answer = client.Recieve();
                client.Close();
                if (answer.Equals("name: " + MazeName + " already taken"))
                {
                    FinishGame(this, answer);
                }
                else
                {
                    _maze = MazeLib.Maze.FromJSON(answer);
                    _playerPos = _maze.InitialPos;
                    NewMaze(this, _maze);
                }
            }
            catch
            {
                ConnectToServer = false;
                FinishGame(this, "Connection Failed");
            }
        }

        /// <summary>
		/// Creates the generate message.
		/// </summary>
		/// <returns></returns>
		private string CreateGenerateMessage()
        {
            return "generate " + _mazeName + " " + _rows.ToString() + " " + _cols.ToString();
		}

        /// <summary>
        /// Solves the maze.
        /// </summary>
        /// <returns></returns>
        public MazeSolution SolveMaze()
        {
            try
            {
                Client.Client client = new Client.Client(_port, _ip);
                client.Initialize();
                string msg = CreateSolveMessage();
                client.Send(msg);
                string answer = client.Recieve();
                client.Close();
                return MazeSolution.FromJson(answer);
            }
            catch
            {
                ConnectToServer = false;
                FinishGame(this, "Connection Failed");
            }
            return null;
        }

        /// <summary>
		/// Creates the solve message.
		/// </summary>
		/// <returns></returns>
		private string CreateSolveMessage()
        {
            int algorithm = Properties.Settings.Default.SearchAlgorithm;
            return "solve " + _mazeName + " " + algorithm.ToString();
        }
    }
}
