using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Server.Commands;
using Server.Models;

namespace Server.Controllers
{
    /// <summary>
    ///     game controller with the relevant ICommands
    /// </summary>
    /// <seealso cref="Controller" />
    internal class GameController : Controller
    {
        private IModel _model;
        private string _name;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameController" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="model">The model.</param>
        public GameController(IModel model, string name)
        {
            _name = name;
            _model = model;
            Commands = new Dictionary<string, ICommand>
            {
                { "play", new Play(_model) },
                { "close", new Close(_model) },
                { "getState", new GetState(_model) }
            };
        }

        public override string ExecuteCommand(string commandLine, TcpClient client = null)
        {
            commandLine += " " + _name;
            return base.ExecuteCommand(commandLine, client);
        }
    }
}