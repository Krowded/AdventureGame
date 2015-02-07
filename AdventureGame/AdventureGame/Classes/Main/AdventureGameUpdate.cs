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
    public partial class AdventureGame : Game
    {
        // Updater methods
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Exit if esc is hit
            if (InputHandler.EscPressed())
                Exit();

            //Save mousestates
            InputHandler.UpdateMouseStates();

            // TODO: Add your update logic here
            UpdatePlayer(gameTime);
            UpdateScrolling(gameTime);
            UpdateAllThings(gameTime); //Does nothing
            base.Update(gameTime);
        }
        /// <summary>
        /// Updates the player character
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void UpdatePlayer(GameTime gameTime)
        {
            //Movement controlled by mouse
            player.TargetPoint = InputHandler.HandleMouse(gameTime);
            player.Running = InputHandler.DoubleClick;

            //Move some of these into player itself?
            if (InputHandler.Begin)
            {
                //Check if click on interactable object
                InteractiveObject thing = new InteractiveObject();
                if (Collider.ClickOnObjectCheck(player.TargetPoint, AllThings, ref thing))
                {
                    if (Vector2.Distance(player.Position, thing.MidPointPosition) < thing.DistanceToInteract)
                    {
                        string answer = thing.Interact();
                        if (thing is Door)
                        {
                            CurrentRoom.Save();
                            Door door = (Door)thing;
                            LoadNewRoom(new Room(answer), door);
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
                else if (Collider.ManagedCollisionCheck(player, Collidables, LastTargetPoint, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height))
                {
                    player.TargetPoint = player.Position;
                    InputHandler.MousePosition = player.Position; //This should be changed, MousePosition shouldnt have to be changed anymore here
                    player.Direction = Vector2.Zero;
                }
                LastTargetPoint = player.TargetPoint;
                player.MoveToTargetPoint();
            }
            player.ScalePlayerSprite(BackgroundPosition, BackgroundHeight);
            //player.ClampPlayer(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);  //Not really needed   
            player.Update(gameTime);
        }
        /// <summary>
        /// Updates all NPCs, Items and Doors
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void UpdateAllThings(GameTime gameTime)
        {
        }
        /// <summary>
        /// Takes care of scrolling
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void UpdateScrolling(GameTime gameTime)
        {
            Scroller.UpdateScrollingVariables(player);
            Scroller.UpdateStillScrollingDirection(player);
            Scroller.Scroll(ref BackgroundPosition, ref InputHandler.MousePosition, player); //UGLY!!!
            Scroller.BackgroundClamp(ref BackgroundPosition, -(BackgroundWidth - GraphicsDevice.Viewport.Width), 0, -(BackgroundHeight - GraphicsDevice.Viewport.Height), 0);
            Scroller.CompensateForScrolling(player);

            SyncInteractiveObjectsWithBackground(AllThings);
        }
        /// <summary>
        /// Makes sure things stay in sync with background
        /// </summary>
        private void SyncInteractiveObjectsWithBackground(List<InteractiveObject> thingList)
        {
            foreach (InteractiveObject thing in thingList)
            {
                thing.Position = BackgroundPosition + thing.PositionOnBackground;
            }
        }
    }
}
