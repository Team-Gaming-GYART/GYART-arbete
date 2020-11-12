using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace GYART
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Spelare spelare;
        List<Fiende> fiender;
        
        List<Pengar> pengar;
        Texture2D pengahög;
        
        Font skrivText;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            pengar = new List<Pengar>();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            spelare = new Spelare(Content.Load<Texture2D>("Spelare/Gå/GoblinDown"), Content.Load<Texture2D>("Rum/Bakgrund1"), 100, 100, 5f, 5f, true, false, false, false);
            
            fiender = new List<Fiende>();
            Random slump = new Random();
            Texture2D tmpSprite = Content.Load<Texture2D>("Fiende/Skelett");
            for (int i = 0; i < 10; i++)
            {
                int rndX = slump.Next(0, Window.ClientBounds.Width - tmpSprite.Width);
                int rndY = slump.Next(0, Window.ClientBounds.Height / 2);
                Fiende temp = new Fiende(tmpSprite, rndX, rndY);
                fiender.Add(temp);
            }

            pengahög = Content.Load<Texture2D>("Drops/coinTossAnim3");


            skrivText = new Font(Content.Load<SpriteFont>("myFont"));

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            spelare.Update(Window, Content, gameTime);
            
            foreach (Fiende f in fiender.ToList())
            {
                if (f.IsAlive)
                {
                    if (f.KollaKollision(spelare))
                    {
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            fiender.Remove(f);
                            
                            pengar.Add(new Pengar(pengahög, 100, 100, gameTime));
                        }
                        
                    }
                    f.Update(Window);
                }
                else
                {
                    fiender.Remove(f);
                }
            }
            
            foreach (Pengar p in pengar.ToList())
            {
                if (p.IsAlive)
                {
                    p.Update(gameTime);

                    if (p.KollaKollision(spelare))
                    {
                        pengar.Remove(p);
                        spelare.Pengar++;
                    }
                }
                else
                {
                    pengar.Remove(p);
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            
            spelare.Draw(spriteBatch, Content);

            foreach (Fiende f in fiender)
            {
                f.Draw(spriteBatch);
            }

            foreach (Pengar p in pengar)
            {
                p.Draw(spriteBatch);
            }
            
            skrivText.SkrivUt($"Pengar: {spelare.Pengar}", spriteBatch, 0, 0);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
