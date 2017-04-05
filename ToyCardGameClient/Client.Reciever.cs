using System;
using System.Net.Sockets;
using System.Threading;

namespace ToyCardGameClient
{
    public sealed partial class Client
    {
        private sealed class Receiver
        {
            internal event EventHandler<DataReceivedEventArgs> DataReceived;

            internal Receiver(NetworkStream stream)
            {
                _stream = stream;
                _thread = new Thread(Run);
                _thread.Start();
            }

            private void Run()
            {
                while (true)
                {
                    var data = new Byte[1024];
                    String responseData = String.Empty;

                    Int32 bytes = _stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    DataReceived.Invoke(this, new DataReceivedEventArgs(responseData));
                }
            }

            private NetworkStream _stream;
            private Thread _thread;
        }
    }

}
