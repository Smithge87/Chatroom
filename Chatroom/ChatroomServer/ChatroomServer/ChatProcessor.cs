using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatroomServer
{
    class ChatProcessor
    {
        ServerData serverData;
        ASCIIEncoding encoder;

        public ChatProcessor(ServerData serverData)
        {
            this.serverData = serverData;
            encoder = new ASCIIEncoding();
        }
        public string ProcessName(string chats, TcpClient senderId)
        {
            string clientName = chats.Remove(0, 5);
            //if (serverData.clientIds.Keys.Contains(senderId))
            //{
            //    string oldClientName = serverData.clientIds[senderId];
            //    serverData.clientIds.Remove(senderId);
            //    serverData.clientIds.Add(senderId, clientName);
            //    return (oldClientName + " has changed their name to " +clientName);
            //}
            //else
            {
                Console.WriteLine("Recieved client ID:" + clientName);
                serverData.clientIds.Add(senderId, clientName);
                serverData.clientNames.Add(clientName);
                return (clientName + " entered the chatroom");
            }
        }
        public void ProcessKeywordMenu(TcpClient senderId)
        {
            string message = "\n CHATROOM COMMAND LIST \n 'chatroom: exit' \n 'chatroom: show clients' \n 'chatroom: add duck'\n";
            SendToOneUser(senderId, message);
        }
        public string ProcessExit(TcpClient senderId )
        {
            string chat = serverData.clientIds[senderId] + " has left the chatroom";
            senderId.Close();
            serverData.clientNames.Remove(serverData.clientIds[senderId]);
            serverData.clientSockets[senderId].Close();
            serverData.chatQueue.Enqueue(chat);
            serverData.chatLog.Enqueue(chat);
            return chat;
        }
        public string ProcessChat(string chat)
        {
            Console.WriteLine("Received: {0}", chat);
            serverData.chatQueue.Enqueue(chat);
            serverData.chatLog.Enqueue(chat);
            return chat;
        }
        public void ProcessClientList(TcpClient senderId)
        {
            string response = ("\nThe Current Members of the Chatroom are:");
            string clientList = DisplayClients();
            string message = (response + clientList);
            Console.WriteLine("Sent:" + message);
            SendToOneUser(senderId, message);
        }
        //public void ProcessNameChange(TcpClient senderId)
        //{
        //    string message = "\nPlease enter your new user name: \n";
        //    SendToOneUser(senderId, ("####" + message));
        //}
        public string ProcessDuck()
        {
            string response = ("\n                        __\n                      /` , __\n                     |    ).-'\n                    / .--'\n                   / /\n     ,      _.==''`  |\n   .'(  _.='         |\n  {   ``  _.='       |\n   {    |`     ;    /\n   {    |`     ;    /\n      `=._    .='\n        '-`\\`__\n            `-._{\n");
            return response;
        }
        private void SendToOneUser(TcpClient senderId, string message)
        {
            foreach (var clients in serverData.clientSockets)
            {
                if (clients.Key == senderId)
                {
                    byte[] outgoingMessage = encoder.GetBytes(message);
                    clients.Value.Write(outgoingMessage, 0, outgoingMessage.Length);
                }
            }
        }
        string DisplayClients()
        {
            string clientListed = "\n";
            foreach (string client in serverData.clientNames)
            {
                clientListed = clientListed + "\n"+ client;
            }
            return clientListed + "\n";
        }
    }
}
