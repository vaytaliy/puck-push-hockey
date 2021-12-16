using Hockey.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hockey.Multiplayer
{
    class Client
    {

        private GameManager gameManager;
        private string address;
        private int port = 8005;
        private string message;
        private Socket socket;
        private Thread exitHandler;
        string messageData;
        public Client(GameManager gameManager)
        {
            address = GetIpv4Address();
            this.gameManager = gameManager;
        }

        public async void Start()
        {
            try
            {
                AddressInput();
                exitHandler = new Thread(new ThreadStart(Exit));
                exitHandler.Start();
                
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                await socket.ConnectAsync(ipEndPoint);
                Console.WriteLine("Press enter at any time to exit");

                while (true)
                {
                    HandleReceivedMessages();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Exit();
            }
        }

        public string GetIpv4Address()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Unable to find IPv4 address");
        }

        public void HandleReceivedMessages()
        {
            byte[] data = new byte[256];
            int bytes = 0;

            do
            {
                bytes = socket.Receive(data, data.Length, 0);
                message = Encoding.Unicode.GetString(data, 0, bytes);

                var messageType = message.Split('&')[0];
                messageData = message.Split('&')[1];

                HandleOutputMessages();

                if (messageType == "conn_msg")
                {
                    Console.WriteLine(messageData);
                }
                else if (messageType == "host_updates") //happens at intervals, so we can do 2 things at once
                {
                    HandleHostUpdates();
                }

            } while (socket.Available > 0);

        }

        public void Exit()
        {
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                Console.WriteLine("To try and reconnect please restart the application");
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }

        public void HandleOutputMessages()
        {
            SendPlayerTwoLocation();
        }

        public void HandleHostUpdates()
        {

            string[] responseParsed = messageData.Split('-');

            if (responseParsed.Length == 4)

            {
                gameManager.playerStrikers[0].position.X = float.Parse(responseParsed[0]);
                gameManager.playerStrikers[0].position.Y = float.Parse(responseParsed[1]);

                gameManager.puck.position.X = float.Parse(responseParsed[2]);
                gameManager.puck.position.Y = float.Parse(responseParsed[3]);
            }
            else if (responseParsed.Length == 6)
            {
                gameManager.scores[GameManager.Teams.Left] = int.Parse(responseParsed[4]);
                gameManager.scores[GameManager.Teams.Right] = int.Parse(responseParsed[5]);
            }

        }

        public async void SendPlayerTwoLocation()
        {
            message = "client_updates&"
            + gameManager.playerStrikers[1].position.X + "-"
            + gameManager.playerStrikers[1].position.Y;

            byte[] data = Encoding.Unicode.GetBytes(message);
            await socket.SendAsync(data, 0);
        }

        public void AddressInput()
        {
            Console.WriteLine("Enter server address");
            string input = Console.ReadLine();
            if (input.Length > 0) address = input;
        }
    }
}
