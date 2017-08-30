using System.Net.Sockets;
using MazeLib;
using Server.Models;

namespace Server.Commands
{
    /// <summary>
    ///     implement generate command
    /// </summary>
    /// <seealso cref="ICommand" />
    internal class Generate : ICommand
    {
        /// <summary>
        ///     The model
        /// </summary>
        private readonly IModel _model;

        /// <summary>
        ///     constructor of the <see cref="Generate" /> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public Generate(IModel model)
        {
            _model = model;
        }

        /// <summary>
        ///     exectue generate command according the arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="client">The client that send the command.</param>
        /// <returns>
        ///     the maze details
        /// </returns>
        public string Execute(string[] args, TcpClient client = null)
        {
            if (args.Length != 3)
                return "wrong arguments";
            // pars the args
            string name = args[0];
            int rows = int.Parse(args[1]);
            int cols = int.Parse(args[2]);

            // craete new maze
            Maze maze = _model.GenerateMaze(name, rows, cols);
            if (maze == null)
                return "name: " + name + " already taken";
            // return the maze details
            return maze.ToJSON();
        }
    }
}