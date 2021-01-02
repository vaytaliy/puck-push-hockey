using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hockey.Menu
{
    class MenuButton
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;

        Color color = new Color(255, 255, 255, 255);

        public Vector2 size;
        public MenuButton(Texture2D texture, GraphicsDevice graphics)
        {
            this.texture = texture;
            size = new Vector2(graphics.Viewport.Width / 8, graphics.Viewport.Height / 30);
        }

        public bool isClicked;
        public void Update(MouseState mouse)
        {
            rectangle = new Rectangle((int)position.X-50, (int)position.Y, (int)size.X*2, (int)size.Y*2);

            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(rectangle))
            {
                color.B = 100;
                if (mouse.LeftButton == ButtonState.Pressed) isClicked = true;
            }
            else
            {
                color.B = 255;
                isClicked = false;
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

    }
}
