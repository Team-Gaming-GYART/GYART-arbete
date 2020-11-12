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
    class GameObject
    {
        protected Texture2D texture;
        protected Vector2 position;

        public GameObject(Texture2D texture, float X, float Y)
        {
            this.texture = texture;
            this.position.X = X;
            this.position.Y = Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public float X { get { return position.X; } }
        public float Y { get { return position.Y; } }
        public float Width { get { return texture.Width; } }
        public float Height { get { return texture.Height; } }
    }

    abstract class MovingObject : GameObject
    {
        protected Vector2 hastighet;
        public MovingObject(Texture2D texture, float X, float Y, float hastighetX, float hastighetY):base(texture, X, Y)
        {
            this.hastighet.X = hastighetX;
            this.hastighet.Y = hastighetY;
        }
    }

    class PhysicalObject : MovingObject
    {
        bool isAlive = true;

        public PhysicalObject(Texture2D texture, float Y, float X, float hastighetX, float hastighetY) : base(texture, X, Y, hastighetX, hastighetY)
        {
        }

        public bool KollaKollision(PhysicalObject annan)
        {
            Rectangle spelareRect = new Rectangle(Convert.ToInt32(X), Convert.ToInt32(Y), Convert.ToInt32(Width), Convert.ToInt32(Height));
            Rectangle annanRect = new Rectangle(Convert.ToInt32(annan.X), Convert.ToInt32(annan.Y), Convert.ToInt32(annan.Width), Convert.ToInt32(annan.Height));
            return spelareRect.Intersects(annanRect);
        }

        public bool IsAlive { get { return isAlive; } set { isAlive = value; } }
    }

    class Spelare : PhysicalObject
    {
        int pengar = 0;

        bool bana1;
        bool bana2;
        bool bana3;
        bool bana4;

        float cooldown = 0.1f;

        float förflutitid;
        int frames = 0;
        
        Texture2D rum;

        Rectangle bakgrund = new Rectangle(0, 0, 1920, 1080);
        Rectangle sourceRect;
        Rectangle destRect = new Rectangle(0, 0, 92, 104);

        public Spelare(Texture2D texture, Texture2D rum, float X, float Y, float hastighetX, float hastighetY, bool b1, bool b2, bool b3, bool b4) : base(texture, X, Y, hastighetX, hastighetY)
        {
            bana1 = b1;
            bana2 = b2;
            bana3 = b3;
            bana4 = b4;
        }

        public int Pengar { get { return pengar; } set { pengar = value; } }

        public void Update(GameWindow Window, ContentManager Content, GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            
            if (ks.IsKeyDown(Keys.D))
            {
                position.X += hastighet.X;
                texture = Content.Load<Texture2D>("Spelare/Gå/GoblinRight");
                AnimationGå(gameTime);
            }
            if (ks.IsKeyDown(Keys.A))
            {
                position.X -= hastighet.X;
                texture = Content.Load<Texture2D>("Spelare/Gå/GoblinLeft");
                AnimationGå(gameTime);
            }
            if (ks.IsKeyDown(Keys.W))
            {
                position.Y -= hastighet.Y;
                texture = Content.Load<Texture2D>("Spelare/Gå/GoblinUp");
                AnimationGå(gameTime);
            }
            if (ks.IsKeyDown(Keys.S))
            {
                position.Y += hastighet.Y;
                texture = Content.Load<Texture2D>("Spelare/Gå/GoblinDown");
                AnimationGå(gameTime);
            }
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                texture = Content.Load<Texture2D>("Spelare/Attack/GoblinAD");
            }
            
            destRect = new Rectangle((int)position.X, (int)position.Y, 92, 104);

            RumByte();
        }

        public void RumByte()
        {
            if (bana1 && Y < 0)
            {
                position = new Vector2(770, 940);
                bana1 = false;
                bana2 = true;
            }
            if (bana2 && Y < 0)
            {
                position = new Vector2(770, 940);
                bana2 = false;
                bana3 = true;
            }
            else if (bana2 && Y > 940)
            {
                position = new Vector2(770, 0);
                bana2 = false;
                bana1 = true;
            }

            if (bana3 && X < 0)
            {
                position = new Vector2(1800, 500);
                bana3 = false;
                bana4 = true;
            }
            else if (bana3 && Y > 940)
            {
                position = new Vector2(770, 0);
                bana3 = false;
                bana2 = true;
            }

            if (bana4 && X > 1920)
            {
                position = new Vector2(0, 500);
                bana4 = false;
                bana3 = true;
            }
        }

        private void AnimationGå(GameTime gameTime)
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

            sourceRect = new Rectangle(92 * frames, 0, 92, 104);
        }

        public void Draw(SpriteBatch spriteBatch, ContentManager Content)
        {
            if (bana1)
            {
                rum = Content.Load<Texture2D>("Rum/Bakgrund1");
                spriteBatch.Draw(rum, bakgrund, Color.White);
            }
            else if (bana2)
            {
                rum = Content.Load<Texture2D>("Rum/Bakgrund2");
                spriteBatch.Draw(rum, bakgrund, Color.White);
            }
            else if (bana3)
            {
                rum = Content.Load<Texture2D>("Rum/Bakgrund3");
                spriteBatch.Draw(rum, bakgrund, Color.White);
            }
            else if (bana4)
            {
                rum = Content.Load<Texture2D>("Rum/Bakgrund4");
                spriteBatch.Draw(rum, bakgrund, Color.White);
            }

            spriteBatch.Draw(texture, destRect, sourceRect, Color.White);
        }
    }
}
