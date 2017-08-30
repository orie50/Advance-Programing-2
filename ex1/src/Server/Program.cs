using Server.ClientHandlers;
using Server.Controllers;
using Server.Models;

namespace Server
{
    /// <summary>
    ///     main method class of the server
    /// </summary>
    internal class Program
    {
        /// <summary>
        ///     Main.
        /// </summary>
        /// <param name="args">The arguments for the main.</param>
        private static void Main(string[] args)
        {
            IModel model = new MazeModel();
            IController controller = new ServerController(model);
            IClientHandler ch = new ClientHandler(controller);
            Server server = new Server(ch);
            server.Start();
            server.Stop();
        }
    }
}