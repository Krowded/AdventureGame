using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace AdventureGame
{
    class Player
    {
        public Animation PlayerAnimation;
        public Vector2 Position;
        private bool active = false;
        public float Scale { get; set; }
        public string PlayerTexture;


        public bool Active 
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }
        public bool movingLeft = true;

        public int Width
        {
            get { return PlayerAnimation.FrameWidth; }
        }

        public int Height
        {
            get { return PlayerAnimation.FrameHeight; }
        }

        public Player() { }

        public Player(int startingx, int startingy)
            : this()
        {
            this.Position.X = startingx;
            this.Position.Y = startingy;
        }

        public void Initialize(Animation animation, Vector2 position)
        {
            PlayerAnimation = animation;
            Position = position;
            Active = true;
        }

        public void Update(GameTime gameTime)
        {
            PlayerAnimation.Position = Position;
            PlayerAnimation.Update(gameTime);
        }


        public void ParseTextFile(string FileName)
        {
            StreamReader file = new StreamReader(FileName);
            string line = file.ReadLine();
            PlayerTexture = line;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (movingLeft)
            {
                PlayerAnimation.Draw(spriteBatch, Scale, SpriteEffects.None);
            }
            else
            {
                PlayerAnimation.Draw(spriteBatch, Scale, SpriteEffects.FlipHorizontally);
            }
        }
    }
}
