using System;

namespace ToyCardGameServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Start the server  
            TcpHelper.StartServer(5678);
            TcpHelper.Listen(); // Start listening.  
        }
    }
}