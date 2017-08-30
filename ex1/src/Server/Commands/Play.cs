using System.Net.Sockets;
using Server.Controllers;

namespace Server.Commands
{
    /// <summary>
    ///     implement play command
    /// </summary>
    /// <seealso cref="ICommand" />
    internal class Play : ICommand
    {
        private readonly GameController _gameController;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Play" /> class.
        /// </summary>
        /// <param name="gameController">The game controller.</param>
        public Play(GameController gameController)
        {
            _gameController = gameController;
        }

        private string Direction { set; get; }

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
            if (args.Length != 1)
                return "wrong arguments";
            Direction = args[0];
            return _gameController.AddMove(Direction, client);
        }
    }
}