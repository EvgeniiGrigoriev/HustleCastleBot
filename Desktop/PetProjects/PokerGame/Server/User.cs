using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class User
    {
        public Socket _socket { get; set; }

        public int Id { get; set; }

        public User(Socket socket, int id)
        {
            this._socket = socket;
            this.Id = id;
        }
    }
}
