using MizJam1;
using System;
namespace MizJam1.UIComponents.Commands
{
    public class OpenOptionsCommand : ICommand
    {
        private MizJam1Game game;
        public OpenOptionsCommand(MizJam1Game game)
        {
            this.game = game;
        }

        /// <summary>
        /// Jumps to the options menu
        /// </summary>
        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
