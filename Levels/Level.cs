using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MizJam1.Units;
using MizJam1.Utilities;
using MizJam1.UIComponents;
using MizJam1.UIComponents.Commands;

namespace MizJam1.Levels
{
    public class Level
    {
        private readonly Random rand = new Random();
        private readonly List<Cell[,]> layers;
        private readonly Unit[,] units;
        private float animationTimer = 0;
        private readonly MizJam1Game game;

        private UIMenu dialog;

        public Unit SelectedUnit { get; set; }
        private HashSet<Point> canGoPositions;
        private Dictionary<Point, ThreatenedCell> threatenedPositions;
        public Cell MouseOverCell { get; set; }
        public Unit MouseOverUnit { get; set; }
        public Point Size { get; private set; }
        public int Width => Size.X;
        public int Height => Size.Y;

        public Level(XDocument levelData, MizJam1Game game)
        {
            this.game = game;
            var mapData = levelData.Element("map");

            Size = new Point(int.Parse(mapData.Attribute("width").Value), int.Parse(mapData.Attribute("height").Value));

            layers = new List<Cell[,]>();
            foreach (var layer in mapData.Elements("layer"))
            {
                Cell[,] cells = ParseLayer(layer.Element("data").Value);
                layers.Add(cells);
            }
            units = new Unit[Height, Width];

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

        private Cell[,] ParseLayer(string layerData)
        {
            Cell[,] cells = null;
            layerData = layerData.Replace("\r\n", "\n").Trim();

            string[] mapLines = layerData.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < Height; i++)
            {
                var mapline = mapLines[i].Trim();
                var columns = mapline.Split(",", StringSplitOptions.RemoveEmptyEntries);
                if (cells == null)
                {
                    cells = new Cell[Height, Width];
                }

                for (int j = 0; j < Width; j++)
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
            return position.X < 0 || position.Y < 0 || position.X >= Width || position.Y >= Height;
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

        public void MouseOver(Point worldPosition, Point screenPosition)
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

            if (dialog != null)
            {
                foreach (var child in dialog.Children)
                {
                    if (child.Contains(screenPosition))
                    {
                        child.Select();
                    }
                    else
                    {
                        child.Deselect();
                    }
                }
            }
        }

        public void LeftClick(Point worldPosition, Point screenPosition)
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
                    FightPhaseLeftClick(worldPosition, screenPosition);
                    break;
                case MizJam1Game.GameStates.OpenDialog:
                    OpenDialogLeftClick(worldPosition, screenPosition);
                    break;
                case MizJam1Game.GameStates.SelectAttack:
                    ChooseAttackLeftClick(worldPosition);
                    break;
                case MizJam1Game.GameStates.DefensePhase:
                    break;
                default:
                    break;
            }
        }

        private void FightPhaseLeftClick(Point worldPosition, Point screenPosition)
        {
            Point position = WorldToCell(worldPosition);

            if (!IsPositionInvalid(position))
            {
                if (SelectedUnit != null &&
                    !SelectedUnit.Enemy &&
                    !SelectedUnit.Acted &&
                        (position == SelectedUnit.Position ||
                        (canGoPositions != null && canGoPositions.Contains(position)) ||
                        (threatenedPositions != null && threatenedPositions.ContainsKey(position))))
                {
                    if (threatenedPositions.ContainsKey(position) && !canGoPositions.Contains(position) && (GetUnit(position)?.Enemy ?? false))
                    {
                        threatenedPositions = new Dictionary<Point, ThreatenedCell>() { [position] = threatenedPositions[position] };
                        canGoPositions = new HashSet<Point>() { threatenedPositions[position].From };
                        OpenDialog(position, screenPosition);
                        return;
                    }
                    else if (canGoPositions.Contains(position))
                    {
                        canGoPositions = new HashSet<Point>() { position };
                        canGoPositions.Remove(SelectedUnit.Position);
                        threatenedPositions = GetThreatenSpaces(new HashSet<Point>() { position }, SelectedUnit);
                        OpenDialog(position, screenPosition);
                        return;
                    }
                    else if (position == SelectedUnit.Position)
                    {
                        canGoPositions = new HashSet<Point>();
                        threatenedPositions = GetThreatenSpaces(new HashSet<Point>() { position }, SelectedUnit);
                        OpenDialog(position, screenPosition);
                        return;
                    }
                }

                SelectedUnit = units[position.Y, position.X];
                if (SelectedUnit != null && !SelectedUnit.Acted)
                {
                    canGoPositions = GetAccessiblePoints(SelectedUnit);

                    foreach (var unit in units)
                    {
                        if (unit == null) continue;
                        if (unit.Enemy == SelectedUnit.Enemy && unit != SelectedUnit)
                        {
                            canGoPositions.Remove(unit.Position);
                        }
                    }
                    threatenedPositions = GetThreatenSpaces(canGoPositions, SelectedUnit);

                    canGoPositions.Remove(new Point(position.X, position.Y));
                    threatenedPositions.Remove(new Point(position.X, position.Y));

                }
                else UnselectTiles();
            }
            else
            {
                UnselectUnit();
                UnselectTiles();
            }
        }

        private void OpenDialog(Point cellClicked, Point screenPosition)
        {
            game.GameState = MizJam1Game.GameStates.OpenDialog;
            dialog = new UIMenu(screenPosition, new Point(500, 500), true)
            {
                SpaceBetweenChildren = 5,
                Vertical = true
            };
            Unit enemy;

            bool attackDirectly = false;
            if ((enemy = GetUnit(cellClicked)) != null && enemy.Enemy)
            {
                UIImage attack = new UIImage(game.Dialogs[MizJam1Game.Actions.Attack], game.SelectedDialogs[MizJam1Game.Actions.Attack]);
                Point moveTarget = threatenedPositions[cellClicked].From;
                MoveUnitCommand moveUnit = new MoveUnitCommand(this, SelectedUnit.Position, moveTarget);
                MoveAndAttackCommand moveAndAttackCommand = new MoveAndAttackCommand(moveUnit, this, SelectedUnit, enemy);
                attack.AddCommand(moveAndAttackCommand);
                dialog.AddChild(attack);
                attackDirectly = true;
            }

            if (!attackDirectly)
            {
                foreach (var threat in threatenedPositions)
                {
                    if ((enemy = GetUnit(threat.Key)) != null && enemy.Enemy)
                    {
                        UIImage attackSelect = new UIImage(game.Dialogs[MizJam1Game.Actions.Attack], game.SelectedDialogs[MizJam1Game.Actions.Attack]);
                        SelectAttackCommand selectAttackCommand = new SelectAttackCommand(this);
                        attackSelect.AddCommand(selectAttackCommand);
                        dialog.AddChild(attackSelect);
                        break;
                    }
                }
            }

            if (cellClicked == SelectedUnit.Position)
            {
                UIImage defend = new UIImage(game.Dialogs[MizJam1Game.Actions.Defend], game.SelectedDialogs[MizJam1Game.Actions.Defend]);
                DefendCommand defendCommand = new DefendCommand(this, SelectedUnit.Position);
                defend.AddCommand(defendCommand);
                dialog.AddChild(defend);
            }
            MoveUnitCommand moveCommand = null;
            if (canGoPositions.Contains(cellClicked))
            {
                UIImage move = new UIImage(game.Dialogs[MizJam1Game.Actions.Move], game.SelectedDialogs[MizJam1Game.Actions.Move]);
                moveCommand = new MoveUnitCommand(this, SelectedUnit.Position, cellClicked);
                move.AddCommand(moveCommand);
                dialog.AddChild(move);
                canGoPositions = new HashSet<Point>() { cellClicked };
            }

            if (!attackDirectly)
            {
                UIImage reroll = new UIImage(game.Dialogs[MizJam1Game.Actions.Reroll], game.SelectedDialogs[MizJam1Game.Actions.Reroll]);
                if (moveCommand == null)
                {
                    moveCommand = new MoveUnitCommand(this, SelectedUnit.Position, SelectedUnit.Position);
                }
                ICommand rerollCommand = new MoveAndRerollCommand(moveCommand, SelectedUnit);
                reroll.AddCommand(rerollCommand);
                dialog.AddChild(reroll);
            }

            if (cellClicked == SelectedUnit.Position)
            {
                UIImage wait = new UIImage(game.Dialogs[MizJam1Game.Actions.Wait], game.SelectedDialogs[MizJam1Game.Actions.Wait]);
                WaitCommand waitCommand = new WaitCommand(this, cellClicked);
                wait.AddCommand(waitCommand);
                dialog.AddChild(wait);
            }

            UIImage cancel = new UIImage(game.Dialogs[MizJam1Game.Actions.Cancel], game.SelectedDialogs[MizJam1Game.Actions.Cancel]);
            ICommand cancelCommand = new CancelCommand(this);
            cancel.AddCommand(cancelCommand);
            dialog.AddChild(cancel);

            dialog.SetScale(3);

            if (dialog.Position.X + dialog.Size.X > 1500)
            {
                dialog.Position = new Point(screenPosition.X - dialog.Size.X, dialog.Position.Y);
            }
            if (dialog.Position.Y + dialog.Size.Y > 1080)
            {
                dialog.Position = new Point(dialog.Position.X, screenPosition.Y - dialog.Size.Y);
            }
        }

        private void OpenDialogLeftClick(Point worldPosition, Point screenPosition)
        {
            if (game.GameState == MizJam1Game.GameStates.OpenDialog)
            {
                game.GameState = MizJam1Game.GameStates.FightPhase;
            }
            if (dialog.Contains(screenPosition))
            {
                dialog.Execute();
            }
            else
            {
                dialog = null;
                FightPhaseLeftClick(worldPosition, screenPosition);
            }

            dialog = null;
        }

        private void ChooseAttackLeftClick(Point worldPosition)
        {
            game.GameState = MizJam1Game.GameStates.FightPhase;

            Point cell = WorldToCell(worldPosition);
            if (!threatenedPositions.ContainsKey(cell))
            {
                Cancel();

                return;
            }

            Unit unit = SelectedUnit;
            Point moveDestination = SelectedUnit.Position;
            if(canGoPositions.Any())
            {
                moveDestination = canGoPositions.First();
            }
            MoveUnit(unit.Position, moveDestination);
            Attack(unit.Position, cell);
        }

        private void UnselectUnit()
        {
            SelectedUnit = null;
        }

        private void UnselectTiles()
        {
            canGoPositions = null;
            threatenedPositions = null;
        }

        public void MoveUnit(Point unitPos, Point destination)
        {
            Unit unit = units[unitPos.Y, unitPos.X];
            units[unitPos.Y, unitPos.X] = null;
            units[destination.Y, destination.X] = unit;
            unit.Position = destination;
            unit.Acted = true;

            UnselectUnit();
            UnselectTiles();
        }

        public void AttackSelect()
        {
            foreach (var threat in threatenedPositions.ToList())
            {
                if (GetUnit(threat.Key) == null)
                {
                    threatenedPositions.Remove(threat.Key);
                }
            }

            game.GameState = MizJam1Game.GameStates.SelectAttack;
        }

        public void Attack(Point attackingUnit, Point defendingUnit)
        {
            Unit att = GetUnit(attackingUnit);
            Unit def = GetUnit(defendingUnit);

            ushort baseAtt = att.Stats[Stats.Attack];
            bool isMagic = false;
            if (att.Stats[Stats.Magic] > baseAtt)
            {
                isMagic = true;
                baseAtt = att.Stats[Stats.Magic];
            }
            int damage = baseAtt + rand.Next(1, 7);

            ushort baseDef = def.Defending ? (ushort)7 : (isMagic ? def.Stats[Stats.MagicDefense] : def.Stats[Stats.Defense]);
            int defense = baseDef + rand.Next(1, 7);

            int res = Math.Abs(damage - defense);
            if (def.Health <= res)
            {
                units[def.Position.Y, def.Position.X] = null;
            }
            else
            {
                def.Health -= (ushort)res;
            }

            att.Acted = true;

            UnselectUnit();
            UnselectTiles();
        }

        public void Defend(Point defendingUnit)
        {
            Unit unit = GetUnit(defendingUnit);
            unit.Defending = true;
            unit.Acted = true;

            UnselectUnit();
            UnselectTiles();
        }

        public void Wait(Point waitingUnit)
        {
            Unit unit = GetUnit(waitingUnit);
            unit.Acted = true;

            UnselectUnit();
            UnselectTiles();
        }

        public void Cancel()
        {
            SelectedUnit = null;
            game.GameState = MizJam1Game.GameStates.FightPhase;
            canGoPositions = null;
            threatenedPositions = null;
        }

        private Dictionary<Point, ThreatenedCell> GetThreatenSpaces(HashSet<Point> reach, Unit unit)
        {
            var threathenedSpaces = new Dictionary<Point, ThreatenedCell>();

            int minimumRange = 1;
            if (unit.Stats[Stats.Range] > 0 && unit.Stats[Stats.Magic] == 0)
            {
                minimumRange = 2;
            }
            foreach (var pos in reach)
            {
                for (int i = unit.Stats[Stats.Range] + 1; i >= minimumRange; i--)
                {
                    int currPriority = 1000 - MathExtensions.CellDistance(unit.Position, pos);
                    for (int j = i; j != 0; j--)
                    {
                        Point threatPos = new Point(pos.X - j, pos.Y - (i - j));
                        if (threathenedSpaces.ContainsKey(threatPos) && threathenedSpaces[threatPos].Priority >= currPriority)
                        {
                            continue;
                        }
                        threathenedSpaces[threatPos] = new ThreatenedCell() { Position = threatPos, From = pos, Priority = currPriority };
                    }
                    for (int j = i; j != 0; j--)
                    {
                        Point threatPos = new Point(pos.X + (i - j), pos.Y - j);
                        if (threathenedSpaces.ContainsKey(threatPos) && threathenedSpaces[threatPos].Priority >= currPriority)
                        {
                            continue;
                        }
                        threathenedSpaces[threatPos] = new ThreatenedCell() { Position = threatPos, From = pos, Priority = currPriority };
                    }
                    for (int j = i; j != 0; j--)
                    {
                        Point threatPos = new Point(pos.X + j, pos.Y + (i - j));
                        if (threathenedSpaces.ContainsKey(threatPos) && threathenedSpaces[threatPos].Priority >= currPriority)
                        {
                            continue;
                        }
                        threathenedSpaces[threatPos] = new ThreatenedCell() { Position = threatPos, From = pos, Priority = currPriority };
                    }
                    for (int j = i; j != 0; j--)
                    {
                        Point threatPos = new Point(pos.X - (i - j), pos.Y + j);
                        if (threathenedSpaces.ContainsKey(threatPos) && threathenedSpaces[threatPos].Priority >= currPriority)
                        {
                            continue;
                        }
                        threathenedSpaces[threatPos] = new ThreatenedCell() { Position = threatPos, From = pos, Priority = currPriority };
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

        /// <summary>
        /// Recursive function that gets the accessible cells for the given range
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rangeLeft"></param>
        /// <param name="result"></param>
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
            if (game.GameState == MizJam1Game.GameStates.FightPhase && TurnFinished(false))
            {
                game.GameState = MizJam1Game.GameStates.DefensePhase;
                WakeUpUnits(true);
                DoEnemyTurn();
            }
            else if (game.GameState == MizJam1Game.GameStates.DefensePhase && TurnFinished(true))
            {
                game.GameState = MizJam1Game.GameStates.FightPhase;
                WakeUpUnits(false);
            }

            animationTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (animationTimer > 256000) animationTimer -= 256000;
        }

        private bool TurnFinished(bool enemyTurn)
        {
            foreach (var unit in units)
            {
                if (unit != null && unit.Enemy == enemyTurn && !unit.Acted)
                {
                    return false;
                }
            }
            return true;
        }

        public bool LevelFinished
        {
            get
            {

                foreach (var unit in units)
                {
                    if (unit != null && unit.Enemy)
                    {
                        return false;
                    }
                }

                return true;

            }
        }

        private void WakeUpUnits(bool enemies)
        {
            foreach (var unit in units)
            {
                if (unit == null) continue;
                unit.Acted = false;
                if (enemies == unit.Enemy)
                {
                    unit.Defending = false;
                }
            }
        }

        private void DoEnemyTurn()
        {
            foreach (var unit in units)
            {
                if (unit != null && unit.Enemy)
                {
                    Defend(unit.Position);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteBatch screenSpriteBatch, Texture2D[] textures)
        {
            DrawCells(spriteBatch, textures);
            DrawAreaTiles(spriteBatch);
            DrawUnits(spriteBatch, textures);
            DrawUI(screenSpriteBatch);
        }

        private void DrawCells(SpriteBatch spriteBatch, Texture2D[] textures)
        {
            foreach (var layer in layers)
            {
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
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

        private void DrawAreaTiles(SpriteBatch spriteBatch)
        {
            if (canGoPositions != null)
            {
                int animation = ((int)animationTimer) / 100 % 4;
                foreach (var threatPos in threatenedPositions)
                {
                    if (GetCell(threatPos.Key).ID == 0) continue;

                    spriteBatch.Draw(
                        game.TransparentTileSelect,
                        new Rectangle(CellToWorld(threatPos.Key), Global.SpriteSize),
                        new Rectangle(new Point(Global.SpriteWidth * animation, 0), Global.SpriteSize),
                        Global.Colors.Accent4);
                }
                foreach (var pos in canGoPositions)
                {
                    if (GetCell(pos).ID == 0) continue;
                    Color color = SelectedUnit.Enemy ? Global.Colors.Accent4 : Global.Colors.Accent3;

                    spriteBatch.Draw(
                        game.TransparentTileSelect,
                        new Rectangle(CellToWorld(pos), Global.SpriteSize),
                        new Rectangle(new Point(Global.SpriteWidth * animation, 0), Global.SpriteSize),
                        color);
                }
            }
        }

        private void DrawUnits(SpriteBatch spriteBatch, Texture2D[] textures)
        {
            int animation = ((int)animationTimer) / 500 % 2;

            if (SelectedUnit != null)
            {
                int borderAnimation = ((int)animationTimer) / 100 % 4;
                spriteBatch.Draw(
                    game.SelectedUnitBorder,
                    new Rectangle(CellToWorld(SelectedUnit.Position), Global.SpriteSize),
                    new Rectangle(new Point(Global.SpriteWidth * borderAnimation, 0), Global.SpriteSize),
                    Color.White);
            }

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Unit unit = units[i, j];
                    if (unit == null) continue;
                    Color color1 = Global.Colors.Accent3;
                    Color color2 = Global.Colors.Accent2;
                    if (unit.Enemy)
                    {
                        color1 = Global.Colors.Accent4;
                        color2 = Global.Colors.Accent1;
                    }
                    Color currColor = color1;
                    if (!unit.Acted && animation == 1)
                    {
                        currColor = color2;
                    }

                    Vector2 origin = new Vector2(Global.SpriteWidth / 2, Global.SpriteHeight / 2);
                    spriteBatch.Draw(
                            textures[TextureIDInterpreter.GetTextureID(unit.ID)],
                            new Rectangle(j * Global.SpriteWidth + (int)origin.X, i * Global.SpriteHeight + (int)origin.Y, Global.SpriteWidth, Global.SpriteHeight),
                            TextureIDInterpreter.GetSourceRectangle(unit.ID),
                            currColor,
                            unit.Rotation,
                            origin,
                            (unit.FlippedHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (unit.FlippedVertically ? SpriteEffects.FlipVertically : SpriteEffects.None),
                            0f);
                }
            }
        }

        private void DrawUI(SpriteBatch screenSpriteBatch)
        {
            if (dialog != null)
            {
                dialog.Draw(screenSpriteBatch);
            }
        }
    }
}
