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
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            //Draw the background
            spriteBatch.Draw(MainBackground, BackgroundPosition, null, Color.Red, 0, Vector2.Zero, CurrentRoom.BackgroundScale, 
                SpriteEffects.None, 0);

            //Draw all background things in the room
            foreach (InteractiveObject thing in AllThings)
            {
                if (!thing.Foreground)
                {
                    thing.Draw(spriteBatch);
                }
            }
            
            //Draw player
            player.Draw(spriteBatch);

            //Draw all foreground things
            foreach (InteractiveObject thing in AllThings)
            {
                if (thing.Foreground)
                {
                    thing.Draw(spriteBatch);
                }
            }
            
            if (InputHandler.RevealkeyPressed())
            {
                DrawInteractiveSymbol();
            }

            spriteBatch.DrawString(font, "TESTINGTESTINGTESTING", player.Position, Color.Blue);
            
            //End
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Draws a list of InteractiveObjects
        /// </summary>
        private void DrawInteractiveObjects(List<InteractiveObject> objectList)
        {
            foreach (InteractiveObject thing in objectList)
            {
                thing.Draw(spriteBatch);
            }
        }
        /// <summary>
        /// Draws a symbol to mark anything interactable (to prevent pixelhunting problems)
        /// </summary>
        private void DrawInteractiveSymbol()
        {
            Vector2 temp = new Vector2(0,0);
            foreach(InteractiveObject thing in AllThings)
            {
                temp.X = thing.Position.X + thing.Texture.Width/2 - InteractiveSymbol.Width/2;
                temp.Y = thing.Position.Y + thing.Texture.Height/2 - InteractiveSymbol.Height/2;
                spriteBatch.Draw(InteractiveSymbol, temp, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
            }
        }
    }
}
