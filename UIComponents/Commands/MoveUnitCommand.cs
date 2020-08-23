using Microsoft.Xna.Framework;
using MizJam1.Audio;
using MizJam1.Levels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.UIComponents.Commands
{
    public class MoveUnitCommand : ICommand
    {
        private Level level;
        private Point start;
        private Point end;

        public MoveUnitCommand(Level level, Point startingPoint, Point endPoint)
        {
            this.level = level;
            start = startingPoint;
            end = endPoint;
        }

        public void Execute()
        {
            level.MoveUnit(start, end);
        }
    }
}
