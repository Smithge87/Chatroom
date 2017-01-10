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
        ChatProcessor processor;
        ASCIIEncoding encoder;
        public Server()
        {
            processor = new ChatProcessor(serverData);
            encoder = new ASCIIEncoding();
        }
        TcpListener serverSocket;
        TcpClient senderId;
        NetworkStream clientInput;
        string chat = "";
        string nameTag = "!@#$%";
        private Object sendLock = new Object();
    
        public void StartServer()
        {
            serverSocket = CreateServerSocket();
            StartServerSocket(serverSocket);
            ListenForClients();
        }
        private TcpListener CreateServerSocket()
        {
            IPAddress serverIp = IPAddress.Parse("192.168.0.146");
            TcpListener serverSocket = new TcpListener(serverIp, 1234);
            return serverSocket;
        }
        private void StartServerSocket(TcpListener serverSocket)
        {
            serverSocket.Start();
            Console.WriteLine("\n\nThe server's Ip Address and port are :" + serverSocket.LocalEndpoint);
        }
        private void ListenForClients()
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
        private void ListenToClient(TcpClient client, NetworkStream stream)
        {
            while (serverData.clientSockets[client].CanRead)
            {
                if (stream.DataAvailable == true)
                {
                    clientInput = client.GetStream();
                    senderId = client;
                }
                RunChat();
            }
        }
        private void RunChat()
        {
            lock (sendLock)
            {
                chat = ConvertStreamToString();
                QueueChat(chat);
                SendChat();
            }
        }
        private string ConvertStreamToString()
        {
            if (clientInput != null)
            {
                int j = 0;
                byte[] messageBuffer = new byte[500];
                clientInput.Read(messageBuffer, 0, messageBuffer.Length);
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
                chat = Encoding.ASCII.GetString(cleaned);
            }
            clientInput = null;
            return chat;
        }
        private void QueueChat(string chats)
        {
            if (chats != "" && chats.Contains(nameTag))
            {
                string clientName = processor.ProcessName(chats, senderId);
                chat = clientName;
                serverData.chatQueue.Enqueue(chat);
            }
            else if (chats != "" && chats.Remove(0,(serverData.clientIds[senderId]).Length+3) == "chatroom: exit")
            {
                chat = processor.ProcessExit(senderId);
            }
            else if (chats != "" && chats.Remove(0, (serverData.clientIds[senderId]).Length + 3) == "chatroom: show clients")
            {
                processor.ProcessClientList(senderId);
                chat = "";
            }
            else if (chats != "" && chats.Remove(0, (serverData.clientIds[senderId]).Length + 3) == "chatroom: keywords")
            {
                processor.ProcessKeywordMenu(senderId);
                chat = "";
            }
            else if (chats != "" && chats.Remove(0, (serverData.clientIds[senderId]).Length + 3) == "chatroom: add duck")
            {
               chat =  processor.ProcessDuck();
            }
            //else if (chats != "" && chats.Remove(0, (serverData.clientIds[senderId]).Length + 3) == "chatroom: change user name")
            //{
            //    processor.ProcessNameChange(senderId);
            //    chat = "";
            //}
            else if (chats != "")
            {
                chat = processor.ProcessChat(chats);
            }
        }
        private void SendChat()
        {
                while (serverData.chatQueue.Count > 0)
                {
                    string queue = serverData.chatQueue.Dequeue();
                foreach (var clients in serverData.clientSockets)
                {
                    if (clients.Key != senderId)
                    {
                        Console.WriteLine("Sent: {0}", queue);
                        byte[] outgoingMessage = encoder.GetBytes(queue);
                        clients.Value.Write(outgoingMessage, 0, outgoingMessage.Length);
                    }
                    chat = "";
                }
            }
        }
    }
}
