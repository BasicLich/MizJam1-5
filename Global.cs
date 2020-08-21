using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1
{
    public static class Global
    {
        public static readonly int SpriteWidth = 16;
        public static readonly int SpriteHeight = 16;
        public static readonly Point SpriteSize = new Point(SpriteWidth, SpriteHeight);
        public static readonly int SpriteSheetWidth = 48;
        public static readonly int SpriteSheetHeight = 22;
        public static readonly int SpriteSheetCount = SpriteSheetWidth * SpriteSheetHeight;

        public static class Colors
        {
            /// <summary>
            /// Yellow
            /// </summary>
            public static readonly Color Accent1 = new Color(0xff1bb4f4);
            /// <summary>
            /// Green
            /// </summary>
            public static readonly Color Accent2 = new Color(0xff73d938);
            /// <summary>
            /// Blue
            /// </summary>
            public static readonly Color Accent3 = new Color(0xffd7ac3c);
            /// <summary>
            /// Red
            /// </summary>
            public static readonly Color Accent4 = new Color(0xff2e48e6);
            /// <summary>
            /// "White"
            /// </summary>
            public static readonly Color Main1 = new Color(0xffb8c6cf);
            /// <summary>
            /// Wood color
            /// </summary>
            public static readonly Color Main2 = new Color(0xff5879bf);
            /// <summary>
            /// Dark
            /// </summary>
            public static readonly Color Background1 = new Color(0xff3c2d47);
            /// <summary>
            /// Lighter dark
            /// </summary>
            public static readonly Color Background2 = new Color(0xff4a447a);
        }
    }
}
