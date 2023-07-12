using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CardCeption
{
    enum Difficulty
    {
        Init=6, //6
        Start=10, //10
        Easy=20, //20
        Medium=28, //28
        Hard=36 //36
    }

    public class Board
    {
        //cards spacing for drawing
        private int distance;
        private int column;

        //textures
        private int totalCards = 37;
        public static List<Card> cards = new List<Card>();
        private Texture2D texture;
        private Texture2D pauseTexture;
        private Rectangle pauseRect;
        private Texture2D pixel;
        private Texture2D heartTexture;
        private MouseState oState = Mouse.GetState();

        //timers
        private float timer = 2f;
        private float maxTime = 2f;
        private float successTimer = 6f;
        private float maxSuccessTimer = 6f;
        private float seeCard = 2f;
        private float maxSeeCard = 2f;
        private int posWrong = -1;
        private float scoreTimer = 4f;
        private float maxScoreTimer = 4f;

        //class
        private Deck deck = new Deck();
        private Lives lives;

        //difficulty spike
        private int[] spike = {
            (int)Difficulty.Init, 
            (int)Difficulty.Start, (int)Difficulty.Start,
            (int)Difficulty.Easy, (int)Difficulty.Medium,
            (int)Difficulty.Medium,(int)Difficulty.Hard
        };
        public static int indexSpike = 0;

        //card game
        public static bool begin = false;
        public static bool shuffling = false;
        public static int score = 0;
        public static bool showScore = false;
        public static bool startGame = true;
        public static bool doneIncrease = false;
        public static bool wrong = false;
        public Card card;
        public static int pos = -1;
        private string scoreText;
        private SpriteFont font;
        private Vector2 textSize;
        private Vector2 textPosition;
        private float time;
        public Board() { }

        public void Load(ContentManager Content, GraphicsDeviceManager _graphics)
        {
            //load ALL cards
            Card.vanishTexture = Content.Load<Texture2D>("cards/vanish");
            for(int i = 0; i < totalCards; i++)
            {
                texture = Content.Load<Texture2D>($"cards/card_reveal/{i + 1}");
                cards.Add(new Card(i + 1, texture, 9, 15));
            }
            pixel = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            //Load hearts
            heartTexture = Content.Load<Texture2D>("heart");
            lives = new Lives(heartTexture);

            //load font
            font = Content.Load<SpriteFont>("file");

            //load pause texture
            pauseTexture = Content.Load<Texture2D>("buttons/pause");
            pauseRect = new Rectangle(0, 0, 50, 50);
        }

        public void Update(GameTime gt)
        {
            time = (float)gt.ElapsedGameTime.TotalSeconds;

            MouseState mState = Mouse.GetState();

            scoreText = $"={score}=";
            textSize = font.MeasureString(scoreText);
            textPosition = new Vector2(640, 360) - textSize / 2f;
            //shuffle deck
            if (!shuffling)
            {
                if(indexSpike < spike.Length)
                {
                    StartDeck(spike[indexSpike++]);
                }
                else
                {
                    StartDeck((int)Difficulty.Hard);
                }
                shuffling = true;
            }
            //start the game
            if (!begin)
            {
                RevealOnceAll(gt);
                startGame = true;
            }
            //in middle of game
            else if(begin && startGame)
            {
                for (int i = 0; i < deck.Arr.Length; i++)
                {
                    if (mState.LeftButton == ButtonState.Pressed && oState.LeftButton == ButtonState.Released && deck.Arr[i].Rect.Contains(mState.Position) && !deck.Arr[i].IsPressed && !wrong && deck.Arr[i].IsVisible)
                    {
                        deck.Arr[i].Flip();
                        if(card == null)
                        {
                            card = deck.Arr[i];
                            pos = i;
                            deck.Arr[i].IsPressed = true;
                        }
                        else
                        {
                            if (card.Id == deck.Arr[i].Id)
                            {
                                deck.Arr[i].IsVisible = false;
                                deck.Arr[pos].IsVisible = false;
                                card = null;
                                deck.Count--;
                            }
                            else
                            {
                                posWrong = i;
                                wrong = true;
                                lives.Lose();
                            }
                        }

                    }
                    deck.Arr[i].Update(gt);
                }
                //wrong pair
                if (wrong)
                {
                    RevealOnce(gt, posWrong);
                }
                //win
                if(deck.Count <= 0)
                {
                    IncreaseScore(gt);
                    Success(gt);
                }
            }

            if(mState.LeftButton == ButtonState.Pressed && oState.LeftButton == ButtonState.Released && pauseRect.Contains(mState.Position))
            {
                Game1.menuState = 3;
            }

            oState = mState;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            int length = deck.Arr.Length;

            switch (length)
            {
                case (int)Difficulty.Init:
                    distance = 480;
                    column = 250;
                    DrawToScreen(_spriteBatch, distance);
                    break;

                case (int)Difficulty.Start:
                    distance = 340;
                    column = 250;
                    DrawToScreen(_spriteBatch, distance);
                    break;

                case (int)Difficulty.Easy:
                    distance = 340;
                    column = 100;
                    DrawToScreen(_spriteBatch, distance);
                    break;

                case (int)Difficulty.Medium:
                    distance = 220;
                    column = 100;
                    DrawToScreen(_spriteBatch, distance);
                    break;

                case (int)Difficulty.Hard:
                    distance = 85;
                    column = 100;
                    DrawToScreen(_spriteBatch, distance);
                    break;
            }
            lives.Draw(_spriteBatch);
            _spriteBatch.Draw(pauseTexture, pauseRect, Color.White);

            if(showScore) _spriteBatch.DrawString(font, scoreText, textPosition, Color.White);
            //_spriteBatch.DrawString(font, "pos: " + pos, new Vector2(100, 100), Color.White);

        }

        /// <summary>
        /// Reveals all cards, flip them all.
        /// </summary>
        /// <param name="gt">gametime</param>
        public void RevealOnceAll(GameTime gt)
        {
            //float time = (float)gt.ElapsedGameTime.TotalSeconds;

            timer -= time;

            if(timer < 0)
            {
                for (int i = 0; i < deck.Arr.Length; i++)
                {
                    deck.Arr[i].Flip();
                    deck.Arr[i].Update(gt);
                }

                begin = true;
                timer = maxTime;
            }
        }

        /// <summary>
        /// Reveals a card, this is the wrong pair method.
        /// </summary>
        /// <param name="gt">gametime</param>
        /// <param name="i">position of the card in the wrong pair</param>
        public void RevealOnce(GameTime gt, int i)
        {
            //float time = (float)gt.ElapsedGameTime.TotalSeconds;
            seeCard -= time;

            if (seeCard < 0)
            {
                deck.Arr[pos].Flip();
                deck.Arr[i].Flip();

                deck.Arr[pos].Update(gt);
                deck.Arr[i].Update(gt);
                deck.Arr[i].IsPressed = false;
                deck.Arr[pos].IsPressed = false;

                card = null;
                pos = -1;
                wrong = false;
                seeCard = maxSeeCard;
                if (lives.Die())
                {
                    startGame = false;
                    Game1.menuState = 3;
                }
            }
        }

        /// <summary>
        /// Starts the deck.
        /// </summary>
        /// <param name="diff">The difficulty</param>
        /// <returns>A Boolean.</returns>
        public Boolean StartDeck(int diff)
        {
            cards = Deck.Shuffle(cards);
            deck.SetDeck(diff);
            deck.AddDeck(cards);
            return true;
        }

        /// <summary>
        /// all pairs found and this method activates
        /// </summary>
        /// <param name="gt">gametime</param>
        public void Success(GameTime gt)
        {
            //float time = (float)gt.ElapsedGameTime.TotalSeconds;

            successTimer -= time;
            showScore = true;

            if (successTimer < 0)
            {
                shuffling = false;
                begin = false;
                successTimer = maxSuccessTimer;
                Lives.Restore();
                showScore = false;
                doneIncrease = false;
            }
        }

        /// <summary>
        /// Increases the score.
        /// </summary>
        /// <param name="gt">gametime</param>
        public void IncreaseScore(GameTime gt)
        {
            //float time = (float)gt.ElapsedGameTime.TotalSeconds;

            if (!doneIncrease)
            {
                scoreTimer -= time;

                if (scoreTimer < 0)
                {
                    score++;
                    scoreTimer = maxScoreTimer;
                    doneIncrease = true;
                }
            }
        }

        /// <summary>
        /// Draws the to screen.
        /// </summary>
        /// <param name="_spriteBatch">The _spriteBatch.</param>
        /// <param name="dist">The max distance</param>
        public void DrawToScreen(SpriteBatch _spriteBatch, int dist)
        {
            int div = 2;
            int lineBreak;
            if(deck.Arr.Length > 10)
            {
                div = 4;
            }
            lineBreak = deck.Arr.Length / div;

            for (int i = 0; i < deck.Arr.Length; i++)
            {
                deck.Arr[i].Anim.Position = new Vector2(distance, column);
                deck.Arr[i].VanishAnim.Position = new Vector2(distance, column);
                deck.Arr[i].Anim.Draw(_spriteBatch);

                //if (begin)
                //{
                //    _spriteBatch.Draw(pixel, deck.Arr[i].Rect, new Color(255, 0, 0, 128));
                //}

                distance += 140;

                if ((i + 1) % lineBreak == 0 && i > 0)
                {
                    column += 180;
                    distance = dist;
                }
            }
        }
    }
}
