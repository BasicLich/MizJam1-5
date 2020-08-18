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
            game.Reset();
        }
    }
}
