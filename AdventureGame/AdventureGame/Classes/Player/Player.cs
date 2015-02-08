using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace AdventureGame
{
    internal class Player
    {
        private string Identifier { get { return "Player"; } }
        public string FileName;
        private string SaveInfo = "";

        public Animation RunningAnimation = new Animation();
        public Animation WalkingAnimation = new Animation();
        public Animation StandingAnimation = new Animation();
        private Animation CurrentAnimation = new Animation();
        /// <summary>
        /// The coordinates of the middle of the player sprite
        /// </summary>
        public Vector2 Position;
        public Vector2 Direction;
        public Vector2 TargetPoint;
        public bool StillScrollingX = false;
        private bool active = false;
        public float Scale { get; set; }
        public float BaseScale { get; set; }
        public float MaxScale { get; set; }
        public float MinScale { get; set; }
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
        public bool TargetReached
        {
            get
            {
                return ((Math.Abs(this.Position.X - this.TargetPoint.X) < 10) &&
                        (Math.Abs(this.Position.Y - this.TargetPoint.Y) < 10));
            }
        }
        public float MoveSpeed { get; set; }
        public float WalkSpeed { get; set; }
        public float RunSpeed { get; set; }
        public int Width
        {
            get { return (int)(this.CurrentAnimation.FrameWidth * this.Scale); }
        }
        public int Height
        {
            get { return (int)(this.CurrentAnimation.FrameHeight * this.Scale); }
        }

        public Player(string fileName) 
        { 
            FileName = fileName;
            ParseTextFile(SaveHandler.GetFilePath(Identifier, FileName));
        }

        public void Initialize(Texture2D texture, Vector2 position)
        {
            RunningAnimation.Initialize(texture, Vector2.Zero, 122, 138, 29, 30, Color.White, this.Scale, true);
            Position = position;
            Active = true;
            CurrentAnimation = RunningAnimation;
        }

        public void Update(GameTime gameTime)
        {
            SetPlayerSpriteDirection();
            UpdateMoveSpeed();
            CurrentAnimation.Position = Position;
            CurrentAnimation.Update(gameTime);
        }

        public void ParseTextFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] words = line.Split(':');
                switch (words[0])
                {
                    case "PlayerTexture":
                        this.PlayerTexture = words[1];
                        break;
                    default:
                        {
                            throw new InvalidOperationException("Text file error in " + filePath);
                        }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (MovingLeft)
            {
                CurrentAnimation.Draw(spriteBatch, Scale, SpriteEffects.FlipHorizontally);
            }
            else
            {
                CurrentAnimation.Draw(spriteBatch, Scale, SpriteEffects.None);
            }
        }

        /// <summary>
        /// Stop moving
        /// </summary>
        public void Stop()
        {
            TargetPoint.X = Position.X;
            TargetPoint.Y = Position.Y;
        }

        /// <summary>
        /// Moves player until within a 10 pixel square of the target
        /// </summary>
        public void MoveToTargetPoint()
        {
            if (!TargetReached)
            {
                MoveTowardsTargetPoint();
            }
            else
            {
                this.Direction = Vector2.Zero;
                this.Running = false;
            }
        }

        /// <summary>
        /// Move player towards target point
        /// </summary>
        /// <param name="point">Target point</param>
        public void MoveTowardsTargetPoint()
        {
            this.Direction.X = TargetPoint.X - this.Position.X;
            this.Direction.Y = TargetPoint.Y - this.Position.Y;
            this.Direction.Normalize();
            
            this.Position += this.Direction * this.MoveSpeed;
        }

        /// <summary>
        /// Check for player movement direction and flip sprite
        /// </summary>
        private void SetPlayerSpriteDirection()
        {
            if (!StillScrollingX)
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
        }

        /// <summary>
        /// Scale player sprite by it's Y relative to the background (scale = ky + m)
        /// </summary>
        public void ScalePlayerSprite(Vector2 backgroundVector, int backgroundHeight)
        {
            this.Scale = (this.MaxScale-this.MinScale) * ((this.Position.Y - backgroundVector.Y) / backgroundHeight) + this.MinScale;

            /*
             //A way to scale differently up and down on the screen, dividingLine == the height at which we want the scaling to change
             float scaleHeight = ((this.Position.Y - backgroundVector.Y) / backgroundHeight) * backgroundHeight;
             if(scaleFactor < dividingLine)
             {
                this.Scale = (this.BaseScale-this.MinScale) * ((this.Position.Y - backgroundVector.Y) / backgroundHeight) + this.MinScale;
             }
             else
             {
                this.Scale = (this.MaxScale-this.BaseScale) * ((this.Position.Y - backgroundVector.Y) / backgroundHeight) + this.BaseScale;
             }
           */

        }

        /// <summary>
        /// Keeps player from running off screen
        /// </summary>
        public void ClampPlayer(int viewportWidth, int viewportHeight)
        {
            this.Position.X = MathHelper.Clamp(Position.X, this.Width / 2, viewportWidth - this.Width / 2);
            this.Position.Y = MathHelper.Clamp(Position.Y, this.Height / 2, viewportHeight - this.Height / 2);

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

        public void Save() 
        { 
            SaveHandler.DeleteCurrentFile(FileName);
            SaveInfo +=  "DirectionX:" + Direction.X + Environment.NewLine;
            SaveInfo +=  "DirectionY:" + Direction.Y + Environment.NewLine;
            SaveInfo +=  "TargetPointX:" + TargetPoint.X + Environment.NewLine;
            SaveInfo +=  "TargetPointY:" + TargetPoint.Y + Environment.NewLine;
            SaveInfo +=  "StillScrollingX" + StillScrollingX + Environment.NewLine;
            SaveInfo +=  "active:" + active + Environment.NewLine;
            SaveInfo +=  "Scale:" + Scale + Environment.NewLine;
            SaveInfo +=  "BaseScale:" + BaseScale + Environment.NewLine;
            SaveInfo +=  "MaxScale:" + MaxScale + Environment.NewLine;
            SaveInfo +=  "MinScale:" + MinScale + Environment.NewLine;
            SaveInfo +=  "PlayerTexture:" + PlayerTexture + Environment.NewLine;
            SaveInfo +=  "Running:" + Running + Environment.NewLine;
            SaveInfo +=  "MovingLeft" + MovingLeft + Environment.NewLine;
            SaveInfo +=  "MoveSpeed:" + MoveSpeed + Environment.NewLine;
            SaveInfo +=  "WalkSpeed:" + WalkSpeed + Environment.NewLine;
            SaveInfo +=  "MoveSpeed:" + RunSpeed + Environment.NewLine;
            SaveHandler.SaveToCurrent(SaveInfo, FileName);
        }
    }
}
