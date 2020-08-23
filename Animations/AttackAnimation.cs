using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MizJam1.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.Animations
{
    class AttackAnimation : IAnimation
    {
        private ushort amount;

        public AttackAnimation(ushort amount)
        {
            this.amount = amount;
        }

        private float time = 0.25f;
        private float currTime = 0f;

        private bool played = false;

        public bool ScreenSpace => true;

        public bool DrawBeforeStart => false;

        public bool Done => currTime >= time;

        public void Draw(SpriteBatch spriteBatch)
        {
            //
        }

        public void Update(GameTime gameTime)
        {
            if (!played)
            {
                if (amount > 0)
                {
                    AudioManager.Instance.PlaySoundEffect("Hit", Vector2.Zero);
                }
                else
                {
                    AudioManager.Instance.PlaySoundEffect("Miss", Vector2.Zero);
                }
                played = true;
            }
            currTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
