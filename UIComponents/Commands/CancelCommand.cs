using MizJam1.Levels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.UIComponents.Commands
{
    public class CancelCommand : ICommand
    {
        private Level level;
        public CancelCommand(Level level)
        {
            this.level = level;
        }

        public void Execute()
        {
            level.Cancel();
        }
    }
}
