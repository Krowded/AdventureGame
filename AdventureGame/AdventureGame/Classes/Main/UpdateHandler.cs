#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using System.Collections.Generic;
using System.IO;
#endregion

namespace AdventureGame
{
    class UpdateHandler
    {
        Vector2 LastTargetPoint;
        Collision Collider;

        public UpdateHandler()
        {
            Collider = new Collision();
        }

        /// <summary>
        /// Updates the player character
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void UpdatePlayer(GameTime gameTime)
        {
            //Movement controlled by mouse
            AdventureGame.player.TargetPoint = AdventureGame.InputHandler.HandleMouse(gameTime);
            AdventureGame.player.Running = AdventureGame.InputHandler.DoubleClick;

            //Move some of these into player itself?
            if (AdventureGame.InputHandler.Begin)
            {
                //Check if click on interactable object
                InteractiveObject thing = new InteractiveObject();
                if (Collider.ClickOnObjectCheck(AdventureGame.player.TargetPoint, AdventureGame.AllThings, ref thing))
                {
                    if (Vector2.Distance(AdventureGame.player.Position, thing.MidPointPosition) < thing.DistanceToInteract)
                    {
                        string answer = thing.Interact();
                        if (thing is Door)
                        {
                            AdventureGame.CurrentRoom.Save();
                            Door door = (Door)thing;
                            AdventureGame.Loader.LoadNewRoom(new Room(answer), door);
                            AdventureGame.player.TargetPoint = AdventureGame.player.Position;
                            return;
                        }
                        else if (thing is NPC) { }
                        else if (thing is Item) { }
                    }
                    else
                    {

                    }
                }
                //Check for collisions
                else if (Collider.ManagedCollisionCheck(AdventureGame.player, AdventureGame.Collidables, LastTargetPoint, AdventureGame.ViewportWidth, AdventureGame.ViewportHeight))
                {
                    AdventureGame.player.TargetPoint = AdventureGame.player.Position;
                    AdventureGame.InputHandler.MousePosition = AdventureGame.player.Position; //This should be changed, MousePosition shouldnt have to be changed anymore here
                    AdventureGame.player.Direction = Vector2.Zero;
                }
                LastTargetPoint = AdventureGame.player.TargetPoint;
                AdventureGame.player.MoveToTargetPoint();
            }
            AdventureGame.player.ScalePlayerSprite(AdventureGame.background.Position, AdventureGame.background.Height);
            //player.ClampPlayer(ViewportWidth, ViewportHeight);  //Not really needed   
            AdventureGame.player.Update(gameTime);
        }
        /// <summary>
        /// Updates all NPCs, Items and Doors
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void UpdateAllThings(GameTime gameTime)
        {
        }
        /// <summary>
        /// Takes care of scrolling
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void UpdateScrolling(GameTime gameTime)
        {
            AdventureGame.Scroller.UpdateScrollingVariables(AdventureGame.player);
            AdventureGame.Scroller.UpdateStillScrollingDirection(AdventureGame.player);
            AdventureGame.Scroller.Scroll(ref AdventureGame.background.Position, ref AdventureGame.InputHandler.MousePosition, AdventureGame.player); //UGLY!!!
            AdventureGame.Scroller.BackgroundClamp(ref AdventureGame.background.Position, -(AdventureGame.background.Width - AdventureGame.ViewportWidth), 0, -(AdventureGame.background.Height - AdventureGame.ViewportHeight), 0);
            AdventureGame.Scroller.CompensateForScrolling(AdventureGame.player);

            SyncInteractiveObjectsWithBackground(AdventureGame.AllThings);
        }
        /// <summary>
        /// Makes sure things stay in sync with background
        /// </summary>
        public void SyncInteractiveObjectsWithBackground(List<InteractiveObject> thingList)
        {
            foreach (InteractiveObject thing in thingList)
            {
                thing.Position = AdventureGame.background.Position + thing.PositionOnBackground;
            }
        }
    }
}
