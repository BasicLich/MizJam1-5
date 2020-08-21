using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MizJam1.Inputs;
using MizJam1.Levels;
using MizJam1.Rendering;
using MizJam1.UIComponents;
using MizJam1.UIComponents.Commands;
using MizJam1.Units;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace MizJam1
{
    public class MizJam1Game : Game
    {
        public enum GameStates
        {
            MainMenu,
            Playing,
            Paused,
            PrefightPhase,
            FightPhase,
            OpenDialog,
            DefensePhase
        }

        public enum Actions
        {
            Wait,
            Move,
            Defend,
            Attack,
            Reroll,
            Heal,
            EndTurn,
            Cancel
        }

        private GraphicsDeviceManager graphics;
        private SpriteBatch mapSpriteBatch;
        private SpriteBatch screenSpriteBatch;
        private Camera camera;

        private SpriteFont mizjamBigFont;
        private SpriteFont mizjamSmallFont;
        private Texture2D whitePixel;
        private Texture2D[] textures;
        private Texture2D windowBorder;
        public Texture2D TransparentTileSelect { get; set; }
        public Texture2D SelectedUnitBorder { get; set; }
        public Dictionary<Actions, Texture2D> Dialogs { get; set; }
        public Dictionary<Actions, Texture2D> SelectedDialogs { get; set; }
        private Level[] levels;
        private Level currentLevel;
        private UIContainer mainMenu;

        public MizJam1Game()
        {
            graphics = new GraphicsDeviceManager(this);
            textures = new Texture2D[4];
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            GameState = GameStates.MainMenu;
            Dialogs = new Dictionary<Actions, Texture2D>();
            SelectedDialogs = new Dictionary<Actions, Texture2D>();

        }

        public GameStates GameState { get; set; }

        protected override void Initialize()
        {
            GameState = GameStates.MainMenu;

            Window.AllowUserResizing = true;
            Window.Title = "MizJam1 Game";

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width == 1920 && GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height == 1080)
            {
                Window.IsBorderless = true;
                Window.Position = Point.Zero;
            }

            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            mapSpriteBatch = new SpriteBatch(GraphicsDevice);
            screenSpriteBatch = new SpriteBatch(GraphicsDevice);

            mizjamBigFont = Content.Load<SpriteFont>("Fonts/mizjam36");
            mizjamSmallFont = Content.Load<SpriteFont>("Fonts/mizjam24");
            whitePixel = Content.Load<Texture2D>("whitePixel");
            textures[0] = Content.Load<Texture2D>("colored_packed");
            textures[1] = Content.Load<Texture2D>("colored_transparent_packed");
            textures[2] = Content.Load<Texture2D>("monochrome_packed");
            textures[3] = Content.Load<Texture2D>("monochrome_transparent_packed");
            windowBorder = Content.Load<Texture2D>("Textures/WindowBorder");
            Dialogs[Actions.Move] = Content.Load<Texture2D>("Textures/Dialogs/MoveDialog");
            Dialogs[Actions.Attack] = Content.Load<Texture2D>("Textures/Dialogs/AttackDialog");
            Dialogs[Actions.Defend] = Content.Load<Texture2D>("Textures/Dialogs/DefendDialog");
            Dialogs[Actions.Heal] = Content.Load<Texture2D>("Textures/Dialogs/HealDialog");
            Dialogs[Actions.Reroll] = Content.Load<Texture2D>("Textures/Dialogs/RerollDialog");
            Dialogs[Actions.Wait] = Content.Load<Texture2D>("Textures/Dialogs/WaitDialog");
            Dialogs[Actions.Cancel] = Content.Load<Texture2D>("Textures/Dialogs/CancelDialog");
            SelectedDialogs[Actions.Move] = Content.Load<Texture2D>("Textures/Dialogs/MoveSelectedDialog");
            SelectedDialogs[Actions.Attack] = Content.Load<Texture2D>("Textures/Dialogs/AttackSelectedDialog");
            SelectedDialogs[Actions.Defend] = Content.Load<Texture2D>("Textures/Dialogs/DefendSelectedDialog");
            SelectedDialogs[Actions.Heal] = Content.Load<Texture2D>("Textures/Dialogs/HealSelectedDialog");
            SelectedDialogs[Actions.Reroll] = Content.Load<Texture2D>("Textures/Dialogs/RerollSelectedDialog");
            SelectedDialogs[Actions.Wait] = Content.Load<Texture2D>("Textures/Dialogs/WaitSelectedDialog");
            SelectedDialogs[Actions.Cancel] = Content.Load<Texture2D>("Textures/Dialogs/CancelSelectedDialog");
            TransparentTileSelect = Content.Load<Texture2D>("Textures/TransparentTileSelect");
            SelectedUnitBorder = Content.Load<Texture2D>("Textures/SelectedUnitBorder");

            string[] levelFiles = Directory.GetFiles("Content/Levels");
            levels = new Level[levelFiles.Length];
            for (int i = 0; i < levelFiles.Length; i++)
            {
                XDocument levelDoc = XDocument.Parse(File.ReadAllText(levelFiles[i]));
                levels[i] = new Level(levelDoc, this);
            }
            currentLevel = levels[0];
            camera = new Camera(1080, 1080, currentLevel.Width, currentLevel.Height);

            mainMenu = new UIContainer(Point.Zero, new Point(1920, 1080), true);
            UIMenu menu = new UIMenu(Point.Zero, new Point(1000, 600), true)
            {
                Vertical = true,
                SpaceBetweenChildren = 50
            };
            UILabel startGame = new UILabel("START GAME", mizjamBigFont, Global.Colors.Main1) { SelectedTextColor = Global.Colors.Accent1 };
            startGame.AddCommand(new StartGameCommand(this));
            UILabel options = new UILabel("OPTIONS", mizjamBigFont, Global.Colors.Main1) { SelectedTextColor = Global.Colors.Accent1 };
            options.AddCommand(new OpenOptionsCommand(this));
            UILabel exitGame = new UILabel("EXIT GAME", mizjamBigFont, Global.Colors.Main1) { SelectedTextColor = Global.Colors.Accent1 };
            exitGame.AddCommand(new ExitGameCommand(this));

            menu.AddChild(startGame);
            menu.AddChild(options);
            menu.AddChild(exitGame);

            mainMenu.AddChild(menu);
        }

        protected override void Update(GameTime gameTime)
        {
            MouseAdapter.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                GameState = GameStates.MainMenu;

            if (GameState == GameStates.MainMenu)
            {
                foreach (var child in ((UIMenu)mainMenu.Child).Children)
                {
                    if (child.Contains(MouseAdapter.Position))
                    {
                        child.Select();
                    }
                    else
                    {
                        child.Deselect();
                    }
                }

                if (MouseAdapter.ConsumeLeftClick)
                {
                    mainMenu.Execute();
                }
            }
            else if (GameState == GameStates.Playing)
            {
                currentLevel.Start();
            }
            else
            {
                if (MouseAdapter.ScrollUp)
                    camera.Zoom += 0.5f;
                if (MouseAdapter.ScrollDown)
                    camera.Zoom -= 0.5f;

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    camera.MoveCamera(new Vector2(-3, 0), true);
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    camera.MoveCamera(new Vector2(3, 0), true);
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    camera.MoveCamera(new Vector2(0, -3), true);
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    camera.MoveCamera(new Vector2(0, 3), true);
                if (MouseAdapter.Position.X > 420 && MouseAdapter.Position.X < 1500) //Mouse inside of game window
                {
                    currentLevel.MouseOver(camera.ScreenToWorld(MouseAdapter.Position.ToVector2()).ToPoint(), MouseAdapter.Position);
                    if (MouseAdapter.ConsumeLeftClick)
                    {
                        currentLevel.LeftClick(camera.ScreenToWorld(MouseAdapter.Position.ToVector2()).ToPoint(), MouseAdapter.Position);
                    }
                }
                else
                {
                    currentLevel.MouseOver(new Point(-10000, -10000), MouseAdapter.Position);
                }
            }

            currentLevel.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Global.Colors.Background2);
            screenSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            mapSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: camera.TransformationMatrix);

            if (GameState == GameStates.MainMenu)
            {
                mainMenu.Draw(screenSpriteBatch);
            }
            else
            {
                currentLevel.Draw(mapSpriteBatch, screenSpriteBatch, textures);
                screenSpriteBatch.Draw(whitePixel, new Rectangle(0, 0, 420, 1080), Global.Colors.Background2);
                screenSpriteBatch.Draw(windowBorder, new Rectangle(0, 0, 420, 1080), Global.Colors.Main1);
                screenSpriteBatch.Draw(whitePixel, new Rectangle(1500, 0, 420, 1080), Global.Colors.Background2);
                screenSpriteBatch.Draw(windowBorder, new Rectangle(1500, 0, 420, 1080), Global.Colors.Main1);
                Unit unit;
                if ((unit = currentLevel.SelectedUnit ?? currentLevel.MouseOverUnit) != null)
                {
                    screenSpriteBatch.DrawString(mizjamSmallFont, string.Format("NAME: {0}\nCLASS: {1}\nALLY: {2}", unit.Name.ToUpper(), unit.UnitClass.Name.ToUpper(), unit.Enemy ? "NO" : "YES"), new Vector2(1505 + 48, 5 + 48), Global.Colors.Main1);
                }
                Cell cell;
                if ((cell = currentLevel.MouseOverCell).ID != 0)
                {
                    CellProperties props = cell.Properties;
                    screenSpriteBatch.DrawString(mizjamSmallFont, string.Format("SOLID: {0}\nDIFF: {1}", props.IsSolid ? "YES" : "NO", props.Difficulty), new Vector2(5 + 48), Global.Colors.Main1);
                }
            }

            //Dice
            //mapSpriteBatch.Draw(textures[0], new Rectangle() { X = 16, Y = 16, Height = 16, Width = 16 }, new Rectangle() { X = 672, Y = 224, Height = 16, Width = 16 }, Color.White);

            //Draw debug world cursor position
            //screenSpriteBatch.DrawString(mainFont, camera.ScreenToWorld(MouseAdapter.Position.ToVector2()).ToString(), Vector2.Zero, Color.White);
            //Draw cursor
            screenSpriteBatch.Draw(textures[1], new Rectangle(MouseAdapter.Position, new Point(32, 32)), new Rectangle(560, 160, 16, 16), Color.White);
            base.Draw(gameTime);
            mapSpriteBatch.End();
            screenSpriteBatch.End();
        }

        public void Reset()
        {
            GameState = GameStates.MainMenu;
        }
    }
}
