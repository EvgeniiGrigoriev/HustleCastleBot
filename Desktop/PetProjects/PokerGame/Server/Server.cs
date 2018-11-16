using DistributedLogic.NetworkLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Server : AbstractNetwork
    {
        private List<User> _users { get; set; }

        private int LastUserId { get; set; } = 0;
        
        private TcpListener _tcpListener { get; set; }

        public Server()
        {
            _users = new List<User>();
        }

        public void Start()
        {
            _tcpListener = new TcpListener(IPAddress, Port);

            _tcpListener.Start();

            _tcpListener.BeginAcceptSocket(AcceptSocket, null);
        }

        public void Finish()
        {
            _tcpListener.Stop();
        }

        public override void SendMessage(Socket socket, string message)
        {
            var messageAsBytes = Encoding.ASCII.GetBytes(message);

            socket.Send(messageAsBytes, messageAsBytes.Length, SocketFlags.None);
        }

        public override void ReceiveMessage(object state)
        {
            var user = (User)state;

            try
            {
                byte[] buffer = new byte[1024];

                var messageLength = user._socket.EndReceive(buffer, buffer.Length, SocketFlags.None);

                var message = Encoding.ASCII.GetString(buffer, 0, messageLength);

                Console.WriteLine(message);

                user._socket.Re


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Error. User id = " + user.Id);
            }
        }

        private void AcceptSocket(object state)
        {
            try
            {
                Socket clientSocket = _tcpListener.EndAcceptSocket(null);

                User user = new User(clientSocket, LastUserId ++);

                _users.Add(user);

                byte[] buffer = new byte[1024];

                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveMessage, user);

                SendMessage(user._socket, "Hello from Server");

                Console.WriteLine("User connected");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            _tcpListener.BeginAcceptSocket(AcceptSocket, null);
        }
    }
}
