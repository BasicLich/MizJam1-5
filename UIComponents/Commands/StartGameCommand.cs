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
            game.GameState = MizJam1Game.GameStates.Playing;
        }
    }
}
