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
        TcpClient senderId;
        byte[] bytes = new byte[256];
        string chat = "";
        NetworkStream clientInput;
        string nameTag = "!@#$%";
        private Object sendLock = new Object();


        public void StartServer()
        {
            serverSocket = CreateServerSocket();
            StartServerSocket(serverSocket);
            ListenForClients();
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
            while (true)
            {
                TcpClient client = serverSocket.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                Console.WriteLine("New Connection accepted. Awaiting client ID...");
                serverData.clientSockets.Add(client, stream);
                Task.Run(() => ListenToClient(client, stream));
            }
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
            clientInput = null;
            return chat;
        }
        public void QueueChat(string chats)
        {
            if (chats != "" && chats.Contains(nameTag))
            {
                string clientName = chats.Remove(0, 5);
                Console.WriteLine("Recieved client ID:" + clientName);
                serverData.clientIds.Add(senderId ,clientName);
                chat = clientName + " entered the chat";
                serverData.chatQueue.Enqueue(chat);
            }
            else if (chats != "")
            {
                Console.WriteLine("Received: {0}", chat);
                serverData.chatQueue.Enqueue(chat);
                serverData.chatHistory.Enqueue(chat);
            }
        }
        public void SendChat()
        {
                while (serverData.chatQueue.Count > 0)
                {
                    string queue = serverData.chatQueue.Dequeue();
                foreach (var clients in serverData.clientSockets)
                {
                    if (clients.Key != senderId)
                    {
                        Console.WriteLine("Sent: {0}", queue);
                        ASCIIEncoding asen = new ASCIIEncoding();
                        byte[] ba = asen.GetBytes(queue);
                        clients.Value.Write(ba, 0, ba.Length);
                    }
                    chat = "";
                }
            }
        }
        public void RunChat()
        {
            lock (sendLock)
            {
                chat = ConvertStreamToChat();
                QueueChat(chat);
                SendChat();
            }
        }
        public void ListenToClient(TcpClient client, NetworkStream stream)
        {
            while (true)
            {
                if (stream.DataAvailable == true)
                {
                    clientInput = client.GetStream();
                    senderId = client;
                }
                else if(stream == null)
                {
                    serverData.clientSockets.Remove(client);
                    var user = serverData.clientIds[client];
                    Console.WriteLine("{0} has left the chatroom", user);
                }
                RunChat();
            }
        }
    }
}
