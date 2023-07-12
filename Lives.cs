using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CardCeption
{
    public class Lives
    {
        private static int lifepoints;
        private static int maxLifepoints;
        private Vector2 vector;
        private Texture2D heartTexture;
        public Lives(Texture2D heartTexture) 
        {
            lifepoints = 4;
            maxLifepoints = 4;
            this.heartTexture = heartTexture;
        }

        public int Lifepoints { get => lifepoints; set => lifepoints = value; }

        public Boolean Die()
        {
            return lifepoints <= 0;
        }

        public static void Restore()
        {
            lifepoints = maxLifepoints;
        }

        public void Lose()
        {
            lifepoints = lifepoints - 1;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            vector = new Vector2(1240, 40); 

            for (int i = 0; i < lifepoints; i++)
            {
                _spriteBatch.Draw(heartTexture, new Vector2(vector.X - 50, vector.Y - 50), Color.White);
                vector.X -= 60;
            }
        }
    }
}
