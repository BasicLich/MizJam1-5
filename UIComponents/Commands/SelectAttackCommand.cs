using Microsoft.Xna.Framework;
using MizJam1.Levels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.UIComponents.Commands
{
    public class SelectAttackCommand : ICommand
    {
        private Level level;
        public SelectAttackCommand(Level level)
        {
            this.level = level;
        }

        public void Execute()
        {
            level.AttackSelect();
        }
    }
}
