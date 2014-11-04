using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame
{
    class Player
    {
        //public Texture2D PlayerTexture;
        public Animation PlayerAnimation;
        public Vector2 Position;
        public bool Active;
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

        public void Draw(SpriteBatch spriteBatch, float scale)
        {
            if (movingLeft)
            {
                PlayerAnimation.Draw(spriteBatch, scale, SpriteEffects.None);
                //spriteBatch.Draw(PlayerTexture, Position, null, Color.White, 0f, Vector2.Zero, scale,
                //    SpriteEffects.None, 0f);
            }
            else
            {
                PlayerAnimation.Draw(spriteBatch, scale, SpriteEffects.FlipHorizontally);
            }
        }
    }
}
