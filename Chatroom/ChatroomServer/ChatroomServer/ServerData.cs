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
        public Dictionary<TcpClient, NetworkStream> serverDictionary = new Dictionary<TcpClient,NetworkStream>();
        public Queue<string> chatQueue = new Queue<string>();
        public Queue<string> chatHistory = new Queue<string>();
        public List<string> clientNames = new List<string>();
    }
}
