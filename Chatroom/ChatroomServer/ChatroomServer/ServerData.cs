using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatroomServer
{
    class ServerData
    {
        public Dictionary<TcpClient, NetworkStream> clientSockets = new Dictionary<TcpClient,NetworkStream>();
        public Dictionary<TcpClient, string> clientIds = new Dictionary<TcpClient, string>();
        public Queue<string> chatQueue = new Queue<string>();
        public Queue<string> chatLog = new Queue<string>();
    }
}
