using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Listener
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            // ŞU ANLIK KULLANIM DIŞIDIR.
            
            MainCommands mainCommands = new MainCommands();

            mainCommands.TCPListener();
            
        }
    }
}
