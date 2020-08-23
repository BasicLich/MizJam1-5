using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.Animations
{
    class DefendAnimation : IAnimation
    {
        private Texture2D texture;
        private Point position;
        private Rectangle sourceRectangle;

        const float time = 1f;
        private float currTime;

        public DefendAnimation(Texture2D texture, Point position, Rectangle sourceRectangle)
        {
            this.texture = texture;
            this.position = position;
            this.sourceRectangle = sourceRectangle;

            currTime = 0;
        }
        public bool ScreenSpace => false;

        public bool DrawBeforeStart => false;

        public bool Done => currTime >= time;

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(position, Global.SpriteSize), sourceRectangle, Color.White); //128
        }

        public void Update(GameTime gameTime)
        {
            currTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
