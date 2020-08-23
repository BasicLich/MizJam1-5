using Microsoft.Xna.Framework;
using MizJam1.Audio;
using System;
namespace MizJam1.UIComponents.Commands
{
    public class ResetCommand : ICommand
    {
        private MizJam1Game game;

        public ResetCommand(MizJam1Game game)
        {
            this.game = game;
        }

        /// <summary>
        /// Resets the game. DEBUG
        /// </summary>
        public void Execute()
        {
            AudioManager.Instance.PlaySoundEffect("MenuClick", Vector2.Zero);

            game.Reset();
        }
    }
}
