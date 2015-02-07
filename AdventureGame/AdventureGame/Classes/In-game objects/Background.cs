using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace AdventureGame
{
    internal class Background
    {
        public Texture2D BGTexture;
        public Vector2 Position;
        public int Width;
        public int Height;

        public Background() { }

        public Background(Texture2D texture, Vector2 position, float scale)
        {
            BGTexture = texture;
            Position = position;
            Width = (int)(BGTexture.Width*scale);
            Height = (int)(BGTexture.Height*scale);
        }
    }
}
