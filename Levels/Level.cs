using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MizJam1.Levels
{
    public class Level
    {
        public Cell[,] Cells { get; set; }

        public Level(string mapData)
        {
            mapData = mapData.Replace("\r\n", "\n").Trim();

            string[] mapLines = mapData.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < mapLines.Count(); i++)
            {
                var mapline = mapLines[i].Trim();
                var columns = mapline.Split(",", StringSplitOptions.RemoveEmptyEntries);
                if (Cells == null)
                {
                    Cells = new Cell[mapLines.Count(), columns.Count()];
                }

                for (int j = 0; j < columns.Count(); j++)
                {
                    var col = columns[j].Trim();
                    Cells[i, j] = new Cell(UInt32.Parse(col));
                }
            }
        }



        public void Draw(SpriteBatch spriteBatch, Texture2D[] textures)
        {
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    Cell cell = Cells[i, j];
                    if (cell.ID == 0)
                    {
                        continue;
                    }
                    uint cellID = cell.ID - 1;
                    int texture = (int)(cellID) / Global.SpriteSheetCount;
                    int inTextureID = (int)cellID % Global.SpriteSheetCount;
                    int inSheetX = (inTextureID % Global.SpriteSheetWidth) * Global.SpriteWidth;
                    int inSheetY = (inTextureID / Global.SpriteSheetWidth) * Global.SpriteHeight;
                    Vector2 origin = new Vector2(Global.SpriteWidth / 2, Global.SpriteHeight / 2);
                    spriteBatch.Draw(
                        textures[texture],
                        new Rectangle(j * Global.SpriteWidth + (int)origin.X, i * Global.SpriteHeight + (int)origin.Y, Global.SpriteWidth, Global.SpriteHeight),
                        new Rectangle(inSheetX, inSheetY, Global.SpriteWidth, Global.SpriteHeight),
                        Color.White,
                        cell.Rotation,
                        origin,
                        (cell.FlippedHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (cell.FlippedVertically ? SpriteEffects.FlipVertically : SpriteEffects.None),
                        0f);
                }
            }
        }
    }
}
