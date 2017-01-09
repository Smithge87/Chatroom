using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatroomServer
{
    public class Server
    {
        ServerData serverData = new ServerData();
        TcpListener serverSocket;
        TcpClient client;
        byte[] bytes = new byte[256];
        string chat = null;
        Stream clientInput;
        string nameTag = "!@#$%";
        int queueCount = 0;



        public void StartServer()
        {
            serverSocket = CreateServerSocket();
            StartServerSocket(serverSocket);
            ListenForClients();
            RunChatroom();
        }
        public TcpListener CreateServerSocket()
        {
            IPAddress serverIp = IPAddress.Parse("192.168.0.146");
            TcpListener serverSocket = new TcpListener(serverIp, 1234);
            return serverSocket;
        }
        public void StartServerSocket(TcpListener serverSocket)
        {
            serverSocket.Start();
            Console.WriteLine("\n\nThe server's Ip Address and port are :" + serverSocket.LocalEndpoint);
        }
        public void ListenForClients()
        {
            TcpClient client = serverSocket.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            Console.WriteLine("New Connection accepted. Awaiting client ID...");
            serverData.serverDictionary.Add(client, stream);
        }
        public string ConvertStreamToChat()
        {
            if (clientInput != null)
            {
                int j = 0;
                byte[] bytes = new byte[500];
                clientInput.Read(bytes, 0, bytes.Length);
                for (int i = 0; i < 500; i++)
                    if (bytes[i] != 0)
                    {
                        j++;
                    }
                byte[] cleaned = new byte[j];
                for (int i = 0; i < j; i++)
                {
                    cleaned[i] = bytes[i];
                }
                chat = Encoding.ASCII.GetString(cleaned);
            }
            return chat;
        }
        public void QueueChat(string chats)
        {
            if (chats != null && chats.Contains(nameTag))
            {
                string clientName = chats.Remove(0,5);
                Console.WriteLine("Recieved client ID:"+ clientName);
                serverData.serverDictionary.Remove(client);
                serverData.clientNames.Add(clientName);
                chat = clientName + " entered the chat";
                serverData.chatQueue.Enqueue(chat);
                queueCount++;
            }
            else if (chats != null)
            {
                Console.WriteLine("Received: {0}", chat);
                serverData.chatQueue.Enqueue(chat);
                queueCount++;           
            }
        }
        public void SendChat()
        {
            while (queueCount > 0)
            {
                string queue = serverData.chatQueue.Peek();
                Console.WriteLine("Sent: {0}", queue);
                queueCount--;
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(chat);
                clientInput.Write(ba, 0, ba.Length);
            }
            chat = null;
        }
        public void RunChat()
        {
            while (true)
            {
                chat = ConvertStreamToChat();
                QueueChat(chat);
                SendChat();
            }
        }
        public void ListenToClients()
        {
            foreach (TcpClient currentTalker in serverData.serverDictionary.Keys)
            {
                if (currentTalker.GetStream() != null)
                {
                    clientInput = currentTalker.GetStream();
                    client = currentTalker;
                }
            }
        }
        public void RunChatroom()
        {
            Task.Run(() => ListenForClients());
            Task.Run(() => ListenToClients());
            RunChat();
        }
    }
}
