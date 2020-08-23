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
        private Point skullPos;
        private Sprite skull;

        const float time = 1f;
        private float currentTime;

        public DeathAnimation(Point position, Sprite sprite, Point skullPos, Sprite skull)
        {
            this.position = position;
            this.sprite = sprite;
            this.skullPos = skullPos;
            this.skull = skull;
            currentTime = 0;
        }
        public bool ScreenSpace => false;

        public bool DrawBeforeStart => true;
        public bool Done => currentTime >= time;

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);

            if (currentTime > 0)
            {
                skull.Draw(spriteBatch, skullPos);
            }
        }

        public void Update(GameTime gameTime)
        {
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
