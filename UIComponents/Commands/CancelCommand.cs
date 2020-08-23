using Microsoft.Xna.Framework;
using MizJam1.Audio;
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
            AudioManager.Instance.PlaySoundEffect("MenuClick", Vector2.Zero);

            level.Cancel();
        }
    }
}
