using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Server.Controllers;

namespace Server.ClientHandlers
{
    /// <summary>
    ///     encapsulate a player handeling
    /// </summary>
    /// <seealso cref="IClientHandler" />
    internal class PlayerHandler : IClientHandler
    {
        private readonly GameController _gameController;

        public PlayerHandler(GameController game)
        {
            _gameController = game;
        }

        /// <summary>
        ///     Handles the player.
        /// </summary>
        /// <param name="client">The client.</param>
        public void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);
            // read requsets from the client and procces them
            Task read = new Task(() =>
            {
                string execute = "";
                do
                {
                    try
                    {
                        // try get input from client
                        string input = reader.ReadLine();
                        execute = _gameController.ExecuteCommand(input, client);
                    }
                    catch (Exception e)
                    {
                        // close the connection
                        execute = "close";
                    }
                    // while the requst isn't closing request
                } while (!execute.Equals("close"));
            });

            // send the client the game state until a closing state is reached
            Task write = new Task(() =>
            {
                string output;
                {
                    do
                    {
                        output = _gameController.GetState(client);
                        writer.WriteLine(output);
                        writer.Flush();
                    } while (!output.Equals("close"));
                    stream.Close();
                    reader.Close();
                    writer.Close();
                    client.Close();
                }
            });

            read.Start();
            write.Start();
        }
    }
}