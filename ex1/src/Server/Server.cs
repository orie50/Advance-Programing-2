using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using Server.ClientHandlers;

namespace Server
{
    /// <summary>
    ///     the server class, manage all the tcp connection
    /// </summary>
    internal class Server
    {
        /// <summary>
        ///     The client handler type
        /// </summary>
        private readonly IClientHandler _ch;

        /// <summary>
        ///     The tcp listener
        /// </summary>
        private TcpListener _listener;

        /// <summary>
        ///     constructor of the <see cref="Server" /> class.
        /// </summary>
        /// <param name="ch">The client handler.</param>
        public Server(IClientHandler ch)
        {
            _ch = ch;
        }

        /// <summary>
        ///     Start the server.
        /// </summary>
        public void Start()
        {
            // initialize
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ConfigurationManager.AppSettings[0]),
                int.Parse(ConfigurationManager.AppSettings[1]));
            _listener = new TcpListener(ep);
            _listener.Start();
            Console.WriteLine("start get connections");
            while (true)
            {
                try
                {
                    // get new connection with client
                    TcpClient client = _listener.AcceptTcpClient();
                    // give to the client handler to maanage the communication with the client
                    _ch.HandleClient(client);
                }
                catch (SocketException)
                {
                }
            }
        }

        /// <summary>
        ///     Stop the server.
        /// </summary>
        public void Stop()
        {
            _listener.Stop();
        }
    }
}