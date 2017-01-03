using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatroomClient
{
     class Program
    {
        public static void Main()
        {
            Client client = new Client();
            client.StartClient();
            //string clientName;
            //    try
            //    {
            //        TcpClient clientSocket = new TcpClient();
            //        Console.WriteLine("\n\nConnecting...");
            //        clientSocket.Connect("192.168.0.132", 1234);
            //        Console.WriteLine("\nConnected!");
            //        Console.WriteLine("\nPlease enter your name:\n");
            //        clientName = Console.ReadLine();

            //    bool connected = true;
            //    while (connected == true)
            //    {
            //        Console.Write(clientName + ":> ");
            //        String str = (clientName +":>"+ Console.ReadLine());
            //        Stream stm = clientSocket.GetStream();

            //        ASCIIEncoding asen = new ASCIIEncoding();
            //        byte[] ba = asen.GetBytes(str);
            //        stm.Write(ba, 0, ba.Length);


            //        byte[] bb = new byte[100];
            //        int k = stm.Read(bb, 0, 100);

            //        for (int i = 0; i < k; i++)
            //            Console.Write(Convert.ToChar(bb[i]));
            //        Console.WriteLine("\n");
                        
            //    }

            //        clientSocket.Close();
            //    }

            //    catch (Exception e)
            //    {
            //        Console.WriteLine("Error..... " + e.StackTrace);
            //    }
            
        }
    }
}
      

