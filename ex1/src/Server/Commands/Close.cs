using System.Net.Sockets;
using Server.Models;

namespace Server.Commands
{
    internal class Close : ICommand
    {
        /// <summary>
        ///     The model
        /// </summary>
        private readonly IModel _model;

        public Close(IModel model)
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
            if (args.Length != 1)
                return "wrong arguments";
            string name = args[0];
            _model.FinishGame(name, client);
            return "close";
        }
    }
}