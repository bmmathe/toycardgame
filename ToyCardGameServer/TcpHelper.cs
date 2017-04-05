using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToyCardGame.Redis;
using ToyCardGameLibrary;

namespace ToyCardGameServer
{
    class TcpHelper
    {
        private static Dictionary<string, TcpClient> _clients;
        private static TcpListener listener { get; set; }
        private static bool accept { get; set; } = false;

        public static void StartServer(int port)
        {
            IPAddress address = IPAddress.Any;
            listener = new TcpListener(address, port);

            listener.Start();
            accept = true;
            _clients = new Dictionary<string, TcpClient>();
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
            var clientAlive = true;
            Player player = null;
            while (true)
            {
                if (player == null)
                {
                    byte[] data = Encoding.ASCII.GetBytes("What is your username?");
                    client.GetStream().Write(data, 0, data.Length);

                    byte[] buffer = new byte[1024];
                    var bytes = client.GetStream().Read(buffer, 0, buffer.Length);
                    string message = Encoding.ASCII.GetString(buffer, 0, bytes);
                    player = PlayerRedis.Get(message);

                    if (player == null)
                    {
                        data = Encoding.ASCII.GetBytes("terminate: Username does not exist.");
                        client.GetStream().Write(data, 0, data.Length);
                        break;
                    }
                    else
                    {
                        _clients.Add(player.Username, client);
                    }
                }
                                                
                byte[] responseBuffer = new byte[1024];
                var responseBytes = client.GetStream().Read(responseBuffer, 0, responseBuffer.Length);
                string response = Encoding.ASCII.GetString(responseBuffer, 0, responseBytes);
                switch (response)
                {
                    case "getdecks":
                        string decks = String.Empty;                        
                        foreach (var deck in player.Decks)
                        {
                            decks += deck.Name + "\n";
                        }
                        byte[] data = Encoding.ASCII.GetBytes(decks);
                        client.GetStream().Write(data, 0, data.Length);
                        break;
                    case "startgame":
                        byte[] gamedata = Encoding.ASCII.GetBytes("game started");
                        client.GetStream().Write(gamedata, 0, gamedata.Length);
                        foreach (var globalClients in _clients)
                        {
                            globalClients.Value.GetStream().Write(gamedata, 0, gamedata.Length);
                        }
                        break;
                }

            }
            Console.WriteLine("Closing connection.");
            client.GetStream().Dispose();
        }
    }
}
