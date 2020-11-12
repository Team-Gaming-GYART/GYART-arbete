using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace GYART
{
    class Pengar : PhysicalObject
    {
        double tidInnanDöd;

        float cooldown = 0.1f;

        float förflutitid;
        int frames = 0;

        Rectangle destRect = new Rectangle(0,0,100,100);
        Rectangle sourceRect;

        public Pengar(Texture2D texture, float X, float Y, GameTime gameTime) : base(texture, X, Y, 0, 0)
        {
            tidInnanDöd = gameTime.TotalGameTime.TotalMilliseconds + 10000;
        }

        public void Update(GameTime gameTime)
        {
            if (tidInnanDöd < gameTime.TotalGameTime.TotalMilliseconds)
            {
                IsAlive = false;
            }

            destRect = new Rectangle(100, 100, 100, 100);
        }

        private void AnimationCoin(GameTime gameTime)
        {
            //öka förflutitid med 1 sekund
            förflutitid += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (förflutitid >= cooldown)
            {
                //2 = 1 - antalet frames för animationen
                if (frames >= 2)
                {
                    frames = 0;
                }
                else
                {
                    frames++;
                }
                förflutitid = 0;
            }

            sourceRect = new Rectangle(100 * frames, 0, 100, 100);
        }

        public void Draw(SpriteBatch spriteBatch, ContentManager Content)
        {
            spriteBatch.Draw(texture, destRect, sourceRect, Color.White);
        }
    }
}
