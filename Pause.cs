using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Formats.Asn1.AsnWriter;

namespace CardCeption
{
    public class Pause
    {
        private Button homeButton;
        private Button resumeButton;
        private Button retryButton;
        private Texture2D pixel;
        private Texture2D resumeTexture;
        private Texture2D homeTexture;
        private Texture2D retryTexture;
        private MouseState oState;
        private SpriteFont font;
        private string scoreText;
        private Vector2 textSize;
        private Vector2 textPosition;
        public Pause() { }

        public void Load(ContentManager Content, GraphicsDeviceManager _graphics)
        {
            homeTexture = Content.Load<Texture2D>("buttons/home");
            resumeTexture = Content.Load<Texture2D>("buttons/resume");
            retryTexture = Content.Load<Texture2D>("buttons/retry");
            pixel = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            homeButton = new Button(homeTexture, 640, 480);
            resumeButton = new Button(resumeTexture, 640, 360);
            retryButton = new Button(retryTexture, 640, 360);

            font = Content.Load<SpriteFont>("file");
            scoreText = $"={Board.score}=";
            textSize = font.MeasureString(scoreText);
            textPosition = new Vector2(640, 250) - textSize / 2f;
        }

        public void Update(GameTime gt) 
        {
            MouseState mState = Mouse.GetState();
            scoreText = $"={Board.score}=";
            textSize = font.MeasureString(scoreText);
            textPosition = new Vector2(640, 250) - textSize / 2f;

            if (mState.LeftButton == ButtonState.Pressed && oState.LeftButton == ButtonState.Released && homeButton.Rect.Contains(mState.Position))
            {
                Reset();
                Game1.menuState = 1;
            }
            if (mState.LeftButton == ButtonState.Pressed && oState.LeftButton == ButtonState.Released && resumeButton.Rect.Contains(mState.Position) && Board.startGame)
            {
                Game1.menuState = 2;
            }
            if (mState.LeftButton == ButtonState.Pressed && oState.LeftButton == ButtonState.Released && retryButton.Rect.Contains(mState.Position) && !Board.startGame)
            {
                Reset();
                Game1.menuState = 2;
            }

            homeButton.Update(gt);
            resumeButton.Update(gt);
            retryButton.Update(gt);

            oState = mState;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            if (Board.startGame)
            {
                resumeButton.Draw(_spriteBatch);
            }
            else
            {
                retryButton.Draw(_spriteBatch);
            }
            homeButton.Draw(_spriteBatch);
            _spriteBatch.DrawString(font, scoreText, textPosition, Color.White);
        }

        public void Reset()
        {
            Board.score = 0;
            Board.indexSpike = 0;
            Board.shuffling = false;
            Board.begin = false;
            Board.pos = -1;
            Board.showScore = false;
            Board.doneIncrease = false;
            Board.wrong = false;
            Lives.Restore();
        }

    }
}
