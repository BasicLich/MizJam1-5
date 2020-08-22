using Microsoft.Xna.Framework;
using MizJam1.Levels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.UIComponents.Commands
{
    public class WaitCommand : ICommand
    {
        private Level level;
        private Point waitingUnit;

        public WaitCommand(Level level, Point waitingUnit)
        {
            this.level = level;
            this.waitingUnit = waitingUnit;
        }

        public void Execute()
        {
            level.Wait(waitingUnit);
        }
    }
}
