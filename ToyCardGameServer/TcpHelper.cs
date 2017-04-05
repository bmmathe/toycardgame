using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ToyCardGameServer
{
    class TcpHelper
    {
        private static TcpListener listener { get; set; }
        private static bool accept { get; set; } = false;

        public static void StartServer(int port)
        {
            IPAddress address = IPAddress.Any;
            listener = new TcpListener(address, port);

            listener.Start();
            accept = true;

            Console.WriteLine($"Server started. Listening to TCP clients at 127.0.0.1:{port}");
        }

        public static void Listen()
        {
            if (listener != null && accept)
            {

                // Continue listening.  
                while (true)
                {
                    Console.WriteLine("Waiting for client...");
                    var clientTask = listener.AcceptTcpClientAsync(); // Get the client  

                    if (clientTask.Result != null)
                    {                        
                        Task.Run(() => TalkToClient(clientTask.Result));
                    }
                }
            }
        }

        private static void TalkToClient(TcpClient client)
        {
            Console.WriteLine("Client connected. Waiting for data.");           
            string message = "";

            while (message != null && !message.StartsWith("quit"))
            {
                byte[] data = Encoding.ASCII.GetBytes("Send next data: [enter 'quit' to terminate] ");
                client.GetStream().Write(data, 0, data.Length);

                byte[] buffer = new byte[1024];
                var bytes = client.GetStream().Read(buffer, 0, buffer.Length);

                message = Encoding.ASCII.GetString(buffer, 0, bytes);
                switch (message)
                {
                    case "getdecks":
                        byte[] deckdata = Encoding.ASCII.GetBytes("Test");
                        client.GetStream().Write(deckdata, 0, deckdata.Length);
                        break;
                }
                Console.WriteLine(message);
            }
            Console.WriteLine("Closing connection.");
            client.GetStream().Dispose();
        }
    }
}
