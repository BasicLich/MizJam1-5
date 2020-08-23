using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace MizJam1.Animations
{
    public class ShowBannerAnimation : IAnimation
    {
        private readonly float time;
        private float currTime;
        private readonly Texture2D banner;
        private Rectangle destination;

        public ShowBannerAnimation(Texture2D banner, Rectangle destination, float time)
        {
            this.banner = banner;
            this.destination = destination;
            this.time = time;
        }

        public bool DrawBeforeStart => false;

        public bool Done => currTime >= time;

        public bool ScreenSpace => true;

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(banner, destination, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            currTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
