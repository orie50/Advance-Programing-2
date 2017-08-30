using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Server.Models;

namespace Server.Commands
{
    class GetState : ICommand
    {
        private IModel _model;

        public GetState(IModel model)
        {
            _model = model;
        }

        public string Execute(string[] args, TcpClient client = null)
        {
            //args[0] == game name
            return _model.GetState(args[0], client);
        }
    }
}
