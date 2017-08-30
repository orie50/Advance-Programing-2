using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    ///     encapsulate the client capabilities
    /// </summary>
    internal class Client
    {
        private StreamReader _reader;
        private NetworkStream _stream;
        private StreamWriter _writer;
        private int clientId = -1;
        private bool alreadyWrite = false;
        /// <summary>
        ///     Starts this session
        /// </summary>
        public void Start()
        {
            // initilaize the tcp end point
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ConfigurationManager.AppSettings[0]),
                int.Parse(ConfigurationManager.AppSettings[1]));
            string answer = "";
            string msg;
            string commandLine = "";
            while (true)
            {
                // connect to the server
                TcpClient client = new TcpClient();
                client.Connect(ep);
                _stream = client.GetStream();
                _reader = new StreamReader(_stream);
                _writer = new StreamWriter(_stream);
                Task write = null;
                // task for read answers from the server
                Task read = new Task(() =>
                {
                    do
                    {
                        answer = "";
                        do
                        {
                            // read 1 answer
                            msg = _reader.ReadLine();
                            answer += msg;
                            if (msg != null && !msg.EndsWith("\n"))
                            {
                                answer += '\n';
                            }
                        } while (msg != null && !msg.Equals("}") && !msg.Equals("]"));
                        try
                        {
                            // check if it's a move
                            Move move = Move.FromJson(answer);
                            if (move.ClientId != clientId)
                                Console.WriteLine(move.ToString());
                        }
                        catch
                        {
                            if (!answer.Contains("close"))
                            {
                                Console.Write(answer);
                            }
                            // if game end
                            else
                            {
                                Console.WriteLine("Game ended!");
                            }
                            // if it's not multiple game break
                            if (!CheckIfMultiple(commandLine, answer))
                            {
                                break;
                            }
                        }
                    } while (!answer.Contains("close"));
                });
                // task for write a command to the server
                if (!alreadyWrite)
                {
                    alreadyWrite = true;
                    write = new Task(() =>
                    {
                        while (true)
                        {
                            commandLine = Console.ReadLine();
                            _writer.WriteLine(commandLine);
                            _writer.Flush();
                        }
                    });
                    write.Start();
                }
                read.Start();
                // wait to answer from server
                read.Wait();
                _stream.Close();
                _reader.Close();
                _writer.Close();
                client.Close();
            }
        }

        /// <summary>
        /// Check if start multiple game.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="answer">The answer.</param>
        /// <returns> return true if it's multiple game, otherwise return false</returns>
        public bool CheckIfMultiple(string command, string answer)
        {
            if (command.Contains("start") || command.Contains("join"))
                if (!answer.Contains("does not exist") && !answer.Contains("wrong arguments"))
                {
                    if (command.Contains("start"))
                    {
                        // if it's the first playar
                        clientId = 0;
                    }
                    else
                    {
                        // second player
                        clientId = 1;
                    }
                    return true;
                }
            return false;
        }
    }
}