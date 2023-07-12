using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;

namespace CardCeption
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private MouseState oState = Mouse.GetState();
        private SpriteFont font;

        public static int menuState = 1;

        Board board = new Board();
        Menu menu = new Menu();
        Pause pause = new Pause();
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            board.Load(Content, _graphics);
            menu.Load(Content, _graphics);
            pause.Load(Content, _graphics);
            font = Content.Load<SpriteFont>("file");
            
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            switch (menuState)
            {
                case 0:
                    Exit();
                    break;
                case 1:
                    menu.Update(gameTime);
                    break;
                case 2:
                    board.Update(gameTime);
                    break;
                case 3:
                    pause.Update(gameTime);
                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGreen);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            switch (menuState)
            {
                case 1:
                    menu.Draw(_spriteBatch);
                    break;
                case 2:
                    board.Draw(_spriteBatch);
                    break;
                case 3:
                    pause.Draw(_spriteBatch);
                    break;
            }
            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}