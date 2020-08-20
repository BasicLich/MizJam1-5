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
        private float animationTimer = 0;
        private readonly MizJam1Game game;

        public Level(XDocument levelData, MizJam1Game game)
        {
            this.game = game;
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
                    Unit unit = new Unit(uint.Parse(obj.Attribute("gid").Value), obj.Attribute("name").Value, UnitClass.UnitClasses[obj.Attribute("type").Value], false);
                    unit.Position = pos;
                    units[pos.Y, pos.X] = unit;
                }
            }

            var enemyGroup = objectGroups.Where(el => el.Attribute("name").Value == "Enemy").SingleOrDefault();
            if (enemyGroup != null)
            {
                foreach (var obj in enemyGroup.Elements("object"))
                {
                    Point pos = new Point((int)float.Parse(obj.Attribute("x").Value) / Global.SpriteWidth, (int)float.Parse(obj.Attribute("y").Value) / Global.SpriteHeight);
                    Unit unit = new Unit(uint.Parse(obj.Attribute("gid").Value), obj.Attribute("name").Value, UnitClass.UnitClasses[obj.Attribute("type").Value], true);
                    unit.Position = pos;
                    units[pos.Y, pos.X] = unit;
                }
            }
        }

        public Unit SelectedUnit { get; set; }
        public HashSet<Point> CanGoPositions { get; set; }
        public HashSet<Point> ThreathenPositions { get; set; }
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

        public void Start()
        {
            game.GameState = MizJam1Game.GameStates.FightPhase;
        }

        private bool IsPositionInvalid(Point position)
        {
            return position.X < 0 || position.Y < 0 || position.X >= layers[0].GetLength(1) || position.Y >= layers[0].GetLength(0);
        }

        private Cell GetCell(Point position)
        {
            if (IsPositionInvalid(position))
            {
                return new Cell();
            }

            for (int i = layers.Count - 1; i >= 0; i--)
            {
                var layer = layers[i];
                if (layer[position.Y, position.X].ID != 0)
                {
                    return layer[position.Y, position.X];
                }
            }
            return new Cell();
        }

        private Unit GetUnit(Point position)
        {
            if (IsPositionInvalid(position))
            {
                return null;
            }

            return units[position.Y, position.X];
        }

        private Point WorldToCell(Point position) => new Point(position.X / Global.SpriteWidth, position.Y / Global.SpriteHeight);
        private Point CellToWorld(Point position) => new Point(position.X * Global.SpriteWidth, position.Y * Global.SpriteHeight);

        public void MouseOver(Point worldPosition)
        {
            var position = WorldToCell(worldPosition);
            if (IsPositionInvalid(position))
            {
                MouseOverUnit = null;
                MouseOverCell = new Cell();
            }
            else
            {
                MouseOverUnit = units[position.Y, position.X];
                MouseOverCell = GetCell(position);
            }
        }

        public void LeftClick(Point position)
        {
            switch (game.GameState)
            {
                case MizJam1Game.GameStates.MainMenu:
                    break;
                case MizJam1Game.GameStates.Playing:
                    break;
                case MizJam1Game.GameStates.Paused:
                    break;
                case MizJam1Game.GameStates.PrefightPhase:
                    break;
                case MizJam1Game.GameStates.FightPhase:
                    FightPhaseLeftClick(position);
                    break;
                case MizJam1Game.GameStates.DefensePhase:
                    break;
                default:
                    break;
            }
        }

        private void FightPhaseLeftClick(Point worldPosition)
        {
            Point position = WorldToCell(worldPosition);

            if (!IsPositionInvalid(position))
            {
                if (SelectedUnit != null && CanGoPositions.Contains(position) && !SelectedUnit.Enemy)
                {
                    MoveUnit(SelectedUnit.Position, position);
                    return;
                }

                SelectedUnit = units[position.Y, position.X];
                if (SelectedUnit != null)
                {
                    CanGoPositions = GetAccessiblePoints(SelectedUnit);

                    foreach (var unit in units)
                    {
                        if (unit == null) continue;
                        if (unit.Enemy == SelectedUnit.Enemy && unit != SelectedUnit)
                        {
                            CanGoPositions.Remove(unit.Position);
                        }
                    }
                    ThreathenPositions = GetThreathenSpaces(CanGoPositions, SelectedUnit);

                    CanGoPositions.Remove(new Point(position.X, position.Y));
                    ThreathenPositions.Remove(new Point(position.X, position.Y));

                }
                else UnselectTiles();
            }
            else
            {
                UnselectUnit();
                UnselectTiles();
            }
        }

        private void UnselectUnit()
        {
            SelectedUnit = null;
        }

        private void UnselectTiles()
        {
            CanGoPositions = null;
            ThreathenPositions = null;
        }

        public void MoveUnit(Point unitPos, Point destination)
        {
            units[destination.Y, destination.X] = units[unitPos.Y, unitPos.X];
            units[unitPos.Y, unitPos.X] = null;
            units[destination.Y, destination.X].Position = destination;

            UnselectUnit();
            UnselectTiles();
        }
        private HashSet<Point> GetThreathenSpaces(HashSet<Point> reach, Unit unit)
        {
            var threathenedSpaces = new HashSet<Point>();

            int minimumRange = 1;
            if (unit.Stats[Stats.Range] > 0 && unit.Stats[Stats.Magic] == 0)
            {
                minimumRange = 2;
            }
            foreach (var pos in reach)
            {
                for (int i = unit.Stats[Stats.Range] + 1; i >= minimumRange; i--)
                {
                    for (int j = i; j != 0; j--)
                    {
                        threathenedSpaces.Add(new Point(pos.X - j, pos.Y - (i - j)));
                    }
                    for (int j = i; j != 0; j--)
                    {
                        threathenedSpaces.Add(new Point(pos.X + (i - j), pos.Y - j));
                    }
                    for (int j = i; j != 0; j--)
                    {
                        threathenedSpaces.Add(new Point(pos.X + j, pos.Y + (i - j)));
                    }
                    for (int j = i; j != 0; j--)
                    {
                        threathenedSpaces.Add(new Point(pos.X - (i - j), pos.Y + j));
                    }
                }
            }

            foreach (var u in units)
            {
                if (u == null) continue;
                if (u.Enemy == unit.Enemy)
                {
                    threathenedSpaces.Remove(u.Position);
                }
            }

            return threathenedSpaces;
        }

        private HashSet<Point> GetAccessiblePoints(Unit unit)
        {
            Dictionary<Point, int> result = new Dictionary<Point, int>();
            GetAccessiblePoints(unit.Position, unit.Stats[Stats.Speed] * 20, result);
            var hashset = result.Keys.ToHashSet();

            return hashset;
        }

        private void GetAccessiblePoints(Point position, int rangeLeft, Dictionary<Point, int> result)
        {
            if (rangeLeft < 0)
            {
                return;
            }
            if (result.ContainsKey(position) && result[position] >= rangeLeft)
            {
                return;
            }
            result[position] = rangeLeft;

            CheckAndAddCell(new Point(position.X - 1, position.Y), rangeLeft, result);
            CheckAndAddCell(new Point(position.X + 1, position.Y), rangeLeft, result);
            CheckAndAddCell(new Point(position.X, position.Y - 1), rangeLeft, result);
            CheckAndAddCell(new Point(position.X, position.Y + 1), rangeLeft, result);

            return;
        }

        private void CheckAndAddCell(Point position, int rangeLeft, Dictionary<Point, int> result)
        {
            var cell = GetCell(position);
            if (cell.ID != 0 && !cell.Properties.IsSolid)
            {
                Unit unit;
                if ((unit = GetUnit(position)) != null && SelectedUnit.Enemy != unit.Enemy) //Can't pass through enemy units
                {
                    return;
                }
                GetAccessiblePoints(position, rangeLeft - cell.Properties.Difficulty, result);
            }
        }

        public void Update(GameTime gameTime)
        {
            animationTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (animationTimer > 256000) animationTimer -= 256000;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D[] textures, Texture2D tileSelectedTexture)
        {
            DrawCells(spriteBatch, textures);

            DrawAreaTiles(spriteBatch, tileSelectedTexture);

            DrawUnits(spriteBatch, textures);
        }

        private void DrawCells(SpriteBatch spriteBatch, Texture2D[] textures)
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
        }

        private void DrawAreaTiles(SpriteBatch spriteBatch, Texture2D tileSelectedTexture)
        {
            if (CanGoPositions != null)
            {
                int animation = ((int)animationTimer) / 100 % 4;
                foreach (var pos in ThreathenPositions)
                {
                    if (GetCell(pos).ID == 0) continue;

                    spriteBatch.Draw(
                        tileSelectedTexture,
                        new Rectangle(CellToWorld(pos), Global.SpriteSize),
                        new Rectangle(new Point(Global.SpriteWidth * animation, 0), Global.SpriteSize),
                        Global.Colors.Accent4);
                }
                foreach (var pos in CanGoPositions)
                {
                    if (GetCell(pos).ID == 0) continue;
                    Color color = SelectedUnit.Enemy ? Global.Colors.Accent4 : Global.Colors.Accent3;

                    spriteBatch.Draw(
                        tileSelectedTexture,
                        new Rectangle(CellToWorld(pos), Global.SpriteSize),
                        new Rectangle(new Point(Global.SpriteWidth * animation, 0), Global.SpriteSize),
                        color);
                }
            }
        }

        private void DrawUnits(SpriteBatch spriteBatch, Texture2D[] textures)
        {
            int animation = ((int)animationTimer) / 500 % 2;
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
                            unit.Enemy ? animation == 0 ? Global.Colors.Accent4 : Global.Colors.Accent1 : animation == 0 ? Global.Colors.Accent2 : Global.Colors.Accent3,
                            unit.Rotation,
                            origin,
                            (unit.FlippedHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (unit.FlippedVertically ? SpriteEffects.FlipVertically : SpriteEffects.None),
                            0f);
                }
            }
        }
    }
}
