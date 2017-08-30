using System.Net.Sockets;
using Ex1;
using Server.Models;

namespace Server.Commands
{
    /// <summary>
    ///     implement solve command
    /// </summary>
    /// <seealso cref="ICommand" />
    internal class Solve : ICommand
    {
        /// <summary>
        ///     The model
        /// </summary>
        private readonly IModel _model;

        /// <summary>
        ///     constructor of the <see cref="Solve" /> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public Solve(IModel model)
        {
            _model = model;
        }

        /// <summary>
        ///     exectue solve command according the arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="client">The client that send the command.</param>
        /// <returns>
        ///     the maze solution
        /// </returns>
        public string Execute(string[] args, TcpClient client = null)
        {
            if (args.Length != 2)
                return "wrong arguments";
            string name = args[0];
            bool type;
            // parse the algorithm and send to model to solve
            if (!bool.TryParse(args[1], out type))
            {
                Algorithm alg = type ? Algorithm.Dfs : Algorithm.Bfs;
                MazeSolution solution = _model.SolveMaze(name, alg);
                return solution.ToJson();
            }
            return null;
        }
    }
}