using Microsoft.Xna.Framework;
using MizJam1.Levels;
using MizJam1.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.UIComponents.Commands
{
    public class MoveAndAttackCommand : ICommand
    {
        private Level level;
        private Unit attacking;
        private Unit defending;
        private MoveUnitCommand moveCommand;

        public MoveAndAttackCommand(MoveUnitCommand moveCommand, Level level, Unit attacking, Unit defending)
        {
            this.level = level;
            this.attacking = attacking;
            this.defending = defending;
            this.moveCommand = moveCommand;
        }

        public void Execute()
        {
            moveCommand.Execute();
            level.Attack(attacking.Position, defending.Position);
        }
    }
}
