using Microsoft.Xna.Framework;
using MizJam1.Audio;
using System;
namespace MizJam1.UIComponents.Commands
{
    public class StartGameCommand : ICommand
    {
        private MizJam1Game game;
        public StartGameCommand(MizJam1Game game)
        {
            this.game = game;
        }

        /// <summary>
        /// Starts or resumes the game.
        /// </summary>
        public void Execute()
        {
            AudioManager.Instance.PlaySoundEffect("MenuClick", Vector2.Zero);
            game.GameState = MizJam1Game.GameStates.Playing;
        }
    }
}
