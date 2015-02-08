using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AdventureGame
{
    internal class ScrollHandler
    {
        private Player player;

        //Scrolling varibles
        private float ScrollSpeed { get; set; }
        private Vector2 ScrollDirection;
        private int LeftScrollBorder { get; set; }
        private int RightScrollBorder { get; set; }
        private int UpperScrollBorder { get; set; }
        private int LowerScrollBorder { get; set; }
        private int MiddleOfScreenX { get; set; }
        private int MiddleOfScreenY { get; set; }

        //Scrolling booleans
        private bool ScrollingLeft { get; set; }
        private bool ScrollingRight { get; set; }
        private bool ScrollingUp { get; set; }
        private bool ScrollingDown { get; set; }
        private bool StillScrollingX { get; set; }
        private bool StillScrollingY { get; set; }
        private bool Moving { get; set; }
        private bool MovingUp { get; set; }
        private bool MovingDown { get; set; }
        private bool MovingLeft { get; set; }
        private bool MovingRight { get; set; }

        //Clamped booleans
        private bool ClampedX { get; set; }
        private bool ClampedY { get; set; }
        
        public ScrollHandler(int leftScrollBorder, int rightScrollBorder, 
                             int upperScrollBorder, int lowerScrollBorder)
        {
            this.LeftScrollBorder = leftScrollBorder;
            this.RightScrollBorder = rightScrollBorder;
            this.UpperScrollBorder = upperScrollBorder;
            this.LowerScrollBorder = lowerScrollBorder;
            this.MiddleOfScreenX = AdventureGame.ViewportWidth / 2;
            this.MiddleOfScreenY = AdventureGame.ViewportHeight / 2;
            this.player = AdventureGame.player;
        }

        public void UpdateVariables()
        {            
            UpdateMovingVariables();
            UpdateScrollingVariables();
            UpdateStillScrollingDirection();
        }

        /// <summary>
        /// Updates all scrolling variables and booleans
        /// </summary>
        private void UpdateScrollingVariables()
        {
            if (this.Moving)
            {
                this.ScrollDirection = -player.Direction;
            }

            UpdateXAxisScrollingVariables();
            UpdateYAxisScrollingVariables();
            UpdateScrollSpeed();
        }

        /// <summary>
        /// Checks if player is trying to move towards the edge of the screen on the X-axis
        /// and updates all scrolling booleans accordingly
        /// </summary>
        private void UpdateXAxisScrollingVariables()
        {
            if (player.Position.X < LeftScrollBorder && MovingLeft)
            {
                ScrollingLeft = true;
                ScrollingRight = false;
            }
            else if (player.Position.X > RightScrollBorder && MovingRight)
            {
                ScrollingLeft = false;
                ScrollingRight = true;
            }
            else
            {
                ScrollingLeft = false;
                ScrollingRight = false;
            }

            //Try to center player
            if (!this.MovingLeft && !this.MovingRight && (Math.Abs(player.Position.X - this.MiddleOfScreenX) > 10) && !ClampedX)
            {
                this.StillScrollingX = true;
            }
            else
            {
                this.StillScrollingX = false;
            }

        }

        /// <summary>
        /// Checks if player is trying to move towards the edge of the screen on the Y-axis
        /// and updates all scrolling booleans accordingly
        /// </summary>
        private void UpdateYAxisScrollingVariables()
        {
            if (player.Position.Y < UpperScrollBorder && MovingUp)
            {
                ScrollingUp = true;
                ScrollingDown = false;
            }
            else if (player.Position.Y > LowerScrollBorder && MovingDown)
            {
                ScrollingUp = false;
                ScrollingDown = true;
            }
            else
            {
                ScrollingUp = false;
                ScrollingDown = false;
            }

            //Try to center player
            if (!this.MovingUp && !this.MovingDown && (Math.Abs(player.Position.Y - this.MiddleOfScreenY) > 10) && !ClampedY)
            {
                this.StillScrollingY = true;
            }
            else
            {
                this.StillScrollingY = false;
            }
        }

        /// <summary>
        /// Check which direction player is moving and change booleans
        /// </summary>
        private void UpdateMovingVariables()
        {
            //X-axis
            if (player.Direction.X > 0)
            {
                this.MovingRight = true;
                this.MovingLeft = false;
            }
            else if (player.Direction.X < 0)
            {
                this.MovingRight = false;
                this.MovingLeft = true;
            }
            else
            {
                this.MovingLeft = false;
                this.MovingRight = false;
            }

            //Y-axis
            if (player.Direction.Y > 0)
            {
                this.MovingDown = true;
                this.MovingUp = false;
            }
            else if (player.Direction.Y < 0)
            {
                this.MovingDown = false;
                this.MovingUp = true;
            }
            else
            {
                this.MovingUp = false;
                this.MovingDown = false;
            }

            //Moving at all
            this.Moving = this.MovingUp || this.MovingDown || this.MovingLeft || this.MovingRight;
        }

        /// <summary>
        /// Adjust scrollspeed to make it smooth
        /// </summary>
        private void UpdateScrollSpeed()
        {
            if (ScrollingUp || ScrollingDown || ScrollingLeft || ScrollingRight)
            {
                ScrollSpeed = 4 * player.MoveSpeed;
            }
            else if (StillScrollingX || StillScrollingY)
            {
                ScrollSpeed = 2 * player.MoveSpeed;
            }
        }

        /// <summary>
        /// Makes sure the screen doesnt go outside the background
        /// Sets ClampedX and ClampedY if clamped
        /// </summary>
        public void BackgroundClamp(Background background,
                                    float lowerLimitX, float upperLimitX, 
                                    float lowerLimitY, float upperLimitY)
        {
            //Clamp background position
            background.Position.X = MathHelper.Clamp(background.Position.X, lowerLimitX, upperLimitX);
            background.Position.Y = MathHelper.Clamp(background.Position.Y, lowerLimitY, upperLimitY);

            //Set Clamped booleans if clamped
            const int epsilon = 1;
            ClampedX = (((Math.Abs(background.Position.X - lowerLimitX) < epsilon) && (MovingRight || !Moving)) ||
                        ((Math.Abs(background.Position.X - upperLimitX) < epsilon) && (MovingLeft || !Moving)));
            ClampedY = (((Math.Abs(background.Position.Y - lowerLimitY) < epsilon) && (MovingDown || !Moving)) ||
                        ((Math.Abs(background.Position.Y - upperLimitY) < epsilon) && (MovingUp || !Moving)));
        }

        /// <summary>
        /// Perform the scroll
        /// </summary>
        /// <param name="scrollDirection">Opposite of player direction</param>
        public void Scroll(Background background, ref Vector2 mousePosition)
        {
            player.StillScrollingX = false;
            //X-axis
            if ((ScrollingLeft || ScrollingRight) && !ClampedX)
            {
                background.Position.X += ScrollDirection.X * ScrollSpeed;
                mousePosition.X += ScrollDirection.X * ScrollSpeed;
            }
            else if (StillScrollingX)
            {
                background.Position.X += ScrollDirection.X * ScrollSpeed;
                mousePosition.X += ScrollDirection.X * ScrollSpeed;
                player.StillScrollingX = true;
            }
            
            //Y-axis
            if ((ScrollingUp || ScrollingDown ) && !ClampedY)
            {
                background.Position.Y += ScrollDirection.Y * ScrollSpeed;
                mousePosition.Y += ScrollDirection.Y * ScrollSpeed;
            }
            else if (StillScrollingY)
            {
                background.Position.Y += ScrollDirection.Y * ScrollSpeed;
                mousePosition.Y += ScrollDirection.Y * ScrollSpeed;
            }
        }

        /// <summary>
        /// Makes sure the screen scrolls in the right direction during StillScrolling
        /// </summary>
        private void UpdateStillScrollingDirection()
        {
            if (StillScrollingX)
            {
                if ((player.Position.X < MiddleOfScreenX && this.ScrollDirection.X < 0) ||
                    (player.Position.X > MiddleOfScreenX && this.ScrollDirection.X > 0))
                {
                    this.ScrollDirection.X = -this.ScrollDirection.X;
                }
            }

            if (StillScrollingY)
            {
                if ((player.Position.Y < MiddleOfScreenY && this.ScrollDirection.Y < 0) ||
                    (player.Position.Y > MiddleOfScreenY && this.ScrollDirection.Y > 0))
                {
                    this.ScrollDirection.Y = -this.ScrollDirection.Y;
                }
            }
        }

        /// <summary>
        /// Compensate for screen scrolling
        /// </summary>
        public void CompensateForScrolling()
        {
            //X-axis
            if ((ScrollingLeft || ScrollingRight) && !ClampedX)
            {
                player.Position.X -= player.Direction.X * player.MoveSpeed;
                player.TargetPoint.X -= player.Direction.X * player.MoveSpeed;
            }
            else if (StillScrollingX && !ClampedX)
            {
                //Sometimes it gets too slow, this is a quick fix
                float scrollDirection = MathHelper.Clamp(this.ScrollDirection.X, 0.1f, 1f);

                player.Position.X -= player.Direction.X * player.MoveSpeed;
                player.Position.X += scrollDirection * ScrollSpeed;
            }

            //Y-axis
            if ((ScrollingUp || ScrollingDown) && !ClampedY)
            {
                player.Position.Y -= player.Direction.Y * player.MoveSpeed;
            }
            else if (StillScrollingY && !ClampedY)
            {
                //Sometimes it gets too slow, this is a quick fix
                float scrollDirection = MathHelper.Clamp(this.ScrollDirection.Y, 0.1f, 1f);
                
                player.Position.Y -= player.Direction.Y * player.MoveSpeed;
                player.Position.Y += scrollDirection * ScrollSpeed;
            }

        }
    }
}
