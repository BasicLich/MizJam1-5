using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MizJam1.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.Animations
{
    public class DeathAnimation : IAnimation
    {
        private Point position;
        private Sprite sprite;

        public DeathAnimation(Point position, Sprite sprite)
        {
            this.position = position;
            this.sprite = sprite;
        }

        public bool DrawBeforeStart => true;
        public bool Done => true;

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }

        public void Update(GameTime gameTime)
        {
            //
        }
    }
}
