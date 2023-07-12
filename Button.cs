using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CardCeption
{
    public class Button
    {
        private SpriteAnimation anim;
        private Vector2 pos;
        private int state = 0;
        private Rectangle rect;

        public Rectangle Rect { get => rect; set => rect = value; }

        public Button(Texture2D texture, int x, int y)
        {
            anim = new SpriteAnimation(texture, 2, 3);
            anim.setFrame(state);
            pos = new Vector2(x, y);
            anim.Origin = new Vector2(texture.Width / 4, texture.Height / 2);
            rect = new Rectangle(x, y, 500, 100);
            rect.Offset(-250, -50);
        }

        public void Update(GameTime gt)
        {
            MouseState mState = Mouse.GetState();

            if (rect.Contains(mState.Position))
            {
                state = 1;
            }
            else
            {
                state = 0;
            }
            
            anim.Position = pos;
            anim.setFrame(state);
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            anim.Draw(_spriteBatch);
        }

        
    }
}
