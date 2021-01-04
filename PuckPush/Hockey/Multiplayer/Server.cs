using Hockey.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Hockey.Multiplayer
{
    // Simple TCP server for P2P connections. This class represents the host. 1 socket is limit for connection 
    
    class Server
    {
        private GameManager gameManager;
        public Socket connectedSocket;
        private string message;
        private string messageData;
        private Thread exitHandler;
        private byte[] data;
        private Socket socket;
        public Server(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public async void Start()
        {
            IPHostEntry host = Dns.GetHostEntry("");
            IPAddress address = host.AddressList[3];
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(address.ToString()), 8005); //[TBD] hardcoded to ipv4, not sure which to use 

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                exitHandler = new Thread(new ThreadStart(Exit));
                exitHandler.Start();

                socket.Bind(endPoint);
                socket.Listen(1);
                Console.WriteLine("Waiting for another player to join..");
                Console.WriteLine("Press enter to exit");

                while (true)
                {
                    connectedSocket = await socket.AcceptAsync();
                    SendSuccessfulConnectMessage();     //right after the client has connected
                   
                    do
                    {
                        data = new byte[256];
                        int bytes = 0;

                        while (true)
                        {

                            HandleConnectedClient();
                            bytes = connectedSocket.Receive(data, data.Length, 0);

                            message = Encoding.Unicode.GetString(data, 0, bytes);
                            
                            var messageType = message.Split('&')[0];
                            messageData = message.Split('&')[1];

                            if (messageType == "client_updates")
                            {
                                HandlePlayerTwoUpdate();
                            }
                            Thread.Sleep(25);
                        }
                        //ShutDown();
                    } while (connectedSocket.Available > 0);
                }
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Exit()
        {
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                Console.WriteLine("To restart the server please restart the application");
                connectedSocket.Shutdown(SocketShutdown.Both);
                connectedSocket.Close();
            }
        }

        public void SendSuccessfulConnectMessage()
        {
            data = new byte[256];
            data = Encoding.Unicode.GetBytes("conn_msg&Connected to the server successfully!");
            Console.WriteLine("Somebody has connected!");
            connectedSocket.Send(data);
        }
   
        public void HandleConnectedClient()  // everything that is being sent to a client
        {
            SendPlayerOneLocation();
        }

        public void HandlePlayerTwoUpdate() //synchronizes player 2 position on serverside
        {
            string[] responseParsed = messageData.Split('-');
            gameManager.playerStrikers[1].position.X = float.Parse(responseParsed[0]);
            gameManager.playerStrikers[1].position.Y = float.Parse(responseParsed[1]);
        }

        public void SendPlayerOneLocation() //send player 1 data: striker, puck, flag of score
        {
            message = "host_updates&" 
                + gameManager.playerStrikers[0].position.X + "-" 
                + gameManager.playerStrikers[0].position.Y + "-"
                + gameManager.puck.position.X + "-" 
                + gameManager.puck.position.Y;

            if (gameManager.inputLock) // flag that equals true during game pause after goal score, cause not necessary to send on every tick 
            {
                message += "-" + gameManager.scores[GameManager.Teams.Left] + "-"
                + gameManager.scores[GameManager.Teams.Right];
            }

            byte[] data = Encoding.Unicode.GetBytes(message);
            connectedSocket.Send(data, data.Length, SocketFlags.None);
        }
    }
}
