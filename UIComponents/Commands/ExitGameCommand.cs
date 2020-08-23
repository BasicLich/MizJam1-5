using Microsoft.Xna.Framework;
using MizJam1;
using MizJam1.Audio;
using System;
namespace MizJam1.UIComponents.Commands
{
    public class ExitGameCommand : ICommand
    {
        private MizJam1Game game;

        public ExitGameCommand(MizJam1Game game)
        {
            this.game = game;
        }

        /// <summary>
        /// Exits the game.
        /// </summary>
        public void Execute()
        {
            AudioManager.Instance.PlaySoundEffect("MenuClick", Vector2.Zero);

            game.Exit();
        }
    }
}
