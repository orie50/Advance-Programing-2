using System.Net.Sockets;
using Server.Models;

namespace Server.Commands
{
    /// <summary>
    ///     implement join command
    /// </summary>
    /// <seealso cref="ICommand" />
    internal class Join : ICommand
    {
        /// <summary>
        ///     The model
        /// </summary>
        private readonly IModel _model;

        /// <summary>
        ///     constructor of the <see cref="Join" /> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public Join(IModel model)
        {
            _model = model;
        }

        /// <summary>
        ///     exectue join command according the arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="client">The client that send the command.</param>
        /// <returns>
        ///     the maze details
        /// </returns>
        public string Execute(string[] args, TcpClient client = null)
        {
            if (args.Length != 1)
                return "wrong arguments";
            string name = args[0];
            return _model.JoinGame(name, client);
        }
    }
}