using MizJam1;
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
            game.Exit();
        }
    }
}
