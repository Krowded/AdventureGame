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
    internal class DrawHandler
    {

        public DrawHandler()
        {
        }

        public void Draw()
        {
            AdventureGame.Graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            AdventureGame.spriteBatch.Begin();

            //Draw the background
            AdventureGame.spriteBatch.Draw(AdventureGame.background.BGTexture, AdventureGame.background.Position, null, Color.Red, 0, Vector2.Zero, AdventureGame.CurrentRoom.BackgroundScale, 
                SpriteEffects.None, 0);

            //Draw all background things in the room
            foreach (InteractiveObject thing in AdventureGame.AllThings)
            {
                if (!thing.Foreground)
                {
                    thing.Draw(AdventureGame.spriteBatch);
                }
            }
            
            //Draw player
            AdventureGame.player.Draw(AdventureGame.spriteBatch);

            //Draw all foreground things
            foreach (InteractiveObject thing in AdventureGame.AllThings)
            {
                if (thing.Foreground)
                {
                    thing.Draw(AdventureGame.spriteBatch);
                }
            }

            if (AdventureGame.InputHandler.RevealkeyPressed())
            {
                DrawInteractiveSymbol();
            }

            DrawText();

            AdventureGame.spriteBatch.End();
            //End
        }

        /// <summary>
        /// Draws a list of InteractiveObjects
        /// </summary>
        private void DrawInteractiveObjects(List<InteractiveObject> objectList)
        {
            foreach (InteractiveObject thing in objectList)
            {
                thing.Draw(AdventureGame.spriteBatch);
            }
        }
      
        private void DrawText()
        {
            AdventureGame.spriteBatch.DrawString(AdventureGame.font, AdventureGame.CurrentStatementToDisplay, new Vector2(AdventureGame.ViewportWidth/2, AdventureGame.ViewportHeight/4), Color.Blue, 0, Vector2.Zero, AdventureGame.TextSize, SpriteEffects.None, 0);
            int counter = 0;
            foreach (string line in AdventureGame.CurrentAnswersToDisplay)
            {
                AdventureGame.spriteBatch.DrawString(AdventureGame.font, line, 
                                      new Vector2((AdventureGame.ViewportWidth/2), (float)(AdventureGame.ViewportHeight*(4.0/5.0)+20*AdventureGame.TextSize*counter)), 
                                      Color.Yellow, 0, Vector2.Zero, AdventureGame.TextSize, SpriteEffects.None, 0);
                counter++;
            }
        }
        /// <summary>
        /// Draws a symbol to mark anything interactable (to prevent pixelhunting problems)
        /// </summary>
        private void DrawInteractiveSymbol()
        {
            Vector2 temp = new Vector2(0,0);
            foreach (InteractiveObject thing in AdventureGame.AllThings)
            {
                temp.X = thing.Position.X + thing.Texture.Width / 2 - AdventureGame.InteractiveSymbol.Width / 2;
                temp.Y = thing.Position.Y + thing.Texture.Height / 2 - AdventureGame.InteractiveSymbol.Height / 2;
                AdventureGame.spriteBatch.Draw(AdventureGame.InteractiveSymbol, temp, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
            }
        }
    }
}
