using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AdventureGame
{
    class Scrolling
    {
        public bool ScrollingUp { get; set; }
        public bool ScrollingDown { get; set; }
        public bool ScrollingLeft { get; set; }
        public bool ScrollingRight { get; set; }
        public float ScrollSpeed { get; set; }
        private bool ClampedX { get; set; }
        private bool ClampedY { get; set; }

        public bool ScrollingX { get; set; }
        public bool ScrollingY { get; set; }

        public Scrolling()
        {
            ScrollingUp = false;
            ScrollingDown = false;
            ScrollingLeft = false;
            ScrollingRight = false;
            ClampedX = false;
            ClampedY = false;
            ScrollingX = false;
            ScrollingY = false;
        }

        /// <summary>
        /// Checks if player is trying to move towards the edge of the screen
        /// and updates booleans accordingly
        /// </summary>
        public void IsScrolling(Vector2 playerPosition, int viewportWidth, int viewportHeight, Vector2 playerDirection)
        {
            const int epsilon = 1;

            if ((playerPosition.X > 2 * viewportWidth / 3) && playerDirection.X > 0)
            {
                ScrollingRight = true;
            }
            else if ((playerPosition.X < viewportWidth / 3) && playerDirection.X < 0)
            {
                ScrollingLeft = true;
            }

            if ((playerPosition.Y > 2 * viewportHeight / 3) && playerDirection.Y > 0)
            {
                ScrollingDown = true;
            }
            else if ((playerPosition.Y < viewportHeight / 3) && playerDirection.Y < 0)
            {
                ScrollingUp = true;
            }

            ScrollingX = !((playerPosition.X - viewportWidth / 2 < epsilon) || ClampedX);
            ScrollingY = !((playerPosition.Y - viewportHeight / 2 < epsilon) || ClampedY);
        }

        /// <summary>
        /// Makes sure the screen doesnt go outside the background
        /// Sets ClampedX and ClampedY if clamped
        /// </summary>
        public void BackgroundClamp(ref Vector2 backgroundPosition, float lowerLimitX, float upperLimitX, float lowerLimitY, float upperLimitY)
        {
            backgroundPosition.X = MathHelper.Clamp(backgroundPosition.X, lowerLimitX, upperLimitX);
            backgroundPosition.Y = MathHelper.Clamp(backgroundPosition.Y, lowerLimitY, upperLimitY);

            const float epsilon = 0.001f;

            //Check left/right
            ClampedX = ((Math.Abs(backgroundPosition.X - upperLimitX) < epsilon) && ScrollingLeft) ||
                       ((Math.Abs(backgroundPosition.X - lowerLimitX) < epsilon) && ScrollingRight);
            
            //Check up/down
            ClampedY = ((Math.Abs(backgroundPosition.Y - upperLimitY) < epsilon) && ScrollingUp) ||
                       ((Math.Abs(backgroundPosition.Y - lowerLimitY) < epsilon) && ScrollingDown);
        }

        /// <summary>
        /// Checks if background is clamped and stops scrolling if it is
        /// </summary>
        public void LimitScroll(ref Vector2 direction)
        {
            //Check left/right
            if (ClampedX)
            {
                ScrollingLeft = false;
                ScrollingRight = false;
                direction.X = 0;
            }

            //Check up/down
            if (ClampedY)
            {
                ScrollingUp = false;
                ScrollingDown = false;
                direction.Y = 0;
            }
        }

        /// <summary>
        /// Perform the scroll
        /// </summary>
        /// <param name="scrollDirection">Opposite of player direction</param>
        public void Scroll(ref Vector2 backgroundPosition, ref Vector2 mousePosition, Vector2 scrollDirection)
        {
            if (ScrollingLeft || ScrollingRight || ScrollingUp || ScrollingDown || (ScrollingX && ScrollingY))
            {
                backgroundPosition += scrollDirection * ScrollSpeed;
                mousePosition += scrollDirection * ScrollSpeed;
            }
        }

        /// <summary>
        /// Compensate for screen scrolling
        /// </summary>
        public void CompensateForScrolling(Player player)
        {
            if (ScrollingLeft || ScrollingRight)
            {
                player.Position.X -= player.Direction.X * player.MoveSpeed;
            }

            if (ScrollingUp || ScrollingDown)
            {
                player.Position.Y -= player.Direction.Y * player.MoveSpeed;
            }
        }
    }
}
