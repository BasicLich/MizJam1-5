using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1
{
    public static class Global
    {
        public static readonly int MapWidth = 20;
        public static readonly int MapHeight = 20;
        public static readonly int SpriteWidth = 16;
        public static readonly int SpriteHeight = 16;
        public static readonly Point SpriteSize = new Point(SpriteWidth, SpriteHeight);
        public static readonly int SpriteSheetWidth = 48;
        public static readonly int SpriteSheetHeight = 22;
        public static readonly int SpriteSheetCount = SpriteSheetWidth * SpriteSheetHeight;

        public static class Colors
        {
            public static readonly Color Accent1 = new Color(0xff1bb4f4);
            public static readonly Color Accent2 = new Color(0xff73d938);
            public static readonly Color Accent3 = new Color(0xffd7ac3c);
            public static readonly Color Accent4 = new Color(0xff2e48e6);
            public static readonly Color Main1 = new Color(0xffb8c6cf);
            public static readonly Color Main2 = new Color(0xff5879bf);
            public static readonly Color Main3 = new Color(0xff4a447a);
            public static readonly Color Background1 = new Color(0xff3c2d47);
        }
    }
}
