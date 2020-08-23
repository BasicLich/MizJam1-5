using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.Animations
{
    public class RollDiceAnimation : IAnimation
    {
        private static Random random = new Random();

        Texture2D dice;
        Point position;
        ushort result;
        ushort staticValue;
        bool fullDice;

        float currTime;
        const float time = 2f;
        float changeDiceTime;
        int currDiceValue;
        int lastDiceValue;

        public RollDiceAnimation(Texture2D dice, Point position, ushort result, ushort staticValue, bool fullDice = false)
        {
            this.dice = dice;
            this.position = position;
            this.result = result;
            this.staticValue = staticValue;
            this.fullDice = fullDice;

            currTime = 0f;
            currDiceValue = random.Next(0, 6);
        }

        public bool DrawBeforeStart => false;

        public bool Done => currTime >= time;

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                dice,
                new Rectangle(position, Global.SpriteSize),
                new Rectangle(new Point(currDiceValue * Global.SpriteWidth, fullDice ? Global.SpriteHeight : 0), Global.SpriteSize),
                Color.White);
        }

        public void Update(GameTime gameTime)
        {
            currTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (currTime < time / 2)
            {
                changeDiceTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (changeDiceTime >= 0.15f)
                {
                    while (currDiceValue == lastDiceValue)
                    {
                        currDiceValue = random.Next(0, 6);
                    }
                    lastDiceValue = currDiceValue;

                    changeDiceTime = 0;
                }
            }
            else
            {
                currDiceValue = result - 1;
            }
        }
    }
}
