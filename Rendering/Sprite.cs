using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.Rendering
{
    public class Sprite
    {
        private Texture2D texture;
        private Point size;
        private Rectangle sourceRectangle;
        private Color color;


        public Sprite(Texture2D texture, Point size, Rectangle sourceRectangle, Color color)
        {
            this.texture = texture;
            this.size = size;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
        }

        public void Draw(SpriteBatch spriteBatch, Point destination)
        {
            spriteBatch.Draw(texture, new Rectangle(destination, size), sourceRectangle, color);
        }
    }
}
