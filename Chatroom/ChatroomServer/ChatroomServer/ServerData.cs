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
        public Dictionary<TcpClient, string> serverDictionary = new Dictionary<TcpClient, string>();
        public Queue<string> conversation = new Queue<string>();
        public Queue<string> chatQueue = new Queue<string>();

        public List<TcpClient> client = new List<TcpClient>();
        public List<string> clientNames = new List<string>();
    }
}
