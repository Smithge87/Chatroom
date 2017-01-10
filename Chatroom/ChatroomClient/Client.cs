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
            CreateConnection();
            SetId();
            Task.Run(() => SendId(clientName, stream));
            WelcomeToChat();
            RunChat();
        }
        private void CreateConnection()
        {
            client = new TcpClient();
            Console.WriteLine("\n\nConnecting...Done!");
            client.Connect("192.168.0.146", 1234);
            stream = client.GetStream();
            Console.WriteLine("\n\n  WELCOME TO THE CHATROOM ");
            Console.WriteLine(" -------------------------\n");
        }
        private void SetId()
        {
            Console.WriteLine("\n  Please enter your name:\n");
            clientName = Console.ReadLine();
            textPrompt = (clientName + ":> ");
        }
        private void SendId(string clientName, NetworkStream stream)
        { 
            byte[] outgoingMessage = encoder.GetBytes("!@#$%" + clientName);
            stream.Write(outgoingMessage, 0, outgoingMessage.Length);
        }
        private void WelcomeToChat()
        {
            Console.WriteLine("\n\n  Hello " + clientName + ". For a list of chatroom commands,");
            Console.WriteLine("  please type 'chatroom: keywords' at any time\n");
            Console.WriteLine("  Press 'enter' to beging chatting");
            Console.ReadLine();
        }
        private void RunChat()
        {
            Task.Run(() => GetChat());
            while (true)
            {
                SendChat();
            }
        }
        private void SendChat()
        {
            Console.Write(textPrompt);
            string message = textPrompt + Console.ReadLine();
            byte[] outgoingMessage = encoder.GetBytes(message);
            stream.Write(outgoingMessage, 0, outgoingMessage.Length);
        }
        private void GetChat()
        {
            while (true)
            {
                int j = 0;
                byte[] messageBuffer = new byte[500];
                stream.Read(messageBuffer, 0, messageBuffer.Length);
                    for (int i = 0; i < 500; i++)
                        if (messageBuffer[i] != 0)
                        {
                            j++;
                        }
                    byte[] cleaned = new byte[j];
                    for (int i = 0; i < j; i++)
                    {
                        cleaned[i] = messageBuffer[i];
                    }
                    string message = Encoding.ASCII.GetString(cleaned);
                if (message.Length == 0)
                {
                    Environment.Exit(0);
                }
                //else if (message.Contains("####"))
                //{
                //    string cleanedMessage = message.Remove(0, 4);
                //    Console.WriteLine(cleanedMessage);
                //    SetId();
                //}
                else if (message.Length >0)
                { 
                CorrectCursorBeforeText();
                    Console.WriteLine(message);
                    CorrectCursorAfterText();
                }
            }

        }
        private static void CorrectCursorBeforeText()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            int currentLine = Console.CursorTop;
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLine);
        }
        private void CorrectCursorAfterText()
        {
            Console.Write("\n");
            Console.WriteLine(textPrompt);
            Console.SetCursorPosition(textPrompt.Length, Console.CursorTop - 1);
        }
    }
}
