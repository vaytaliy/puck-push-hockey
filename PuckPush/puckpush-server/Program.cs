using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PuckServer
{
    class Program
    {
        static int port = 8005; 
        static void Main(string[] args)
        {
            string hostName = Dns.GetHostName();
            IPHostEntry host = Dns.GetHostEntry(hostName);

            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(host.AddressList[3].ToString()), port);
            Console.WriteLine(host.AddressList[3]);

            //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //socket.Bind(localEndPoint);

            //socket.Listen(10);
            //Console.WriteLine("waiting conn");
        }
    }
}

