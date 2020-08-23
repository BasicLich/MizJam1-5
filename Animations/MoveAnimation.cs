using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MizJam1.Rendering;
using MizJam1.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.Animations
{
    public class MoveAnimation : IAnimation
    {
        private Point startPoint;
        private Point displacement;
        private readonly float time;
        private float currTime;
        private Sprite sprite;

        public MoveAnimation(Point startPoint, Point endPoint, Sprite sprite, float time = 1)
        {
            this.startPoint = startPoint;
            this.displacement = endPoint - startPoint;
            this.time = time;
            this.sprite = sprite;
            currTime = 0;
        }

        public bool DrawBeforeStart => true;

        public bool Done => currTime >= time;

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, startPoint + (displacement.ToVector2() * (currTime / time)).ToPoint());
        }

        public void Update(GameTime gameTime)
        {
            currTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
