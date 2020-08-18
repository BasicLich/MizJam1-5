using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MizJam1.Levels;
using System.Collections.Generic;

namespace MizJam1
{
    public class MizJam1Game : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _mapSpriteBatch;
        private SpriteBatch _screenSpriteBatch;
        private Camera camera;

        private SpriteFont mainFont;
        private SpriteFont mizjamFont;
        private Texture2D[] textures;
        private Level levelTest;

        public MizJam1Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            textures = new Texture2D[4];
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            camera = new Camera();
            camera.Zoom = 2f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _mapSpriteBatch = new SpriteBatch(GraphicsDevice);
            _screenSpriteBatch = new SpriteBatch(GraphicsDevice);

            mainFont = Content.Load<SpriteFont>("mainFont");
            mizjamFont = Content.Load<SpriteFont>("Fonts/mizjam");
            textures[0] = Content.Load<Texture2D>("colored_packed");
            textures[1] = Content.Load<Texture2D>("colored_transparent_packed");
            textures[2] = Content.Load<Texture2D>("monochrome_packed");
            textures[3] = Content.Load<Texture2D>("monochrome_transparent_packed");
            levelTest = new Level(@"1,2684354569,2684354569,2684354569,2684354569,2684354569,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
1,1,1,1,6,1,1,1,1,1,1,1,1,1,1,8,1,1,1,1,
1,1,1,1,1,1,1,1,1,1,1,1,1,1,10,2684354569,2684354569,2684354569,2684354569,2684354569,
1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,1,1,1,1,1,
1,1,1,1,1,1,1,1,6,1,1,1,1,1,9,1,1,1,1,1,
1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,1,1,1,1,1,
1,1,1,1,1,8,1,1,1,1,1,1,1,1,9,1,1,1,1,1,
1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,1,1,1,1,1,
1,6,1,1,1,1,1,1,1,1,1,1,6,1,9,1,1,8,1,1,
1,1,1,1,1,1,1,6,1,1,1,1,1,1,3221225481,1,1,1,1,1,
1,1,1,1,1,1,1,6,1,1,1,1,1,1,3221225481,1,1,1,1,1,
1,1,1,1,1,1,1,1,1,1,1,1,1,1,3221225481,1,1,1,1,1,
1,1,6,1,1,1,1,1,1,1,1,1,1,1,3221225481,1,6,1,1,1,
1,1,1,1,1,1,1,1,1,1,1,1,1,1,3221225481,1,1,1,1,1,
1,1,1,1,1,1,1,1,1,6,1,1,1,1,3221225481,1,1,1,1,1,
1,1,1,1,1,1,1,1,1,1,1,1,1,1,3221225481,1,1,1,1,1,
1,1,1,1,1,1,1,1,1,1,1,8,1,1,3221225481,1,1,1,1,1,
1,1,1,1,1,1,1,1,1,1,1,1,1,1,3221225481,1,1,1,1,1,
1,1,1,1,1,1,1,1,1,1,1,1,1,1,3221225481,1,1,1,1,1");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                camera.Zoom = 4f;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _screenSpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            _mapSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: camera.TransformationMatrix);

            _screenSpriteBatch.DrawString(mainFont, camera.ScreenToWorld(Mouse.GetState().Position.ToVector2()).ToString(), Vector2.Zero, Color.White);
            levelTest.Draw(_mapSpriteBatch, textures);
            _screenSpriteBatch.DrawString(mizjamFont, "TESTING FONT", Vector2.One * 16, Color.White);
            _mapSpriteBatch.Draw(textures[0], new Rectangle() { X = 16, Y = 16, Height = 16, Width = 16 }, new Rectangle() { X = 672, Y = 224, Height = 16, Width = 16 }, Color.White);

            base.Draw(gameTime);
            _mapSpriteBatch.End();
            _screenSpriteBatch.End();
        }
    }
}
