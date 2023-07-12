using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CardCeption
{
    public class Card
    {
        private bool flag = false;
        private bool rev = true;
        private SpriteAnimation anim;
        private SpriteAnimation vanishAnim;
        private Texture2D texture;
        private Rectangle rect;
        private int id;
        private bool isVisible = true;
        private bool isPressed = false;

        public static Texture2D vanishTexture;

        public Card(int id, Texture2D texture, int width, int fps)
        {
            this.id = id;
            this.texture = texture;
            anim = new SpriteAnimation(texture, width, fps);
            vanishAnim = new SpriteAnimation(vanishTexture, width, fps);
            anim.setFrame(8);

            anim.Origin = new Vector2(140 / 2, 160 / 2);
            vanishAnim.Origin = new Vector2(140 / 2, 160 / 2);
            Flip();
        }

        public SpriteAnimation Anim { get => anim; set => anim = value; }
        public Rectangle Rect { get => rect; set => rect = value; }
        public bool Flag { get => flag; set => flag = value; }
        public int Id { get => id; set => id = value; }
        public Texture2D Texture { get => texture; set => texture = value; }
        public bool IsVisible { get => isVisible; set => isVisible = value; }
        public bool IsPressed { get => isPressed; set => isPressed = value; }
        public SpriteAnimation VanishAnim { get => vanishAnim; set => vanishAnim = value; }

        public void Update(GameTime gt)
        {

            if (flag && isVisible)
            {
                anim.UpdateOnce(gt);
            }

            if (!isVisible)
            {
                anim = vanishAnim;
                anim.UpdateOnce(gt);
            }

            rect = new Rectangle((int)anim.Position.X + 8, (int)anim.Position.Y + 8, 105, 142);
            rect.Offset(-70, -80);
        }

        public void Flip()
        {
            flag = true;
            rev = !rev;
            anim.IsReversed = rev;
        }
    }
}
