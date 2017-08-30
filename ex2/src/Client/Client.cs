using System.IO;
using System.Net;
using System.Net.Sockets;
using MessagingLib;

namespace Client
{
    /// <summary>
    ///     encapsulate the client capabilities
    /// </summary>
    public class Client
    {
        private NetworkStream _stream;
        private MessageReader _reader;
        private MessageWriter _writer;
        private TcpClient _client;
        private int _port;
        private string _ip;

        /// <summary>
        /// constructor of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="ip">The ip.</param>
        public Client(int port, string ip)
        {
            _port = port;
            _ip = ip;
        }

        /// <summary>
        /// Initialize the client.
        /// </summary>
        public void Initialize()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(_ip), _port);
            _client = new TcpClient();
            _client.Connect(ep);
            _stream = _client.GetStream();
            _reader = new MessageReader(new StreamReader(_stream));
            _writer = new MessageWriter(new StreamWriter(_stream));
        }

        /// <summary>
        /// Close the client.
        /// </summary>
        public void Close()
        {
            if (_stream != null)
            {
                _stream.Close();
                _reader.Close();
                _writer.Close();
                _client.Close();
            }
        }

        /// <summary>
        /// Send given message.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public void Send(string msg)
        {
            _writer.WriteMessage(msg);
        }

        /// <summary>
        /// Recieve message.
        /// </summary>
        /// <returns></returns>
        public string Recieve()
        {
            return _reader.ReadMessage();
        }
    }
}