using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hockey.Entities
{
    class PlayerStriker
    {
        public PlayerStriker(float startX, float startY, float speed, float strikerSize, GraphicsDeviceManager graphics, Dictionary<string, Keys> bindings)
        {
            this.speed = speed;
            oldSpeed = speed;
            startPosition = new Vector2(startX, startY);
            position = new Vector2(startX, startY);
            previousPosition = new Vector2(startX, startY);
            radius = strikerSize / 2;
            this.graphics = graphics;
            left = bindings["left"];
            right = bindings["right"];
            down = bindings["down"];
            up = bindings["up"];
            inBounds = true;
        }
        public Vector2 position;
        public Vector2 startPosition;
        public Vector2 previousPosition;
        public Vector2 velocity;
        public float radius;
        public float oldSpeed;
        public bool inBounds;
        public float maxSpeed = 160f;

        private readonly Keys left;
        private readonly Keys right;
        private readonly Keys up;
        private readonly Keys down;
        public float speed { get; set; }
        public GraphicsDeviceManager graphics;

        public void HandlePlayer(KeyboardState kstate, GameTime gameTime, Puck puck)
        {
            previousPosition = position;
            if (kstate.IsKeyDown(up) || kstate.IsKeyDown(left) || kstate.IsKeyDown(down) || kstate.IsKeyDown(right))
            {
                if (speed * 1.4f < maxSpeed)
                {
                    speed *= 1.4f;
                }
                if (kstate.IsKeyDown(up))
                {
                    if (position.Y - radius > 0)
                    {
                        position.Y += -1 * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    } else
                    {
                        position.Y = 0 + radius;
                        velocity.Y = -1 * velocity.Y;
                        position.Y += 1 * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }
                if (kstate.IsKeyDown(left) && position.X - radius > 0)
                {
                    position.X += -1 * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (kstate.IsKeyDown(down) && position.Y + radius < graphics.PreferredBackBufferHeight)
                {
                    position.Y += 1 * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (kstate.IsKeyDown(right) && position.X + radius < graphics.PreferredBackBufferWidth)
                {
                    position.X += 1 * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

            }
            else
            {
                speed = oldSpeed;
            }
            if (position.Y - radius > 0 && position.X - radius > 0 && position.Y + radius < graphics.PreferredBackBufferHeight && position.X + radius < graphics.PreferredBackBufferWidth)
            {
                velocity.X = (velocity.X + position.X - previousPosition.X) * 0.988f;
                velocity.Y = (velocity.Y + position.Y - previousPosition.Y) * 0.988f;
                position.X += velocity.X / 20;
                position.Y += velocity.Y / 20;
            }
            puck.HandleCollision(this, gameTime);
        }
    }
}
