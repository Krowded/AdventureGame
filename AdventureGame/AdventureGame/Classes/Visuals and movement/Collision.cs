using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AdventureGame
{
    class Collision
    {
        private bool CollisionChecked = false;

        /// <summary>
        /// Makes sure the check isn't made when not needed
        /// </summary>
        public bool ManagedCollisionCheck(Player player, List<InteractiveObject> collidables, Vector2 lastTargetPoint, int viewportWidth, int viewportHeight)
        {
            if (CollisionChecked && lastTargetPoint != player.TargetPoint && !player.TargetReached)
            {
                CollisionChecked = false;
            }
            
            //See if player will collide with anything on the way to it's destination
            if (!player.TargetReached && !CollisionChecked && CollisionCheck(collidables, player, player.TargetPoint,
                                                                             viewportWidth, viewportHeight) )
            {
                CollisionChecked = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if player will collide with anything on it's way to target point
        /// </summary>
        private bool CollisionCheck(List<InteractiveObject> thingList, Player player, Vector2 targetPoint, int viewportWidth, int viewportHeight)
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
                    int playerBoxTop = (int)(currentPosition.Y - player.Height / 2);
                    int playerBoxBottom = (int)(currentPosition.Y + player.Height / 2);
                    
                    //Check if player sprite box intersects with thing's collidable area
                    if (!(playerBoxRight < thing.CollidableAreaLeftSide ||
                          playerBoxLeft > thing.CollidableAreaRightSide ||
                          playerBoxBottom < thing.CollidableAreaTop ||
                          playerBoxTop > thing.CollidableAreaBottom))
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
        private bool StillBetweenPlayerAndTarget(Vector2 currentPosition, Vector2 currentDirection, 
                                                 Vector2 playerPosition, Vector2 targetPosition, 
                                                 int viewportWidth, int viewportHeight)
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

        //Checks if target point is on an object and returns a bool together with the object (if true)
        public bool ClickOnObjectCheck(Vector2 targetPoint, List<InteractiveObject> thingList, ref InteractiveObject clickedThing)
        {
            foreach (InteractiveObject thing in thingList)
            {
                if (targetPoint.X > thing.Position.X &&
                    targetPoint.X < thing.Position.X + thing.Width &&
                    targetPoint.Y > thing.Position.Y &&
                    targetPoint.Y < thing.Position.Y + thing.Height)
                {
                    clickedThing = thing;
                    return true;
                }
            }
            return false;
        }
    }
}
