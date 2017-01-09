using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatroomClient
{
    class Client
    {
        TcpClient client;
        NetworkStream stream;
        string clientName;
        public void StartClient()
        {
            stream = CreateConnection();
            SetId(client);
            RunChat();
        }
        public NetworkStream CreateConnection()
        {
            client = new TcpClient();
            Console.WriteLine("\n\nConnecting...");
            client.Connect("192.168.0.146", 1234);
            NetworkStream stream = client.GetStream();
            return stream;
        }
        public void SetId(TcpClient clientSocket)
        {
            Console.WriteLine("\nPlease enter your name:\n");
            clientName = Console.ReadLine();

            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes("!@#$%" + clientName);
            stream.Write(ba, 0, ba.Length);
        }
        public void SendChat()
        {
            {
                Console.Write(clientName + ":> ");
                string message = clientName + ":> " + Console.ReadLine();
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(message);
                stream.Write(ba, 0, ba.Length);
            }
        }
        public void GetChat()
        {
                byte[] bb = new byte[100];
                int k = stream.Read(bb, 0, 100);
            for (int i = 0; i < k; i++)
            {
                Console.Write(Convert.ToChar(bb[i]));
            }
            Console.Write("\n");
        }
        public void RunChat()
        {
            while (true)
            {
                GetChat();
                SendChat();
            }
        }
    }
}
