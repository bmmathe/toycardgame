using System;
using System.Collections.Generic;
using System.Text;

namespace ToyCardGameClient
{
    public class DataReceivedEventArgs : EventArgs
    {
        public string Data { get; set; }

        public DataReceivedEventArgs(string data)
        {
            Data = data;
        }
    }
}
