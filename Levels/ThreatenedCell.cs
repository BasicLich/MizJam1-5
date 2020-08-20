using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.Levels
{
    struct ThreatenedCell
    {
        public Point Position { get; set; }
        public Point From { get; set; }
        public int Priority { get; set; }
    }
}
