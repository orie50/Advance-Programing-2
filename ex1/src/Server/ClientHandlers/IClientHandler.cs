using System.Net.Sockets;

namespace Server.ClientHandlers
{
    /// <summary>
    ///     interface of the client handler
    /// </summary>
    internal interface IClientHandler
    {
        /// <summary>
        ///     Handles the client.
        /// </summary>
        /// <param name="client">The client.</param>
        void HandleClient(TcpClient client);
    }
}