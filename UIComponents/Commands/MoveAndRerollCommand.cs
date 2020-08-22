using MizJam1.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.UIComponents.Commands
{
    public class MoveAndRerollCommand : ICommand
    {
        private Unit unit;
        private ICommand moveUnitCommand;
        public MoveAndRerollCommand(MoveUnitCommand moveUnitCommand, Unit unit)
        {
            this.unit = unit;
            this.moveUnitCommand = moveUnitCommand;
        }

        public void Execute()
        {
            moveUnitCommand.Execute();
            unit.Reroll();
            unit.Acted = true;
        }
    }
}
