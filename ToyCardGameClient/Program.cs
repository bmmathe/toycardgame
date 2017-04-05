using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ToyCardGameClient
{
    class Program
    {
        private static TcpClient _client;

        static void Main(string[] args)
        {
            //using (_client = new TcpClient())
            //{
            //    var host = "localhost";
            //    //var host = "toycardgameserver";
            //    _client.ConnectAsync(host, 5678).Wait();
            //    Task.Run(() =>
            //    {
            //        var isAlive = true;
            //        using (NetworkStream stream = _client.GetStream())
            //        {
            //            while (isAlive)
            //            {  
            //                var data = new Byte[1024];
            //                String responseData = String.Empty;
                      
            //                Int32 bytes = stream.Read(data, 0, data.Length);
            //                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            //                Console.WriteLine("Received: {0}", responseData);
            //                if (responseData.StartsWith("terminate"))
            //                {
            //                    isAlive = false;
            //                    break;
            //                }
            //            }
            //        }
                    
            //    });
                
            //}

            //var message = Console.ReadLine();
            //while (message != "quit")
            //{
            //    var response = System.Text.Encoding.ASCII.GetBytes(message);
            //    _client.GetStream().Write(response, 0, response.Length);
            //}

            //_client.GetStream().Dispose();
            //_client.Dispose();

            //Console.WriteLine("Client disconnected");
            //Console.Read();

            var client = new Client();
            client.DataReceived += Client_DataReceived;
            string message = Console.ReadLine();            
            while (message != "quit")
            {
                var data = System.Text.Encoding.ASCII.GetBytes(message);
                client.SendData(data);
                message = Console.ReadLine();
            }
        }

        private static void Client_DataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}