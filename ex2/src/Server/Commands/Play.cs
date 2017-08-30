using System.Net.Sockets;
using Server.Controllers;
using Server.Models;

namespace Server.Commands
{
    /// <summary>
    ///     implement play command
    /// </summary>
    /// <seealso cref="ICommand" />
    internal class Play : ICommand
    {
        private readonly IModel _model;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Play" /> class.
        /// </summary>
        /// <param name="model">The game controller.</param>
        public Play(IModel model)
        {
            _model = model;
        }

        /// <summary>
        ///     exectue the command according the arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="client">The client that send the command.</param>
        /// <returns>
        ///     the answer to the client
        /// </returns>
        public string Execute(string[] args, TcpClient client = null)
        {
            string name = args[1];
            string direction = args[0];
            return _model.AddMove(name, direction, client);
        }
    }
}