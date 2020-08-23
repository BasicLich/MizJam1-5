using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.UIComponents.Commands
{
    public class RestartLevelCommand : ICommand
    {
        private MizJam1Game game;

        public RestartLevelCommand(MizJam1Game game)
        {
            this.game = game;
        }

        public void Execute()
        {
            game.RestartLevel();
        }
    }
}
