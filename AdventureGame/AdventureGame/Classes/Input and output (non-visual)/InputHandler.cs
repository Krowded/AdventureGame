using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AdventureGame
{
    internal class InputHandling
    {
        private MouseState CurrentMouseState;
        private MouseState PreviousMouseState;
        public Vector2 MousePosition = Vector2.Zero;
        public bool MousePressed = false;
        public bool Begin = false;
        public bool DoubleClick = false;
        public int ElapsedTime = 0;

        /// <summary>
        /// Handles all in game mouse actions
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public Vector2 HandleMouse(GameTime gameTime)
        {
            //Run to mouse if double click, walk if single click
            if (CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                Begin = true;
                if (!MousePressed)
                {
                    MousePressed = true;
                    MousePosition.X = CurrentMouseState.X;
                    MousePosition.Y = CurrentMouseState.Y;

                    if (ElapsedTime <= 500)
                    {
                        DoubleClick = true;
                        ElapsedTime = 0;
                    }
                    else
                    {
                        DoubleClick = false;
                    }
                    ElapsedTime = 0;
                }
            }
            else
            {
                MousePressed = false;
                ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            }

            return MousePosition;
        }

        /// <summary>
        /// Updates mouse states
        /// </summary>
        public void UpdateMouseStates()
        {
            this.PreviousMouseState = CurrentMouseState;
            this.CurrentMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Checks if Esc is pressed.
        /// Only looks like this to quickly collect input methods in one class. Change.
        /// </summary>
        public bool EscPressed()
        {
            return (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape));
        }

        /// <summary>
        /// Checks if the key for revealing interactives is pressed (probably space)
        /// </summary>
        public bool RevealkeyPressed()
        {
            return false;
        }
    }
}
