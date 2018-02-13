using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Eratosthenes
{
    class Circle
    {
        private int diameter { get; set; }
        private int xLocation { get; set; }
        private int yLocation { get; set; }
        private int space { get; set; }
        private Texture2D texture { get; set; }
        private Color colour { get; set; }

        public Circle(int d, int xLoc, int s, Texture2D tex, Color c)
        {
            diameter = d;
            xLocation = xLoc;
            space = s;
            texture = tex;
            colour = c;
        }

        // Draws the circle and uses some algebra voodoo and way too much casting to place them correctly
        public void DrawCircle(SpriteBatch s, int x_zero, int y_zero)
        {
            s.Draw(texture, new Rectangle(x_zero + xLocation * space, (int)(y_zero - space * ((double)diameter / 2)), diameter * space, (int)(diameter * (double)space / 2)), colour);
        }
    }
}
