using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MizJam1.Levels;
using MizJam1.Rendering;
using MizJam1.UIComponents;
using MizJam1.UIComponents.Commands;
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
            DefensePhase
        }

        private GraphicsDeviceManager graphics;
        private SpriteBatch mapSpriteBatch;
        private SpriteBatch screenSpriteBatch;
        private Camera camera;

        private SpriteFont mainFont;
        private SpriteFont mizjamFont;
        private Texture2D[] textures;
        private Level[] levels;
        private UIContainer mainMenu;

        public MizJam1Game()
        {
            graphics = new GraphicsDeviceManager(this);
            textures = new Texture2D[4];
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            GameState = GameStates.MainMenu;
        }

        public GameStates GameState { get; set; }

        protected override void Initialize()
        {
            camera = new Camera
            {
                Zoom = 3f
            };

            GameState = GameStates.MainMenu;

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            mapSpriteBatch = new SpriteBatch(GraphicsDevice);
            screenSpriteBatch = new SpriteBatch(GraphicsDevice);

            mainFont = Content.Load<SpriteFont>("Fonts/mainFont");
            mizjamFont = Content.Load<SpriteFont>("Fonts/mizjam");
            textures[0] = Content.Load<Texture2D>("colored_packed");
            textures[1] = Content.Load<Texture2D>("colored_transparent_packed");
            textures[2] = Content.Load<Texture2D>("monochrome_packed");
            textures[3] = Content.Load<Texture2D>("monochrome_transparent_packed");



            string[] levelFiles = Directory.GetFiles("Content/Levels");
            levels = new Level[levelFiles.Length];
            for (int i = 0; i < levelFiles.Length; i++)
            {
                XDocument levelDoc = XDocument.Parse(File.ReadAllText(levelFiles[i]));
                levels[i] = new Level(levelDoc);
            }


            mainMenu = new UIContainer(Point.Zero, new Point(1920, 1080), true);
            UIMenu menu = new UIMenu(Point.Zero, new Point(1000, 600), true);
            menu.Vertical = true;
            menu.SpaceBetweenChildren = 50;
            UILabel startGame = new UILabel("START GAME", mizjamFont, Global.Colors.Main1) { SelectedTextColor = Global.Colors.Accent1 };
            startGame.AddCommand(new StartGameCommand(this));
            UILabel options = new UILabel("OPTIONS", mizjamFont, Global.Colors.Main1) { SelectedTextColor = Global.Colors.Accent1 };
            options.AddCommand(new OpenOptionsCommand(this));
            UILabel exitGame = new UILabel("EXIT GAME", mizjamFont, Global.Colors.Main1) { SelectedTextColor = Global.Colors.Accent1 };
            exitGame.AddCommand(new ExitGameCommand(this));

            menu.AddChild(startGame);
            menu.AddChild(options);
            menu.AddChild(exitGame);

            mainMenu.AddChild(menu);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                GameState = GameStates.MainMenu;

            if (GameState == GameStates.MainMenu)
            {
                MouseState mouseState = Mouse.GetState();
                foreach (var child in ((UIMenu)mainMenu.Child).Children)
                {
                    if (child.Contains(mouseState.Position))
                    {
                        child.Select();
                    }
                    else
                    {
                        child.Deselect();
                    }
                }

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    mainMenu.Execute();
                }
            }
            else
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    camera.Zoom = 4f;
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    camera.MoveCamera(new Vector2(-3, 0), true);
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    camera.MoveCamera(new Vector2(3, 0), true);
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    camera.MoveCamera(new Vector2(0, -3), true);
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    camera.MoveCamera(new Vector2(0, 3), true);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Global.Colors.Main3);
            screenSpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            mapSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: camera.TransformationMatrix);

            //Draw debug world cursor position
            screenSpriteBatch.DrawString(mainFont, camera.ScreenToWorld(Mouse.GetState().Position.ToVector2()).ToString(), Vector2.Zero, Color.White);
            if (GameState == GameStates.MainMenu)
            {
                mainMenu.Draw(screenSpriteBatch);
            }
            else
            {
                levels[0].Draw(mapSpriteBatch, textures);
            }
            //Test font
            //screenSpriteBatch.DrawString(mizjamFont, "TESTING FONT", Vector2.One * 16, new Color(0xffb8c6cf));
            //Dice
            //mapSpriteBatch.Draw(textures[0], new Rectangle() { X = 16, Y = 16, Height = 16, Width = 16 }, new Rectangle() { X = 672, Y = 224, Height = 16, Width = 16 }, Color.White);

            //Draw cursor
            screenSpriteBatch.Draw(textures[1], new Rectangle(Mouse.GetState().Position, new Point(32, 32)), new Rectangle(560, 160, 16, 16), Color.White);
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
