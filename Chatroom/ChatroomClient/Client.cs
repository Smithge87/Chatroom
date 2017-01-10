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
        string textPrompt;
        public void StartClient()
        {
            stream = CreateConnection();
            clientName = SetId(client);
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
        public string SetId(TcpClient clientSocket)
        {
            Console.WriteLine("\nPlease enter your name:\n");
            clientName = Console.ReadLine();
            textPrompt = (clientName + ":> ");
            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes("!@#$%" + clientName);
            stream.Write(ba, 0, ba.Length);
            return clientName;
        }
        public void SendChat()
        {
            Console.Write(textPrompt);
            string message = textPrompt + Console.ReadLine();
            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(message);
            stream.Write(ba, 0, ba.Length);
        }
        public void GetChat()
        {
            while (true)
            {
                byte[] bb = new byte[100];
                int k = stream.Read(bb, 0, 100);
                AdjustCursorBeforeText();
                for (int i = 0; i < k; i++)
                {
                    Console.Write(Convert.ToChar(bb[i]));
                }
                AdjustCursorAfterText();
            }
        }
        public void RunChat()
        {
            Task.Run(() => GetChat());
            while (true)
            {
                SendChat();
            }
        }
        public static void AdjustCursorBeforeText()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            int currentLineCursor = Console.CursorTop;
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
        public void AdjustCursorAfterText()
        {
            Console.Write("\n");
            Console.WriteLine(textPrompt);
            Console.SetCursorPosition(textPrompt.Length, Console.CursorTop - 1);
        }

    }
}
