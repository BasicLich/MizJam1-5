using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MizJam1.Audio;
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
        const float time = 1.5f;
        float changeDiceTime;
        int currDiceValue;
        int lastDiceValue;

        bool playedShakeSound = false;
        bool playedThrowSound = false;

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

        public bool ScreenSpace => false;

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
            if (currTime < time *0.75)
            {
                if (!playedShakeSound)
                {
                    AudioManager.Instance.PlaySoundEffect("DiceShake", Vector2.Zero);
                    playedShakeSound = true;
                }
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
                if (!playedThrowSound)
                {
                    AudioManager.Instance.PlaySoundEffect("DiceThrow", Vector2.Zero);
                    playedThrowSound = true;
                }
                currDiceValue = result - 1;
            }
        }
    }
}
