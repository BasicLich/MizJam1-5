using MizJam1.Levels;
using MizJam1.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.UIComponents.Commands
{
    public class MoveAndRerollCommand : ICommand
    {
        private Level level;
        private Unit unit;
        private ICommand moveUnitCommand;

        public MoveAndRerollCommand(MoveUnitCommand moveUnitCommand,Level level, Unit unit)
        {
            this.level = level;
            this.unit = unit;
            this.moveUnitCommand = moveUnitCommand;
        }

        public void Execute()
        {
            moveUnitCommand.Execute();
            level.Reroll(unit.Position);
        }
    }
}
