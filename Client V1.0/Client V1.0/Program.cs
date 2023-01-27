using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Microsoft.Win32;



namespace Client_V1._0
{
    internal class Program
    {
        static Socket dinleyiciSoket = new Socket
             (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        const int PORT = 52000;
        static void Main(string[] args)
        {
            DeviceInfo deviceInfo = new DeviceInfo();

            deviceInfo.TCPListener();



        }
    }
}
