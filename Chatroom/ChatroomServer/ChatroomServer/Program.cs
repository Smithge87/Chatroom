using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ChatroomServer;

namespace Chatroom
    {
    public class Server
    {
        public static void Main()
        {
            ChatroomServer.Server server = new ChatroomServer.Server();
            server.StartServer();
        //    bool connected = true;

        //    try
        //    {
        //        IPAddress serverIp = IPAddress.Parse("192.168.0.106");
        //        TcpListener serverSocket = new TcpListener(serverIp, 1234);
        //        serverSocket.Start();

        //        Console.WriteLine("\n\nThe server's Ip Address and port are :" + serverSocket.LocalEndpoint);
        //        Console.WriteLine("Waiting for a connection...");

        //        byte[] bytes = new byte[256];
        //        string chat = null;

        //        while (connected == true)
        //        {
        //            TcpClient clientSocket = serverSocket.AcceptTcpClient();
        //            Console.WriteLine("Connection accepted from " + clientSocket.ExclusiveAddressUse);

        //            chat = null;
        //            NetworkStream stream = clientSocket.GetStream();
        //            int i;

        //            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
        //            {
        //                chat = Encoding.ASCII.GetString(bytes, 0, i);
        //                Console.WriteLine("Received: {0}", chat);

        //                byte[] msg = Encoding.ASCII.GetBytes(chat);

        //                stream.Write(msg, 0, msg.Length);
        //                Console.WriteLine("Sent: {0}", chat);
        //            }
        //            clientSocket.Close();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Error... " + e.StackTrace);
        //    }
        //    finally
        //    {
        //        //serverSocket.Stop();
        //    }
        //}
        }
    }  
}

            //public static void Main()
            //{
            //    TcpListener server;
            //    {
            //        int port = 12345;
            //        IPAddress localAddr = IPAddress.Parse("192.168.0.106");
            //        server = new TcpListener(localAddr, port);
            //        server.Start();
            //        Byte[] bytes = new Byte[256];
            //        String data = null;

            //        while (true)
            //        {
            //            Console.Write("Waiting for a connection... ");
            //            TcpClient clientSocket = server.AcceptTcpClient();
            //            Console.WriteLine("Connected!");
            //            Console.ReadLine();
            //            data = null;
            //        }


            //    }
           //}
        
    

    //Create a socket.
    //Bind the socket to a local IPEndPoint.
    //Place the socket in listen mode.
    //Accept an incoming connection on the socket.  



    //(20 points): As a user, I want to be able to chat with another person over the local network, so that I can keep in communication with friends and family.
    //(10 points): As a developer, I want to implement the observer design pattern, so that I can send out a notification to all users that a new person has joined the chat room.
    //(10 points): As a developer, I want to implement dependency injection for logging, so that I can log all messages, log when someone joins the chat, and log when someone leaves the chat.
    //(10 points): As a developer, I want to use a dictionary, so that I can store the users in my chat program.
    //(10 points): As a developer, I want to use a queue, so that I can store and process incoming messages.
    //(10 points): As a developer, I want to use C# best practices, SOLID design principles, and good naming conventions on the project. 
    //(10 points (5 points each)): As a developer, I want pinpoint at least two places where I used one of the SOLID design principles and discuss my reasoning, so that I can properly understand good code design.
    //(Bonus 5 points): As a user, I want the ability to send and receive direct messages, so that I can choose a specific user to talk to.
    //(Bonus 5 points): As a developer, I want the ability to create private chat rooms, so that users can join a channel for themselves.
    //(Bonus 5 points): As a developer, I want to implement a Graphical User Interface(GUI), so that my users don’t have to do everything in the console.
    //HINT: Use TCPclient instead of raw socket class. This will make things a little bit easier.
    //HINT: There should be two projects in the same solution: one client-side (Client) and one server-side(Server)

