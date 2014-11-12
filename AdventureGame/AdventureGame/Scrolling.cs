﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AdventureGame
{
    class Scrolling
    {
        

        //Scrolling varibles
        public float ScrollSpeed { get; set; }
        public Vector2 ScrollDirection { get; set; }
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
        
        public Scrolling(int leftScrollBorder, int rightScrollBorder, 
                         int upperScrollBorder, int lowerScrollBorder, 
                         int middleOfScreenX, int middleOfScreenY)
        {
            this.LeftScrollBorder = leftScrollBorder;
            this.RightScrollBorder = rightScrollBorder;
            this.UpperScrollBorder = upperScrollBorder;
            this.LowerScrollBorder = lowerScrollBorder;
            this.MiddleOfScreenX = middleOfScreenX;
            this.MiddleOfScreenY = middleOfScreenY;
        }

        /// <summary>
        /// Checks if player is trying to move towards the edge of the screen on any axis
        /// and updates all scrolling booleans accordingly
        /// </summary>
        public void CheckForScrolling(Player player)
        {
            CheckMovementDirection(player);
           
            if (this.Moving)
            {
                this.ScrollDirection = -player.Direction;
            }

            CheckXAxisScrolling(player);
            CheckYAxisScrolling(player);

            AdjustScrollSpeed(player); //This here?
        }

        /// <summary>
        /// Checks if player is trying to move towards the edge of the screen on the X-axis
        /// and updates all scrolling booleans accordingly
        /// </summary>
        private void CheckXAxisScrolling(Player player)
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

            if (!this.MovingLeft && !this.MovingRight &&
                !this.ClampedX && Math.Abs(player.Position.X - this.MiddleOfScreenX) > 10)
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
        private void CheckYAxisScrolling(Player player)
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

            //Check if player is in the middle of the screen after scroll 
            if (!this.MovingUp && !this.MovingDown &&
                !this.ClampedY && Math.Abs(player.Position.Y - this.MiddleOfScreenY) > 10)
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
        /// <param name="player"></param>
        private void CheckMovementDirection(Player player)
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
        private void AdjustScrollSpeed(Player player)
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
        public void BackgroundClamp(ref Vector2 backgroundPosition, 
                                    float lowerLimitX, float upperLimitX, 
                                    float lowerLimitY, float upperLimitY)
        {
            //Clamp background position
            backgroundPosition.X = MathHelper.Clamp(backgroundPosition.X, lowerLimitX, upperLimitX);
            backgroundPosition.Y = MathHelper.Clamp(backgroundPosition.Y, lowerLimitY, upperLimitY);

            //Set clamped booleans
            const int epsilon = 1;
            ClampedX = (((Math.Abs(backgroundPosition.X - lowerLimitX) < epsilon) && (MovingRight || StillScrollingX)) ||
                        ((Math.Abs(backgroundPosition.X - upperLimitX) < epsilon) && (MovingLeft || StillScrollingX)));
            ClampedY = (((Math.Abs(backgroundPosition.Y - lowerLimitY) < epsilon) && (MovingDown || StillScrollingY)) ||
                        ((Math.Abs(backgroundPosition.Y - upperLimitY) < epsilon) && (MovingUp || StillScrollingY)));
        }

        /// <summary>
        /// Perform the scroll
        /// </summary>
        /// <param name="scrollDirection">Opposite of player direction</param>
        public void Scroll(ref Vector2 backgroundPosition, ref Vector2 mousePosition)
        {
            //X-axis
            if ((ScrollingLeft || ScrollingRight) && !ClampedX)
            {
                backgroundPosition.X += ScrollDirection.X * ScrollSpeed;
                mousePosition.X += ScrollDirection.X * ScrollSpeed;
            }
            else if (StillScrollingX)
            {
                backgroundPosition.X += ScrollDirection.X * ScrollSpeed;
                mousePosition.X += ScrollDirection.X * ScrollSpeed;
            }
            
            //Y-axis
            if ((ScrollingUp || ScrollingDown ) && !ClampedY)
            {
                backgroundPosition.Y += ScrollDirection.Y * ScrollSpeed;
                mousePosition.Y += ScrollDirection.Y * ScrollSpeed;
            }
            else if (StillScrollingY)
            {
                backgroundPosition.Y += ScrollDirection.Y * ScrollSpeed;
                mousePosition.Y += ScrollDirection.Y * ScrollSpeed;
            }
        }

        /// <summary>
        /// Compensate for screen scrolling
        /// </summary>
        public void CompensateForScrolling(Player player)
        {
            //X-axis
            if ((ScrollingLeft || ScrollingRight) && !ClampedX)
            {
                player.Position.X -= player.Direction.X * player.MoveSpeed;
            }
            else if (StillScrollingX && !ClampedX)
            {
                player.Position.X -= player.Direction.X * player.MoveSpeed;
                player.Position.X += this.ScrollDirection.X * ScrollSpeed;
            }

            //Y-axis
            if ((ScrollingUp || ScrollingDown) && !ClampedY)
            {
                player.Position.Y -= player.Direction.Y * player.MoveSpeed;
            }
            else if (StillScrollingY && !ClampedY)
            {
                player.Position.Y -= player.Direction.Y * player.MoveSpeed;
                player.Position.Y += this.ScrollDirection.Y * ScrollSpeed;
            }

        }
    }
}
