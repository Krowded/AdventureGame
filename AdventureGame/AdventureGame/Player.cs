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
        /// <summary>
        /// The coordinates of the middle of the player sprite
        /// </summary>
        public Vector2 Position;
        public Vector2 Direction;
        private bool active = false;
        public float Scale { get; set; }
        public float RoomScale { get; set; }
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
        public bool Running { get; set; }
        public bool MovingLeft { get; set; }
        public float MoveSpeed { get; set; }
        public float WalkSpeed { get; set; }
        public float RunSpeed { get; set; }
        public int Width
        {
            get { return PlayerAnimation.FrameWidth; }
        }
        public int Height
        {
            get { return PlayerAnimation.FrameHeight; }
        }

        public Player() { }

        public Player(Vector2 startingPositionOnBackground)
            : this()
        {
            this.Position.X = startingPositionOnBackground.X;
            this.Position.Y = startingPositionOnBackground.Y;
        }

        public void Initialize(Animation animation, Vector2 position)
        {
            PlayerAnimation = animation;
            Position = position;
            Active = true;
        }

        public void Update(GameTime gameTime)
        {
            SetPlayerSpriteDirection();
            UpdateMoveSpeed();
            PlayerAnimation.Position = Position;
            PlayerAnimation.Update(gameTime);
        }

        public void ParseTextFile(string FileName)
        {
            StreamReader file = new StreamReader(FileName);
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] words = line.Split(':');
                    switch (words[0])
                    {
                        case "PlayerTexture":
                            this.PlayerTexture = words[1];
                            break;
                        default:
                            {
                                throw new InvalidOperationException("Text file error in " + FileName);
                            }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (MovingLeft)
            {
                PlayerAnimation.Draw(spriteBatch, Scale, SpriteEffects.FlipHorizontally);
            }
            else
            {
                PlayerAnimation.Draw(spriteBatch, Scale, SpriteEffects.None);
            }
        }

        /// <summary>
        /// Moves player until within a 10 pixel square of the target
        /// </summary>
        /// <param name="point">Target point</param>
       

        public bool MoveToPoint(bool startMoving, Vector2 point)
        {
            bool targetReached = ((Math.Abs(this.Position.X - point.X) < 10) &&
                                  (Math.Abs(this.Position.Y - point.Y) < 10));

            if (startMoving && !targetReached)
            {
                MoveTowardsPoint(point);
            }
            else
            {
                this.Direction = Vector2.Zero;
                this.Running = false;
            }

            return targetReached;
        }

        /// <summary>
        /// Move player towards target point
        /// </summary>
        /// <param name="point">Target point</param>
        private void MoveTowardsPoint(Vector2 point)
        {
            this.Direction.X = point.X - this.Position.X;
            this.Direction.Y = point.Y - this.Position.Y;
            this.Direction.Normalize();

            this.Position += this.Direction * this.MoveSpeed;
        }

        /// <summary>
        /// Check for player movement direction and flip sprite
        /// </summary>
        private void SetPlayerSpriteDirection()
        {
            if (this.Direction.X < 0)
            {
                this.MovingLeft = true;
            }
            else if (this.Direction.X > 0)
            {
                this.MovingLeft = false;
            }
        }

        /// <summary>
        /// Scale player sprite by it's Y relative to the background
        /// </summary>
        public void ScalePlayerSprite(Vector2 backgroundVector, int backgroundHeight, float viewportWidth, float smallestScale, float naturalScreenWidth)
        {
            this.Scale = this.RoomScale*(((this.Position.Y - backgroundVector.Y) / backgroundHeight + smallestScale)
                * (viewportWidth / naturalScreenWidth));
        }

        /// <summary>
        /// Keeps player from running off screen
        /// </summary>
        public void ClampPlayer(int viewportWidth, int viewportHeight)
        {
            this.Position.X = MathHelper.Clamp(Position.X, this.Scale * this.Width / 2, viewportWidth - this.Width * this.Scale / 2);
            this.Position.Y = MathHelper.Clamp(Position.Y, this.Scale * this.Height / 2, viewportHeight - this.Height * this.Scale / 2);

        }

        /// <summary>
        /// Adjusts MoveSpeed
        /// </summary>
        private void UpdateMoveSpeed()
        {
            if (this.Running)
            {
                this.MoveSpeed = this.RunSpeed;
            }
            else
            {
                this.MoveSpeed = this.WalkSpeed;
            }
        }
    }
}
