using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Hockey.Entities
{
    class GameManager
    {
        private Puck puck;
        private List<PlayerStriker> playerStrikers;
        public Dictionary<Teams, int> scores { get; set; }
        private GraphicsDeviceManager graphics;
        public bool inputLock = false;
        private int maxScore = 7;
        public GameState currentState { get; set; }

        public enum Teams
        {
            Left,
            Right
        }

        public enum GameState
        {
            MainMenu,
            Options,
            Playing
        }

        public GameManager(Puck puck, List<PlayerStriker> playerStrikers, GraphicsDeviceManager graphics)
        {
            this.puck = puck;
            this.playerStrikers = playerStrikers;
            this.graphics = graphics;
            currentState = GameState.MainMenu;
            scores = new Dictionary<Teams, int>() {
                {   Teams.Left,
                    0
                },
                {   Teams.Right,
                    0
                }
            };
        }

        //accepts "L" or "R";
        public void IncrementPoints(Teams scoredTeam)
        {
            inputLock = true;
            if (scores[scoredTeam] < maxScore)
            {
                scores[scoredTeam] += 1;
                Console.WriteLine($"Team {scoredTeam} scores!" +
                    $"\n=== Current score ===" +
                    $"\n{scores[Teams.Left]} : {scores[Teams.Right]}");
                if (scores[scoredTeam] == maxScore)
                {
                    Console.WriteLine($"The winner is {scoredTeam}");
                    ResetGame();
                    ReturnToMenu();
                }
                ResetStrikers();
            }
        }

        public void ResetStrikers()
        {
            foreach (PlayerStriker playerStriker in playerStrikers)
            {
                playerStriker.position.X = playerStriker.startPosition.X;
                var randomPos = new Random();
                var randomStart = randomPos.Next(0 + (int)playerStriker.radius, graphics.PreferredBackBufferHeight - (int)playerStriker.radius);
                
                playerStriker.position.Y = randomStart;

                playerStriker.velocity.X = 0;
                playerStriker.velocity.Y = 0;
            }
            Thread.Sleep(1000);
            inputLock = false;
        }

        public void ResetGame()
        {
            ResetStrikers();
            scores = new Dictionary<Teams, int>() {
                {   Teams.Left,
                    0
                },
                {   Teams.Right,
                    0
                }
            };
        }

        public void ReturnToMenu()
        {
            ResetGame();
            currentState = GameState.MainMenu;
        }
    }
}
