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
    class Spelare
    {
        Texture2D _texture;
        Vector2 _position;
        Vector2 _hastighet;

        Rectangle sourceRect;
        Rectangle destRect = new Rectangle(300, 300, 92, 104);

        float animHast = 0.1f;
        int frames = 0;
        float tid;

        public Spelare(Texture2D texture, float X, float Y, float hastighetX, float hastighetY)
        {
            this._texture = texture;
            this._position.X = X;
            this._position.Y = Y;
            this._hastighet.X = hastighetX;
            this._hastighet.Y = hastighetY;
        }

        public void Update(GameWindow Window, GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.D))
            {
                _position.X += _hastighet.X;
                AnimationGå(gameTime);
            }
            else if (ks.IsKeyDown(Keys.A))
            {
                _position.X -= _hastighet.X;
                AnimationGå(gameTime);
            }
            else if (ks.IsKeyDown(Keys.W))
            {
                _position.Y -= _hastighet.Y;
                AnimationGå(gameTime);
            }
            else if (ks.IsKeyDown(Keys.S))
            {
                _position.Y += _hastighet.Y;
                AnimationGå(gameTime);
            }
            else
            {
                sourceRect = new Rectangle(0, 0, 92, 104);
            }
                
            
            destRect = new Rectangle((int)_position.X, (int)_position.Y, 92, 104);

            if (_position.X < 0)
                _position.X = 0;

            if (_position.X > Window.ClientBounds.Width - _texture.Width)
                _position.X = Window.ClientBounds.Width - _texture.Width;

            if (_position.Y < 0)
                _position.Y = 0;

            if (_position.Y > Window.ClientBounds.Height - _texture.Height)
                _position.Y = Window.ClientBounds.Height - _texture.Height;
        }
        
        private void AnimationGå(GameTime gameTime)
        {
            //öka förflutitid med 1 sekund
            tid += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (tid >= animHast)
            {
                //2 = 1 - antalet frames för animationen
                if (tid >= 2)
                {
                    frames = 0;
                }
                else
                {
                    frames++;
                }
                tid = 0;
            }

            sourceRect = new Rectangle(92 * frames, 0, 92, 104);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, destRect, sourceRect, Color.White);
        }
    }
}
