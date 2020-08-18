using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MizJam1.Levels
{
    public class Level
    {
        private readonly List<Cell[,]> layers;

        public Level(XDocument levelData)
        {
            layers = new List<Cell[,]>();
            foreach (var layer in levelData.Element("map").Elements("layer"))
            {
                Cell[,] cells = ParseLayer(layer.Element("data").Value);
                layers.Add(cells);
            }
        }

        private Cell[,] ParseLayer(string layerData)
        {
            Cell[,] cells = null;
            layerData = layerData.Replace("\r\n", "\n").Trim();

            string[] mapLines = layerData.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < mapLines.Count(); i++)
            {
                var mapline = mapLines[i].Trim();
                var columns = mapline.Split(",", StringSplitOptions.RemoveEmptyEntries);
                if (cells == null)
                {
                    cells = new Cell[mapLines.Count(), columns.Count()];
                }

                for (int j = 0; j < columns.Count(); j++)
                {
                    var col = columns[j].Trim();
                    cells[i, j] = new Cell(UInt32.Parse(col));
                }
            }
            return cells;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D[] textures)
        {
            foreach (var layer in layers)
            {
                for (int i = 0; i < layer.GetLength(0); i++)
                {
                    for (int j = 0; j < layer.GetLength(1); j++)
                    {
                        Cell cell = layer[i, j];
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
}
