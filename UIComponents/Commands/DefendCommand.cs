using Microsoft.Xna.Framework;
using MizJam1.Levels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.UIComponents.Commands
{
    public class DefendCommand : ICommand
    {
        private Level level;
        private Point defendingUnit;
        public DefendCommand(Level level, Point defendingUnit)
        {
            this.level = level;
            this.defendingUnit = defendingUnit;
        }

        public void Execute()
        {
            level.Defend(defendingUnit);
        }
    }
}
