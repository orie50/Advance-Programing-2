using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeLib;
using Newtonsoft.Json.Linq;
using SearchAlgorithmsLib;

namespace Ex1
{
    /// <summary>
    ///     adapter for the maze soltion.
    ///     enables the conversion of the original ISolution
    ///     into direction list, and enables serialization
    /// </summary>
    public class MazeSolution : IEnumerable<Direction>
    {
        private readonly string _name;
        private readonly int _nodesEvaluated;
        private readonly Stack<Direction> _solution;


        /// <summary>
        ///     Initializes a new instance of the <see cref="MazeSolution" /> class.
        /// </summary>
        /// <param name="name">The maze name.</param>
        /// <param name="sol">The original solution.</param>
        /// <param name="nodesEvaluated">number of nodes evaluated.</param>
        public MazeSolution(string name, ISolution<Position> sol, int nodesEvaluated)
        {
            _name = name;
            _solution = ToDirections(sol);
            _nodesEvaluated = nodesEvaluated;
        }

        private MazeSolution(string name, Stack<Direction> sol, int nodesEvaluated)
        {
            _name = name;
            _solution = sol;
            _nodesEvaluated = nodesEvaluated;
        }

        /// <summary>
        ///     deserialize MazeSolution From json string.
        /// </summary>
        /// <param name="str">json represantation of MazeSolution.</param>
        /// <returns>
        ///     MazeSolution object
        /// </returns>
        public static MazeSolution FromJson(string str)
        {
            JObject mazeSolution = JObject.Parse(str);
            string name = (string) mazeSolution["Name"];
            string directions = (string) mazeSolution["Solution"];
            List<Direction> sol = new List<Direction>(directions.Length);
            foreach (char direction in directions)
                switch (direction)
                {
                    case '0':
                        sol.Add(Direction.Left);
                        break;
                    case '1':
                        sol.Add(Direction.Right);
                        break;
                    case '2':
                        sol.Add(Direction.Up);
                        break;
                    case '3':
                        sol.Add(Direction.Down);
                        break;
                }
            int nodesEvaluated = (int) mazeSolution["NodesEvaluated"];
            return new MazeSolution(name, new Stack<Direction>(sol), nodesEvaluated);
        }

		public IEnumerator<Direction> GetEnumerator()
		{
			return ((IEnumerable<Direction>)_solution).GetEnumerator();
		}

		/// <summary>
		///     serialize MazeSolution to json string.
		/// </summary>
		/// <returns>
		///     json represantation of MazeSolution.
		/// </returns>
		public string ToJson()
        {
            JObject mazeSolution = new JObject
            {
                ["Name"] = _name,
                ["NodesEvaluated"] = _nodesEvaluated
            };
            StringBuilder solution = new StringBuilder();
            foreach (Direction d in _solution)
                solution.Append((int) d);
            mazeSolution["Solution"] = solution.ToString();
           
            return mazeSolution.ToString();
        }

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Direction>)_solution).GetEnumerator();
		}

		/// <summary>
		///     converts the position list to direction list
		/// </summary>
		/// <param name="sol">The sol.</param>
		/// <returns></returns>
		private Stack<Direction> ToDirections(ISolution<Position> sol)
        {
            List<Direction> directions = new List<Direction>(sol.Count() - 1);
            Position last = sol.Get().Data;
            foreach (State<Position> state in sol)
            {
                Position next = state.Data;
                int nextRow = next.Row;
                int nextCol = next.Col;
                int lastRow = last.Row;
                int lastCol = last.Col;

                if (nextRow > lastRow)
                    directions.Add(Direction.Down);
                else if (nextRow < lastRow)
                    directions.Add(Direction.Up);
                else if (nextCol < lastCol)
                    directions.Add(Direction.Left);
                else
                    directions.Add(Direction.Right);
                last = next;
            }
            return new Stack<Direction>(directions);
        }
    }
}