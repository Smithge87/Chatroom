﻿using System;
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
        }
    }
}
      

