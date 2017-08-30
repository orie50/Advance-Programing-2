using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Server.Commands;

namespace Server.Controllers
{
    /// <summary>
    ///     class of base controller
    /// </summary>
    /// <seealso cref="IController" />
    internal class Controller : IController
    {
        /// <summary>
        ///     dicitonary of name and command class
        /// </summary>
        protected Dictionary<string, ICommand> Commands;

        /// <summary>
        ///     constructor of the <see cref="Controller" /> class.
        /// </summary>
        protected Controller()
        {
            Commands = new Dictionary<string, ICommand>();
        }

        /// <summary>
        ///     exectue command according the arguments.
        /// </summary>
        /// <param name="commandLine"></param>
        /// <param name="client">The client that send the command.</param>
        /// <returns>
        ///     the answer of the execute
        /// </returns>
        public virtual string ExecuteCommand(string commandLine, TcpClient client = null)
        {
            // get the command name
            string[] arr = commandLine.Split(' ');
            string commandKey = arr[0];
            if (!Commands.ContainsKey(commandKey))
                return "Command not found\n";
            // get the args for the command
            string[] args = arr.Skip(1).ToArray();
            ICommand command = Commands[commandKey];
            // execute the command
            return command.Execute(args, client);
        }
    }
}