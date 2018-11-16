using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DistributedLogic.NetworkLogic
{
    public abstract class AbstractNetwork
    {
        public int Port { get; set; } = 3031;

        public IPAddress IPAddress { get; set; } = IPAddress.Parse("127.0.0.1");

        public AbstractNetwork()
        {

        }

        public abstract void SendMessage(Socket socket, string message);

        public abstract void ReceiveMessage(object state);
    }
}
