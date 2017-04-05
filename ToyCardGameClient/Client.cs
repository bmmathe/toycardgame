using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace ToyCardGameClient
{
    public sealed partial class Client : IDisposable
    {
        // Called by producers to send data over the socket.
        public void SendData(byte[] data)
        {
            _sender.SendData(data);
        }

        // Consumers register to receive data.
        public event EventHandler<DataReceivedEventArgs> DataReceived;

        public Client()
        {
            _client = new TcpClient();
            var host = "localhost";
            //var host = "toycardgameserver";
            _client.ConnectAsync(host, 5678).Wait();
            _stream = _client.GetStream();

            _receiver = new Receiver(_stream);
            _sender = new Sender(_stream);

            _receiver.DataReceived += OnDataReceived;
        }

        private void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            var handler = DataReceived;
            if (handler != null) DataReceived(this, e);  // re-raise event
        }

        private TcpClient _client;
        private NetworkStream _stream;
        private Receiver _receiver;
        private Sender _sender;

        public void Dispose()
        {
            _client.GetStream().Dispose();
            _client.Dispose();
        }
    }

}
