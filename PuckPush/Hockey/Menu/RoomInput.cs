using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hockey.Menu
{
    class RoomInput
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;
        Color color = new Color(255, 255, 255, 255);
        public Vector2 size;
        public bool isClicked;

        public RoomInput(Texture2D texture, GraphicsDevice graphics, string roomNumber) //for now string, later may be something else
        {
            this.texture = texture;
            size = new Vector2(graphics.Viewport.Width / 8, graphics.Viewport.Height / 30);
            this.roomNumber = roomNumber;
            roomCreatedInputFreeze = true;
            userInput = "";
        }

        public RoomInput(Texture2D texture, GraphicsDevice graphics) //for now string, later may be something else
        {
            this.texture = texture;
            size = new Vector2(graphics.Viewport.Width / 8, graphics.Viewport.Height / 30);
            this.roomNumber = "";
            roomCreatedInputFreeze = true;
            userInput = "";
        }

        public bool roomCreatedInputFreeze { get; set; }
        public string roomNumber { get; set; }
        public string userInput { get; set; }

        public void Update(MouseState mouse)
        {
            rectangle = new Rectangle((int)position.X-50, (int)position.Y, (int)size.X, (int)size.Y);
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(rectangle))
            {
                if (mouse.LeftButton == ButtonState.Pressed) isClicked = true;
            }
            else
            {
                if (mouse.LeftButton == ButtonState.Pressed) isClicked = false;
            }

            if (isClicked)
            {
                color.B = 100;
            } else
            {
                color.B = 255;
            }

        }

        public void setPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, color);
        }

        public Keys HandleInput(KeyboardState kstate)
        {
            var keys = kstate.GetPressedKeys();
           // Console.WriteLine(kstate.GetPressedKeys());
            foreach (Keys key in keys)
            {
                return key;
            }
            return Keys.None;
        }
    }
}
