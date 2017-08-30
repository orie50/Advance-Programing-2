using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;

namespace ClientGUI.model
{
    /// <summary>
    /// abstract class for players model
    /// </summary>
    /// <seealso cref="ClientGUI.model.IPlayerModel" />
    public abstract class PlayerModel : IPlayerModel
    {
        /// <summary>
        /// The maze name
        /// </summary>
        protected string _mazeName;
        /// <summary>
        /// The rows
        /// </summary>
        protected int _rows;
        /// <summary>
        /// The cols
        /// </summary>
        protected int _cols;
        /// <summary>
        /// The maze
        /// </summary>
        protected Maze _maze;
        /// <summary>
        /// The ip
        /// </summary>
        protected string _ip;
        /// <summary>
        /// The port
        /// </summary>
        protected int _port;

        /// <summary>
        /// constructor of the <see cref="PlayerModel"/> class.
        /// </summary>
        public PlayerModel()
        {
            // initialize from default settings
            _ip = Properties.Settings.Default.ServerIP;
            _port = Properties.Settings.Default.ServerPort;
            _mazeName = "name";
            Rows = Properties.Settings.Default.MazeRows;
            Cols = Properties.Settings.Default.MazeCols;
        }

        /// <summary>
        /// Gets or sets the name of the maze.
        /// </summary>
        /// <value>
        /// The name of the maze.
        /// </value>
        public string MazeName
        {
            get
            {
                return _mazeName;
            }
            set
            {
                _mazeName = value;
            }
        }

        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>
        /// The rows.
        /// </value>
        public int Rows
        {
            get
            {
                return _rows;
            }
            set
            {
                _rows = value;
            }
        }

        /// <summary>
        /// Gets or sets the cols.
        /// </summary>
        /// <value>
        /// The cols.
        /// </value>
        public int Cols
        {
            get
            {
                return _cols;
            }
            set
            {
                _cols = value;
            }
        }
        /// <summary>
        /// Gets the maze.
        /// </summary>
        /// <value>
        /// The maze.
        /// </value>
        public string Maze
        {
            get
            {
                return _maze.ToString();
            }
        }

        /// <summary>
        /// Changes the position of the player.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="playerPosition">The current player position.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">wrond argument in Move</exception>
        public Position ChangePosition(Direction direction, Position playerPosition)
        {
            int row = playerPosition.Row;
            int col = playerPosition.Col;
            if (IsValidMove(direction, playerPosition))
            {
                switch (direction)
                {
                    case Direction.Up:
                        playerPosition = new Position(row - 1, col);
                        break;
                    case Direction.Down:
                        playerPosition = new Position(row + 1, col);
                        break;
                    case Direction.Right:
                        playerPosition = new Position(row, col + 1);
                        break;
                    case Direction.Left:
                        playerPosition = new Position(row, col - 1);
                        break;
                    default:
                        throw new Exception("wrond argument in Move");
                }
            }
            else
            {
                playerPosition = playerPosition;
            }
            return playerPosition;
        }

        /// <summary>
        /// check if the move is valid.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="playerPosition">The player position.</param>
        /// <returns>
        ///   <c>true</c> if [is valid move] [the specified direction]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.Exception">wrong argument in IsValidMove</exception>
        public bool IsValidMove(Direction direction, Position playerPosition)
        {
            int row = playerPosition.Row;
            int col = playerPosition.Col;
            try
            {
                switch (direction)
                {
                    case Direction.Up:
                        return _maze[row - 1, col] == CellType.Free;
                    case Direction.Down:
                        return _maze[row + 1, col] == CellType.Free;
                    case Direction.Right:
                        return _maze[row, col + 1] == CellType.Free;
                    case Direction.Left:
                        return _maze[row, col - 1] == CellType.Free;
                    default:
                        throw new Exception("wrong argument in IsValidMove");
                }
            }
			//in case the movement is outside the maze
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

    }
}
