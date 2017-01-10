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
        ASCIIEncoding encoder = new ASCIIEncoding();
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
            byte[] outgoingMessage = encoder.GetBytes("!@#$%" + clientName);
            stream.Write(outgoingMessage, 0, outgoingMessage.Length);
            return clientName;
        }
        public void SendChat()
        {
            Console.Write(textPrompt);
            string message = textPrompt + Console.ReadLine();
            byte[] outgoingMessage = encoder.GetBytes(message);
            stream.Write(outgoingMessage, 0, outgoingMessage.Length);
        }
        public void GetChat()
        {
            while (true)
            {
                byte[] incomingMessage = new byte[100];
                int messageLength = stream.Read(incomingMessage, 0, 100);
                CorrectCursorBeforeText();
                if (messageLength.Equals(0))
                {
                    Environment.Exit(0);
                }
                else
                {
                    for (int i = 0; i < messageLength; i++)
                    {
                        Console.Write(Convert.ToChar(incomingMessage[i]));
                    }
                }
                CorrectCursorAfterText();
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
        public static void CorrectCursorBeforeText()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            int currentLine = Console.CursorTop;
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLine);
        }
        public void CorrectCursorAfterText()
        {
            Console.Write("\n");
            Console.WriteLine(textPrompt);
            Console.SetCursorPosition(textPrompt.Length, Console.CursorTop - 1);
        }

    }
}
