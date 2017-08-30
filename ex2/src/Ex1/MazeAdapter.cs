using System.Collections.Generic;
using MazeLib;
using SearchAlgorithmsLib;

namespace Ex1
{
    /// <summary>
    ///     wrap the maze with the solve of ISearchable logic
    /// </summary>
    /// <seealso cref="Position" />
    public class MazeAdapter : ISearchable<Position>
    {
        /// <summary>
        ///     The maze
        /// </summary>
        private readonly Maze _maze;

        /// <summary>
        ///     constructor of the <see cref="MazeAdapter" /> class.
        /// </summary>
        /// <param name="maze">The maze.</param>
        public MazeAdapter(Maze maze)
        {
            _maze = maze;
        }

        /// <summary>
        ///     get a state and return all the possible state that next to it (according the Isearchable).
        /// </summary>
        /// <param name="state">The current state.</param>
        /// <returns>
        ///     return list of the states
        /// </returns>
        public List<State<Position>> GetAllPossibleState(State<Position> state)
        {
            List<State<Position>> succesors = new List<State<Position>>();
            State<Position> succesor;
            State<Position> parent = state.CameFrom;
            CellType free = CellType.Free;
            int row = state.Data.Row;
            int col = state.Data.Col;
            float cost = state.Cost + 1;

            if (row + 1 < _maze.Rows && _maze[row + 1, col] == free)
            {
                succesor = new State<Position>(new Position(row + 1, col));
                if (parent == null || !succesor.Equals(parent))
                {
                    succesor.CameFrom = state;
                    succesor.Cost = cost;
                    succesors.Add(succesor);
                }
            }
            if (col + 1 < _maze.Cols && _maze[row, col + 1] == free)
            {
                succesor = new State<Position>(new Position(row, col + 1));
                if (parent == null || !succesor.Equals(parent))
                {
                    succesor.CameFrom = state;
                    succesor.Cost = cost;
                    succesors.Add(succesor);
                }
            }
            if (row - 1 >= 0 && _maze[row - 1, col] == free)
            {
                succesor = new State<Position>(new Position(row - 1, col));
                if (parent == null || !succesor.Equals(parent))
                {
                    succesor.CameFrom = state;
                    succesor.Cost = cost;
                    succesors.Add(succesor);
                }
            }
            if (col - 1 >= 0 && _maze[row, col - 1] == free)
            {
                succesor = new State<Position>(new Position(row, col - 1));
                if (parent == null || !succesor.Equals(parent))
                {
                    succesor.CameFrom = state;
                    succesor.Cost = cost;
                    succesors.Add(succesor);
                }
            }
            return succesors;
        }

        /// <summary>
        ///     Get goal state.
        /// </summary>
        /// <returns>
        ///     return the goal state
        /// </returns>
        public State<Position> GetGoalState()
        {
            return new State<Position>(_maze.GoalPos);
        }

        /// <summary>
        ///     Gets the state of the inintial.
        /// </summary>
        /// <returns>
        ///     return the initial state
        /// </returns>
        public State<Position> GetInintialState()
        {
            return new State<Position>(_maze.InitialPos);
        }
    }
}