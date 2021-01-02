using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hockey.Entities
{
    class Puck
    {
        public Vector2 position;
        public int gatesTextureLower;
        public int gatesTextureUpper;
        public Vector2 velocity;
        private int upperBound;
        private GraphicsDeviceManager graphics;
        private readonly float collisionSlowdown = 0.990f;
        private readonly float friction = 0.996f;
        private readonly float radius;
        public GameManager gameManager { get; set; }
        public Puck(float startX, float startY, GraphicsDeviceManager graphics, float textureSize, Rectangle gatesTextureBounds)
        {
            position = new Vector2(startX, startY);
            velocity = new Vector2(0, 0);
            this.graphics = graphics;
            radius = textureSize / 2;
            gatesTextureLower = gatesTextureBounds.Bottom;
            gatesTextureUpper = gatesTextureBounds.Height;
            upperBound = graphics.PreferredBackBufferHeight - ((graphics.PreferredBackBufferHeight - gatesTextureUpper) / 2);
            this.gameManager = gameManager;
        }

        public void ResetPosition()
        {
            position.X = graphics.PreferredBackBufferWidth / 2;
            position.Y = graphics.PreferredBackBufferHeight / 2;
            velocity.X = 0;
            velocity.Y = 0;
        }

        public bool CheckIntersect(PlayerStriker playerStriker)
        {
            var playerX = playerStriker.position.X;
            var playerY = playerStriker.position.Y;

            var deltax = position.X - playerX;
            var deltay = position.Y - playerY;
            var distance = Math.Sqrt(deltax * deltax + deltay * deltay);

            if (distance < radius + playerStriker.radius)
            {
                velocity *= collisionSlowdown;  //slow it down
                return true;
            }
            return false;
        }

        public void CheckBounce()
        {
            if (position.Y - radius < 0 || position.Y + radius > graphics.PreferredBackBufferHeight)
            {
                if (position.Y - radius < 0)
                {
                    position.Y = 0 + radius;
                }
                else if (position.Y + radius > graphics.PreferredBackBufferHeight)
                {
                    position.Y = graphics.PreferredBackBufferHeight - radius;
                }
                velocity.Y *= -1 * collisionSlowdown;

            }
            if (position.X - radius < 0 || position.X + radius > graphics.PreferredBackBufferWidth)
            {
                if (position.Y > (graphics.PreferredBackBufferHeight - gatesTextureUpper) / 2 + radius && position.Y + radius < upperBound)
                {
                    if (position.X - radius < 0 - radius * 2)
                    {
                        //Handle receive point for the right team
                        ResetPosition();
                        gameManager.IncrementPoints(GameManager.Teams.Right);
                    }
                    else if (position.X > graphics.PreferredBackBufferWidth + radius * 2)
                    {
                        //Handle receive point for the left team
                        ResetPosition();
                        gameManager.IncrementPoints(GameManager.Teams.Left);
                    }
                }
                else
                {

                    if (position.X - radius < 0)
                    {
                        position.X = 0 + radius;
                    }
                    else if (position.X + radius > graphics.PreferredBackBufferWidth)
                    {
                        position.X = graphics.PreferredBackBufferWidth - radius;
                    }
                    velocity.X *= -1 * collisionSlowdown;
                }
            }
        }

        public void HandleCollision(List<PlayerStriker> playerStrikers, GameTime gameTime)
        {
            CheckBounce();
            foreach (PlayerStriker playerStriker in playerStrikers)
            {
                if (CheckIntersect(playerStriker))
                {
                    velocity += (position - playerStriker.position) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                velocity *= friction;
                position += velocity;
            }
        }

        public void HandleCollision(PlayerStriker playerStriker, GameTime gameTime)
        {
            CheckBounce();
            if (CheckIntersect(playerStriker))
            {
                velocity += (position - playerStriker.position) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            velocity *= friction;
            position += velocity;
        }
    }
}
