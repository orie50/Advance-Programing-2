using System.Net.Sockets;
using Server.Models;

namespace Server.Commands
{
    /// <summary>
    ///     implement start command
    /// </summary>
    /// <seealso cref="ICommand" />
    internal class Start : ICommand
    {
        /// <summary>
        ///     The model
        /// </summary>
        private readonly IModel _model;

        /// <summary>
        ///     constructor of the <see cref="Start" /> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public Start(IModel model)
        {
            _model = model;
        }

        /// <summary>
        ///     exectue start command according the arguments.
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
            // parse the args
            string name = args[0];
            int rows = int.Parse(args[1]);
            int cols = int.Parse(args[2]);

            return _model.NewGame(name, rows, cols, client);
        }
    }
}