using Hockey.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hockey.Multiplayer
{
    class Startup
    {
        private GameManager gameManager;
        public Startup(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }
    
        public void Initialize()
        {
            
            Console.WriteLine("" +
                "1 - to start server" +
                "\n 2 - to join server" +
                "\n 3- to continue offline");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    CreateServer();
                    break;
                case "2":
                    JoinServer();
                    break;
                default:
                    break;
            }
        }

        public void CreateServer()
        {
            Server server = new Server(gameManager);
            server.Start();
        }

        public void JoinServer()
        {
            Client client = new Client(gameManager);
            client.Start();
        }
    }
}
