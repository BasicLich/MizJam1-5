using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MizJam1.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MizJam1.Animations
{
    public class AnimationQueue
    {
        private readonly Queue<(IAnimation, Unit)> animations;
        private (IAnimation, Unit)? current;

        public AnimationQueue()
        {
            animations = new Queue<(IAnimation, Unit)>();
        }

        public bool Done => current == null;

        public void Add(IAnimation animation, Unit unit)
        {
            if (unit != null)
            {
                unit.Hide = true;
            }

            animations.Enqueue((animation, unit));
            if (current == null)
            {
                current = animations.Dequeue();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (current != null)
            {
                if (current.Value.Item1.Done)
                {
                    if (current.Value.Item2 != null)
                    {
                        current.Value.Item2.Hide = false;
                    }
                    current = null;
                    if (animations.Any())
                    {
                        current = animations.Dequeue();
                    }
                }
                else
                {
                    current.Value.Item1.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteBatch screenSpriteBatch)
        {
            foreach (var animation in animations.ToList())
            {
                if (animation.Item1.DrawBeforeStart)
                {
                    if (animation.Item1.LevelSpace)
                    {
                        animation.Item1.Draw(spriteBatch);
                    }
                    else
                    {
                        animation.Item1.Draw(screenSpriteBatch);
                    }
                }
            }

            if (current != null)
            {
                if (current.Value.Item1.LevelSpace)
                {
                    current?.Item1.Draw(spriteBatch);
                }
                else
                {
                    current?.Item1.Draw(screenSpriteBatch);
                }
            }
        }
    }
}
