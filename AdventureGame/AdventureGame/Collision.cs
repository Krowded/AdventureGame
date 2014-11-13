using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AdventureGame
{
    class Collision
    {

        /// <summary>
        /// Check if player will collide with anything on it's way to target point
        /// </summary>
        public bool CollisionCheck(List<InteractiveObject> thingList, Player player, Vector2 targetPoint, int viewportWidth, int viewportHeight)
        {
            const int precision = 10;
            foreach (InteractiveObject thing in thingList)
            {
                Vector2 currentPosition = player.Position;
                Vector2 currentDirection = targetPoint - player.Position;
                currentDirection.Normalize();

                while (StillBetweenPlayerAndTarget(currentPosition, currentDirection, player.Position, targetPoint, viewportWidth, viewportHeight))
                {
                    currentPosition += currentDirection * precision;

                    //Player dimensions, for readability
                    int playerBoxLeft = (int)(currentPosition.X - player.Width / 2);
                    int playerBoxRight = (int)(currentPosition.X + player.Width / 2);
                    int playerBoxBot = (int)(currentPosition.Y - player.Height / 2);
                    int playerBoxTop = (int)(currentPosition.Y + player.Height / 2);
                    
                    //Check if player sprite intersects with thing sprite
                    if (!(playerBoxRight < thing.Position.X ||
                          playerBoxLeft > thing.Position.X + thing.Texture.Width ||
                          playerBoxBot < thing.Position.Y ||
                          playerBoxTop > thing.Position.Y + thing.Texture.Height))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check if currentPosition is between target and player
        /// </summary>
        private bool StillBetweenPlayerAndTarget(Vector2 currentPosition, Vector2 currentDirection, Vector2 playerPosition, Vector2 targetPosition, int viewportWidth, int viewportHeight)
        {
            if(currentDirection.X > 0)
            {
                if(currentDirection.Y > 0)
                {
                    return currentPosition.X < viewportWidth && currentPosition.X < targetPosition.X &&
                           currentPosition.Y < viewportHeight && currentPosition.Y < targetPosition.Y;
                }
                else if(currentDirection.Y < 0)
                {
                    return currentPosition.X < viewportWidth && currentPosition.X < targetPosition.X &&
                           currentPosition.Y > 0 && currentPosition.Y > targetPosition.Y;
                }

            }
            else if (currentDirection.X < 0)
            {
                if (currentDirection.Y > 0)
                {
                    return currentPosition.X > 0 && currentPosition.X > targetPosition.X &&
                           currentPosition.Y < viewportHeight && currentPosition.Y < targetPosition.Y;
                }
                else if (currentDirection.Y < 0)
                {
                    return currentPosition.X > 0 && currentPosition.X > targetPosition.X &&
                           currentPosition.Y > 0 && currentPosition.Y > targetPosition.Y;
                }
            }


            return false;
        }

        private bool CollidesWithPlayer(InteractiveObject thing, Player player)
        {
            return (CollidesWithPlayerX(thing, player) && CollidesWithPlayerY(thing, player));
        }

        private bool CollidesWithPlayerX(InteractiveObject thing, Player player)
        {
            return ((player.Position.X + player.Width * player.Scale / 2 > thing.Position.X) && (player.Position.X - player.Width * player.Scale / 2 < (thing.Position.X + thing.Texture.Width * thing.Scale)));
        }

        private bool CollidesWithPlayerY(InteractiveObject thing, Player player)
        {
            return ((player.Position.Y + player.Height * player.Scale / 2 > thing.Position.Y) && (player.Position.Y - player.Height * player.Scale / 2 < (thing.Position.Y + thing.Texture.Height * thing.Scale)));
        }
    }
}
