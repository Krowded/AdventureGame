using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AdventureGame
{
    class Scrolling
    {
        public float ScrollSpeed { get; set; }

        //Booleans
        private bool ScrollingLeft { get; set; }
        private bool ScrollingRight { get; set; }
        private bool ScrollingUp { get; set; }
        private bool ScrollingDown { get; set; }
        private bool StillScrollingX { get; set; }
        private bool StillScrollingY { get; set; }
        private bool ClampedX { get; set; }
        private bool ClampedY { get; set; }
        
        public Scrolling()
        {
            ScrollingUp = false;
            ScrollingDown = false;
            ScrollingLeft = false;
            ScrollingRight = false;
            ClampedX = false;
            ClampedY = false;
            StillScrollingX = false;
            StillScrollingY = false;
        }

        /// <summary>
        /// Checks if player is trying to move towards the edge of the screen
        /// and updates booleans accordingly
        /// </summary>
        public void CheckForScrolling(Vector2 playerPosition, Vector2 playerDirection, 
                                        int leftScrollBorder, int rightScrollBorder, 
                                        int upperScrollBorder, int lowerScrollBorder, 
                                        int middleOfScreenX, int middleOfScreenY)
        {

            //X-axis
            if ((playerPosition.X > rightScrollBorder) && playerDirection.X > 0)
            {
                ScrollingRight = true;
                ScrollingLeft = false;
            }
            else if ((playerPosition.X < leftScrollBorder) && playerDirection.X < 0)
            {
                ScrollingLeft = true;
                ScrollingRight = false;
            }
            else
            {
                ScrollingLeft = false;
                ScrollingRight = false;
            }

            //Y-axis
            if ((playerPosition.Y > lowerScrollBorder) && playerDirection.Y > 0)
            {
                ScrollingDown = true;
                ScrollingUp = false;
            }
            else if ((playerPosition.Y < upperScrollBorder) && playerDirection.Y < 0)
            {
                ScrollingUp = true;
                ScrollingDown = false;
            }
            else
            {
                ScrollingDown = false;
                ScrollingUp = false;
            }

            //Check if player in the middle of the screen
            const int epsilon = 10;
            bool isInMiddleX = Math.Abs(playerPosition.X - middleOfScreenX) < epsilon;
            bool isInMiddleY = Math.Abs(playerPosition.Y - middleOfScreenY) < epsilon;

            StillScrollingX = !(ClampedX || isInMiddleX);
            StillScrollingY = !(ClampedY || isInMiddleY);
        }

        /// <summary>
        /// Makes sure the screen doesnt go outside the background
        /// Sets ClampedX and ClampedY if clamped
        /// </summary>
        public void BackgroundClamp(ref Vector2 backgroundPosition, float lowerLimitX, float upperLimitX, float lowerLimitY, float upperLimitY)
        {
            backgroundPosition.X = MathHelper.Clamp(backgroundPosition.X, lowerLimitX, upperLimitX);
            backgroundPosition.Y = MathHelper.Clamp(backgroundPosition.Y, lowerLimitY, upperLimitY);

            const float epsilon = 10;

            //Check left/right
            ClampedX = ((Math.Abs(backgroundPosition.X - upperLimitX) < epsilon) && ScrollingLeft) ||
                       ((Math.Abs(backgroundPosition.X - lowerLimitX) < epsilon) && ScrollingRight);
            
            //Check up/down
            ClampedY = ((Math.Abs(backgroundPosition.Y - upperLimitY) < epsilon) && ScrollingUp) ||
                       ((Math.Abs(backgroundPosition.Y - lowerLimitY) < epsilon) && ScrollingDown);
        }

        /// <summary>
        /// Perform the scroll
        /// </summary>
        /// <param name="scrollDirection">Opposite of player direction</param>
        public void Scroll(ref Vector2 backgroundPosition, ref Vector2 mousePosition, Vector2 scrollDirection)
        {
            if ((ScrollingLeft || ScrollingRight) && !ClampedX)
            {
                backgroundPosition.X += scrollDirection.X * ScrollSpeed;
                mousePosition.X += scrollDirection.X * ScrollSpeed;
            }
            else if (StillScrollingX)
            {
                backgroundPosition.X += scrollDirection.X * ScrollSpeed;
                mousePosition.X += scrollDirection.X * ScrollSpeed;
            }

            if ((ScrollingUp || ScrollingDown) && !ClampedY)
            {
                backgroundPosition.Y += scrollDirection.Y * ScrollSpeed;
                mousePosition.Y += scrollDirection.Y * ScrollSpeed;
            }
            else if (StillScrollingY)
            {
                backgroundPosition.Y += scrollDirection.Y * ScrollSpeed;
                mousePosition.Y += scrollDirection.Y * ScrollSpeed;
            }
        }

        /// <summary>
        /// Compensate for screen scrolling
        /// </summary>
        public void CompensateForScrolling(Player player)
        {
            if ((ScrollingLeft || ScrollingRight) && !ClampedX)
            {
                player.Position.X -= player.Direction.X * player.MoveSpeed;
            }

            if ((ScrollingUp || ScrollingDown) && !ClampedY)
            {
                player.Position.Y -= player.Direction.Y * player.MoveSpeed;
            }
        }
    }
}
