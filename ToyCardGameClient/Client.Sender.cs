using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ToyCardGameClient
{
    public sealed partial class Client
    {

        private sealed class Sender
        {
            private readonly ConcurrentQueue<byte[]> _queue = new ConcurrentQueue<byte[]>();
            private readonly AutoResetEvent _signal = new AutoResetEvent(true);

            internal void SendData(byte[] data)
            {
                // transition the data to the thread and send it...
                _queue.Enqueue(data);
                _signal.Set();
            }

            internal Sender(NetworkStream stream)
            {
                _stream = stream;
                _thread = new Thread(Run);
                _thread.Start();
            }

            private void Run()
            {
                while (true)
                {
                    _signal.WaitOne();

                    byte[] item = null;
                    while (_queue.TryDequeue(out item))
                    {
                        _stream.Write(item, 0, item.Length);
                    }
                }
            }

            private NetworkStream _stream;
            private Thread _thread;
        }
    }
}
