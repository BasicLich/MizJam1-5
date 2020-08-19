using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.Utilities
{
    public static class TextureIDInterpreter
    {
        public static int GetTextureID(uint id) => (int)(id - 1) / Global.SpriteSheetCount;
        private static int GetInTextureID(uint id) => (int)(id - 1) % Global.SpriteSheetCount;
        private static int GetSourceX(uint id) => (GetInTextureID(id) % Global.SpriteSheetWidth) * Global.SpriteWidth;
        private static int GetSourceY(uint id) => (GetInTextureID(id) / Global.SpriteSheetWidth) * Global.SpriteHeight;
        public static Rectangle GetSourceRectangle(uint id) => new Rectangle(GetSourceX(id), GetSourceY(id), Global.SpriteWidth, Global.SpriteHeight);
    }
}
