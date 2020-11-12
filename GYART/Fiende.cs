using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GYART
{
    class Fiende : PhysicalObject
    {
        public Fiende(Texture2D texture, float X, float Y) : base(texture, X, Y, 6f, 0.5f)
        {
        }

        public void Update(GameWindow Window)
        {
            position.X += hastighet.X;
            if (position.X > Window.ClientBounds.Width - texture.Width || position.X < 0)
            {
                hastighet.X *= -1;
            }
            position.Y += hastighet.Y;
        }
        
    }
}
