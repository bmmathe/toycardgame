using System;
using System.Net;
using System.Net.Sockets;

namespace ToyCardGameClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new TcpClient())
            {
                var host = "localhost";
                //var host = "toycardgameserver";
                client.ConnectAsync(host, 5678).Wait();
                string message = string.Empty;
                using (NetworkStream stream = client.GetStream())
                {
                    while (message != "quit")
                    {
                        Console.WriteLine("Type message to send to server");
                        message = Console.ReadLine();
                        Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);


                        // Send the message to the connected TcpServer. 
                        stream.Write(data, 0, data.Length);

                        Console.WriteLine("Sent: {0}", message);

                        // Receive the TcpServer.response.

                        // Buffer to store the response bytes.
                        data = new Byte[1024];

                        // String to store the response ASCII representation.
                        String responseData = String.Empty;

                        // Read the first batch of the TcpServer response bytes.
                        Int32 bytes = stream.Read(data, 0, data.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                        Console.WriteLine("Received: {0}", responseData);

                    }
                }
            }
        }
    }
}