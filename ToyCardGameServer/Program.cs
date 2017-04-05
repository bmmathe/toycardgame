namespace ToyCardGameServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            TcpHelper.StartServer(5678);
            TcpHelper.Listen(); // Start listening.      
        }
    }
}