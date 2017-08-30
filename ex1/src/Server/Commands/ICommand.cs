using System.Net.Sockets;

namespace Server.Commands
{
    /// <summary>
    ///     interface of commands
    /// </summary>
    internal interface ICommand
    {
        /// <summary>
        ///     exectue the command according the arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="client">The client that send the command.</param>
        /// <returns>
        ///     the answer to the client
        /// </returns>
        string Execute(string[] args, TcpClient client = null);
    }
}