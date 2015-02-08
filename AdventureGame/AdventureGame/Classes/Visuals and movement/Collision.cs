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
        private int PredictionCycles = 5;

        /// <summary>
        /// Makes sure the check isn't made when not needed
        /// </summary>
        public bool ManagedCollisionCheck(Vector2 lastTargetPoint)
        {
            if (CollisionChecked && lastTargetPoint != AdventureGame.player.TargetPoint && !AdventureGame.player.TargetReached)
            {
                CollisionChecked = false;
            }
            
            //See if player will collide with anything on the way to it's destination
            if (!AdventureGame.player.TargetReached && !CollisionChecked && CollisionCheck(AdventureGame.Collidables))
            {
                CollisionChecked = true;
                return true;
            }

            return false;
        }

        /*
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
        */

        private bool CollisionCheck(List<InteractiveObject> thingList)
        {
            const int precision = 10;
            foreach (InteractiveObject thing in thingList)
            {
                Vector2 currentPosition = AdventureGame.player.Position;
                Vector2 currentDirection = AdventureGame.player.TargetPoint - AdventureGame.player.Position;
                currentDirection.Normalize();

                float playerHalfWidth = AdventureGame.player.Width * AdventureGame.player.Scale / 2f;
                float playerHalfHeight = AdventureGame.player.Height * AdventureGame.player.Scale / 2f;

                for (int i = 0; i < PredictionCycles; ++i)
                {
                    currentPosition += currentDirection * precision;
                    //Player dimensions, for readability
                    int playerBoxLeft = (int)(currentPosition.X - playerHalfWidth);
                    int playerBoxRight = (int)(currentPosition.X + playerHalfWidth);
                    int playerBoxTop = (int)(currentPosition.Y - playerHalfHeight);
                    int playerBoxBottom = (int)(currentPosition.Y + playerHalfHeight);

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
        private bool StillBetweenPlayerAndTarget()
        {
            if(AdventureGame.player.Direction.X > 0)
            {
                if (AdventureGame.player.Direction.Y > 0)
                {
                    return AdventureGame.player.Position.X < AdventureGame.ViewportWidth && AdventureGame.player.Position.X < AdventureGame.player.TargetPoint.X &&
                           AdventureGame.player.Position.Y < AdventureGame.ViewportHeight && AdventureGame.player.Position.Y < AdventureGame.player.TargetPoint.Y;
                }
                else if (AdventureGame.player.Direction.Y < 0)
                {
                    return AdventureGame.player.Position.X < AdventureGame.ViewportWidth && AdventureGame.player.Position.X < AdventureGame.player.TargetPoint.X &&
                           AdventureGame.player.Position.Y > 0 && AdventureGame.player.Position.Y > AdventureGame.player.TargetPoint.Y;
                }
            }
            else if (AdventureGame.player.Direction.X < 0)
            {
                if (AdventureGame.player.Direction.Y > 0)
                {
                    return AdventureGame.player.Position.X > 0 && AdventureGame.player.Position.X > AdventureGame.player.TargetPoint.X &&
                           AdventureGame.player.Position.Y < AdventureGame.ViewportHeight && AdventureGame.player.Position.Y < AdventureGame.player.TargetPoint.Y;
                }
                else if (AdventureGame.player.Direction.Y < 0)
                {
                    return AdventureGame.player.Position.X > 0 && AdventureGame.player.Position.X > AdventureGame.player.TargetPoint.X &&
                           AdventureGame.player.Position.Y > 0 && AdventureGame.player.Position.Y > AdventureGame.player.TargetPoint.Y;
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
