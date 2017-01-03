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
        //TcpClient server;
        TcpClient client;
        string clientName;
        public void StartClient()
        {
            client = CreateConnection();
            SetId(client);
            RunChat();
        }
        public TcpClient CreateConnection()
        {
            TcpClient client = new TcpClient();
            Console.WriteLine("\n\nConnecting...");
            client.Connect("10.134.134.115", 1234);
            //IPAddress clientIp = IPAddress.Parse("10.134.134.115");
            //TcpListener clientSocket = new TcpListener(clientIp, 1234);
            //clientSocket.Start();
            //server = clientSocket.AcceptTcpClient();
            //Console.WriteLine("\nConnected to server!");

            return client;
        }
        public void SetId(TcpClient clientSocket)
        {
            Console.WriteLine("\nPlease enter your name:\n");
            clientName = Console.ReadLine();
            Stream stm = clientSocket.GetStream();

            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes("!@#$%" + clientName);
            stm.Write(ba, 0, ba.Length);
        }
        public void SendChat()
        {
            Console.Write(clientName + ":> ");
            //String str = (clientName + ":>" + Console.ReadLine());
            Stream stm = client.GetStream();

        }
        public void GetChat()
        {
            Stream stm = client.GetStream();
            byte[] bb = new byte[100];
            int k = stm.Read(bb, 0, 100);

            for (int i = 0; i < k; i++)
            
                Console.Write(Convert.ToChar(bb[i]));
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
