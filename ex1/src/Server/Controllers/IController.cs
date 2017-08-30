using System.Net.Sockets;

namespace Server.Controllers
{
    /// <summary>
    ///     interface of the controller
    /// </summary>
    internal interface IController
    {
        /// <summary>
        ///     exectue methods according the arguments.
        /// </summary>
        /// <param name="commandLine">The arguments.</param>
        /// <param name="client">The client that send the command.</param>
        /// <returns>
        ///     the answer of the execute
        /// </returns>
        string ExecuteCommand(string commandLine, TcpClient client = null);
    }
}