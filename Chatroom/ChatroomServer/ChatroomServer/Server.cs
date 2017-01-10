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
        ASCIIEncoding encoder = new ASCIIEncoding();
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
                string clientName = chats.Remove(0, 5);
                Console.WriteLine("Recieved client ID:" + clientName);
                serverData.clientIds.Add(senderId ,clientName);
                chat = clientName + " entered the chat";
                serverData.chatQueue.Enqueue(chat);
            }
            else if (chats != "" && chats.Remove(0,(serverData.clientIds[senderId]).Length+3) == "exit")
            {
                chat = serverData.clientIds[senderId] + " has left the chatroom";
                senderId.Close();
                serverData.clientSockets[senderId].Close();
                serverData.chatQueue.Enqueue(chat);
                serverData.chatLog.Enqueue(chat);
            }
            else if (chats != "")
            {
                Console.WriteLine("Received: {0}", chat);
                serverData.chatQueue.Enqueue(chat);
                serverData.chatLog.Enqueue(chat);
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
