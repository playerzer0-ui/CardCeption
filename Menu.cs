using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CardCeption
{
    public class Menu
    {
        private Button playButton;
        private Button quitButton;
        private Texture2D menuTexture;
        private Texture2D playTexture;
        private Texture2D quitTexture;
        private Texture2D pixel;
        private MouseState oState;
        
        public Menu() { }

        public void Load(ContentManager Content, GraphicsDeviceManager _graphics)
        {
            menuTexture = Content.Load<Texture2D>("menu");
            playTexture = Content.Load<Texture2D>("buttons/play");
            quitTexture = Content.Load<Texture2D>("buttons/quit");

            playButton = new Button(playTexture, 640, 360);
            quitButton = new Button(quitTexture, 640, 490);

            pixel = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
        }

        public void Update(GameTime gt)
        {
            MouseState mState = Mouse.GetState();

            if(mState.LeftButton == ButtonState.Pressed && oState.LeftButton == ButtonState.Released && playButton.Rect.Contains(mState.Position))
            {
                Board.startGame = true;
                Game1.menuState = 2;
            }
            if(mState.LeftButton == ButtonState.Pressed && oState.LeftButton == ButtonState.Released && quitButton.Rect.Contains(mState.Position))
            {
                Game1.menuState = 0;
            }

            playButton.Update(gt);
            quitButton.Update(gt);

            oState = mState;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(menuTexture, new Vector2(0, 0), Color.White);
            playButton.Draw(_spriteBatch);
            quitButton.Draw(_spriteBatch);
            //_spriteBatch.Draw(pixel, playButton.Rect, new Color(255, 0, 0, 128));
        }
    }
}
