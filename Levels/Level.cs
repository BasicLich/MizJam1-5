using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MizJam1.Units;
using MizJam1.Utilities;

namespace MizJam1.Levels
{
    public class Level
    {
        private readonly List<Cell[,]> layers;
        private readonly Unit[,] units;

        public Level(XDocument levelData)
        {
            var mapData = levelData.Element("map");

            layers = new List<Cell[,]>();
            foreach (var layer in mapData.Elements("layer"))
            {
                Cell[,] cells = ParseLayer(layer.Element("data").Value);
                layers.Add(cells);
            }
            units = new Unit[layers[0].GetLength(0), layers[0].GetLength(1)];

            var objectGroups = mapData.Elements("objectgroup");
            var playerGroup = objectGroups.Where(el => el.Attribute("name").Value == "Player").SingleOrDefault();
            if (playerGroup != null)
            {
                foreach (var obj in playerGroup.Elements("object"))
                {
                    Point pos = new Point((int)float.Parse(obj.Attribute("x").Value) / Global.SpriteWidth, (int)float.Parse(obj.Attribute("y").Value) / Global.SpriteHeight);
                    units[pos.Y, pos.X] = new Unit(uint.Parse(obj.Attribute("gid").Value), obj.Attribute("name").Value, UnitClass.UnitClasses[obj.Attribute("type").Value], false);
                }
            }

            var enemyGroup = objectGroups.Where(el => el.Attribute("name").Value == "Enemy").SingleOrDefault();
            if (enemyGroup != null)
            {
                foreach (var obj in enemyGroup.Elements("object"))
                {
                    Point pos = new Point((int)float.Parse(obj.Attribute("x").Value) / Global.SpriteWidth, (int)float.Parse(obj.Attribute("y").Value) / Global.SpriteHeight);
                    units[pos.Y, pos.X] = new Unit(uint.Parse(obj.Attribute("gid").Value), obj.Attribute("name").Value, UnitClass.UnitClasses[obj.Attribute("type").Value], true);
                }
            }
        }

        public Unit SelectedUnit { get; set; }
        public Cell MouseOverCell { get; set; }
        public Unit MouseOverUnit { get; set; }

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

        public void MouseOver(Point position)
        {
            position = new Point(position.X / Global.SpriteWidth, position.Y / Global.SpriteHeight);
            if (position.X > 0 && position.Y > 0 && position.X < layers[0].GetLength(1) && position.Y < layers[0].GetLength(0))
            {
                MouseOverUnit = units[position.Y, position.X];
                for (int i = layers.Count - 1; i >= 0; i--)
                {
                    var layer = layers[i];
                    MouseOverCell = layer[position.Y, position.X];
                    if (layer[position.Y, position.X].ID != 0)
                    {
                        break;
                    }
                }
            }
            else
            {
                MouseOverUnit = null;
                MouseOverCell = new Cell();
            }
        }

        public void LeftClick(Point position)
        {
            position = new Point(position.X / Global.SpriteWidth, position.Y / Global.SpriteHeight);
            if (position.X > 0 && position.Y > 0 && position.X < layers[0].GetLength(1) && position.Y < layers[0].GetLength(0))
            {
                SelectedUnit = units[position.Y, position.X];
            }
            else
            {
                SelectedUnit = null;
            }
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
                        Vector2 origin = new Vector2(Global.SpriteWidth / 2, Global.SpriteHeight / 2);
                        spriteBatch.Draw(
                            textures[TextureIDInterpreter.GetTextureID(cell.ID)],
                            new Rectangle(j * Global.SpriteWidth + (int)origin.X, i * Global.SpriteHeight + (int)origin.Y, Global.SpriteWidth, Global.SpriteHeight),
                            TextureIDInterpreter.GetSourceRectangle(cell.ID),
                            Color.White,
                            cell.Rotation,
                            origin,
                            (cell.FlippedHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (cell.FlippedVertically ? SpriteEffects.FlipVertically : SpriteEffects.None),
                            0f);
                    }
                }
            }

            for (int i = 0; i < units.GetLength(0); i++)
            {
                for (int j = 0; j < units.GetLength(1); j++)
                {
                    Unit unit = units[i, j];
                    if (unit == null) continue;

                    Vector2 origin = new Vector2(Global.SpriteWidth / 2, Global.SpriteHeight / 2);
                    spriteBatch.Draw(
                            textures[TextureIDInterpreter.GetTextureID(unit.ID)],
                            new Rectangle(j * Global.SpriteWidth + (int)origin.X, i * Global.SpriteHeight + (int)origin.Y, Global.SpriteWidth, Global.SpriteHeight),
                            TextureIDInterpreter.GetSourceRectangle(unit.ID),
                            unit.Enemy ? Global.Colors.Accent4 : Global.Colors.Accent2,
                            unit.Rotation,
                            origin,
                            (unit.FlippedHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (unit.FlippedVertically ? SpriteEffects.FlipVertically : SpriteEffects.None),
                            0f);
                }
            }
        }
    }
}
