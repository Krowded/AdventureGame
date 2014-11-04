#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace AdventureGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class AdventureGame : Game
    {
        //Window  and graphics managing
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //The size for which the game is programmed (consistency important for scaling)
        const int NaturalScreenWidth = 1920;
        const int NaturalScreenHeight = 1080;

        //The current room, where most is loaded from
        Room currentRoom;

        //Player
        Player player;

        //Mousehandling variables
        MouseState currentMouseState;
        MouseState previousMouseState;
        Vector2 mousePosition = new Vector2();
        Vector2 posDelta = new Vector2();
        bool mousePressed;
        bool begin;

        //Scaling
        float scale;
        float smallestScale = 0.5f;
        int elapsedTime;

        //Variables for moving things
        int scrollSpeed;
        float MoveSpeed;
        bool scrollingRight;
        bool scrollingLeft;
        bool scrollingDown;
        bool scrollingUp;

        //Background
        Texture2D mainBackground;
        string mainBg;
        Rectangle rectBackground;
        List<Tuple<Item, Texture2D, Rectangle>> items = new List<Tuple<Item, Texture2D, Rectangle>>();
        List<Tuple<NPC, Animation, Rectangle>> npcs = new List<Tuple<NPC, Animation, Rectangle>>();
        List<Tuple<Door, Texture2D, Rectangle>> doors = new List<Tuple<Door, Texture2D, Rectangle>>();

        public AdventureGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Load info from the Room
            currentRoom = new Room("/In-Game Objects/Rooms/Room1.txt");
            currentRoom.initializeRoom();
            mainBg = "mainbackground";

            //Initialize all the miscellaneous variables
            player = new Player(0, 0);
            mousePressed = false;
            begin = false;
            elapsedTime = 0;

            //Scrollspeed (should it really just be a multiple of movespeed?
            Running(true);
            scrollSpeed = 4 * (int)MoveSpeed;
            Running(false);
            
            //Sets the natural screen size (supposed to resize automatically)
            graphics.PreferredBackBufferWidth = NaturalScreenWidth;
            graphics.PreferredBackBufferHeight = NaturalScreenHeight;

            //TouchPanel.EnabledGestures = GestureType.FreeDrag;  <- fix this at the end so that it works for phones, etc. as well

            //The end
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, 
                GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            
            //Initialize the player
            Animation playerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>("player");
            playerAnimation.Initialize(playerTexture, Vector2.Zero, 115, 69, 1, 30, Color.White, scale, true);
            player.Initialize(playerAnimation, playerPosition);
            
            //Initialize the background
            mainBackground = Content.Load<Texture2D>(mainBg);
            rectBackground = new Rectangle(0, 0, GraphicsDevice.Viewport.Width*2, 
                GraphicsDevice.Viewport.Height*2);

            //Load all Items
            if (currentRoom.Items != null)
            foreach(Item i in currentRoom.Items)
            {
                Item item = i;
                Texture2D texture = Content.Load<Texture2D>(item.Image);
                Rectangle rectang = new Rectangle(item.Coordinates.Item1, item.Coordinates.Item2, texture.Width, texture.Height);
                items.Add(new Tuple<Item, Texture2D, Rectangle>(item, texture, rectang));
            }

            //Load all NPCs
            if( currentRoom.NPCs != null )
            foreach (NPC npc in currentRoom.NPCs)
            {
                Animation anim = new Animation();
                Texture2D tex = Content.Load<Texture2D>(npc.Image);
                anim.Initialize(tex, new Vector2(npc.Coordinates.Item1, npc.Coordinates.Item2), tex.Width, tex.Height, 1, 30, Color.White, npc.Scale, true);
                Rectangle rectang = new Rectangle(npc.Coordinates.Item1, npc.Coordinates.Item2, anim.FrameWidth, anim.FrameHeight);
                npc.Scale = 1;
                npcs.Add(new Tuple<NPC, Animation, Rectangle>(npc, anim, rectang));
            }

            //Load all Doors
            if( currentRoom.Doors != null )
            foreach (Door door in currentRoom.Doors)
            {
                Texture2D texture = Content.Load<Texture2D>(door.Image);
                Rectangle rectang = new Rectangle(door.Coordinates.Item1, door.Coordinates.Item2, texture.Width, texture.Height);
                doors.Add(new Tuple<Door, Texture2D, Rectangle>(door, texture, rectang));
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Exit if esc is hit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Save mousestates
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            // TODO: Add your update logic here
            UpdatePlayer(gameTime);
            UpdateBackground(gameTime);
            player.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the player character
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void UpdatePlayer(GameTime gameTime)
        {
            float pos = player.Position.X;
            //Movement controlled by mouse
            HandleMouse(gameTime);
            
            //Check for direction (sprite flip)
            if (pos < player.Position.X)
            {
                player.movingLeft = true;
            }
            else if (pos > player.Position.X)
            {
                player.movingLeft = false;
            }

            //Compensate for screen scrolling
            if (scrollingRight)
            {
                player.Position.X -= MoveSpeed;
            }
            else if (scrollingLeft)
            {
                player.Position.X += MoveSpeed;
            }

            if (scrollingUp)
            {
                player.Position.Y += MoveSpeed;
            }
            else if (scrollingDown)
            {
                player.Position.Y -= MoveSpeed;
            }
            
            //Scale player sprite by it's Y relative to the background
            scale = (((player.Position.Y - rectBackground.Y) / rectBackground.Height + smallestScale)
                *((float)GraphicsDevice.Viewport.Width / (float)NaturalScreenWidth));

            //Keeps player from running off screen
            player.Position.X = MathHelper.Clamp(player.Position.X, scale*player.Width/2, GraphicsDevice.Viewport.Width - player.Width*scale/2);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, scale*player.Height, GraphicsDevice.Viewport.Height - player.Height*scale);
               
        }

        /// <summary>
        /// Updates all NPCs, Items and Doors
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="plusX"></param>
        /// <param name="plusY"></param>
        private void UpdateBackgroundThings(GameTime gameTime, int plusX, int plusY)
        {
            //NPCs
            for (int i = 0; i < npcs.Count; i++)
            {
                //Scale NPC sprite by it's Y relative to the background
                Rectangle rec = new Rectangle(npcs[i].Item3.X + plusX, npcs[i].Item3.Y + plusY, npcs[i].Item3.Width, npcs[i].Item3.Height);
                npcs[i] = new Tuple<NPC, Animation, Rectangle>(npcs[i].Item1, npcs[i].Item2, rec);

                npcs[i].Item1.Scale = (((npcs[i].Item3.Y - rectBackground.Y) / rectBackground.Height + smallestScale)
                    * ((float)GraphicsDevice.Viewport.Width / (float)NaturalScreenWidth));


                npcs[i].Item2.Active = true;
                npcs[i].Item2.Update(gameTime);
            }


            //Items
            for (int i = 0; i < items.Count; i++)
            {
                //Scale Item sprite by it's Y relative to the background
                Rectangle rec = new Rectangle(items[i].Item3.X + plusX, items[i].Item3.Y + plusY, items[i].Item3.Width, items[i].Item3.Height);
                items[i] = new Tuple<Item, Texture2D, Rectangle>(items[i].Item1, items[i].Item2, rec);

                items[i].Item1.Scale = (((items[i].Item3.Y - rectBackground.Y) / rectBackground.Height + smallestScale)
                    * ((float)GraphicsDevice.Viewport.Width / (float)NaturalScreenWidth));
            }

            //Doors
            for (int i = 0; i < doors.Count; i++)
            {
                //Scale Door sprite by it's Y relative to the background
                Rectangle rec = new Rectangle(doors[i].Item3.X + plusX, npcs[i].Item3.Y + plusY, doors[i].Item3.Width, doors[i].Item3.Height);
                doors[i] = new Tuple<Door, Texture2D, Rectangle>(doors[i].Item1, doors[i].Item2, rec);

                doors[i].Item1.Scale = (((doors[i].Item3.Y - rectBackground.Y) / rectBackground.Height + smallestScale)
                    * ((float)GraphicsDevice.Viewport.Width / (float)NaturalScreenWidth));
            }
        }

        /// <summary>
        /// Updates the background by taking care of scrolling etc.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void UpdateBackground(GameTime gameTime)
        {
            int previousBackgroundX = rectBackground.X;
            int previousBackgroundY = rectBackground.Y;

            //Check if scroll should be made and scroll
            if(player.Position.X > 2 * GraphicsDevice.Viewport.Width / 3 )
            {
                posDelta.Normalize();
                rectBackground.X -= scrollSpeed;
                scrollingRight = true;
            }
            else if (player.Position.X < GraphicsDevice.Viewport.Width / 3)
            {
                posDelta.Normalize();
                rectBackground.X += scrollSpeed;
                scrollingLeft = true;
            }
            else
            {
                scrollingRight = false;
                scrollingLeft = false;
            }

            if (player.Position.Y > 2 * GraphicsDevice.Viewport.Height / 3)
            {
                posDelta.Normalize();
                rectBackground.Y -= scrollSpeed;
                scrollingDown = true;
            }
            else if (player.Position.Y < GraphicsDevice.Viewport.Height / 3)
            {
                posDelta.Normalize();
                rectBackground.Y += scrollSpeed;
                scrollingUp = true;
            }
            else
            {
                scrollingDown = false;
                scrollingUp = false;
            }

            //If screen is still, then don't compensate for scrolling
            rectBackground.X = (int)MathHelper.Clamp(rectBackground.X,
                -(rectBackground.Width - GraphicsDevice.Viewport.Width), 0);
            if ((rectBackground.X == 0) || 
                (rectBackground.X == -(rectBackground.Width - GraphicsDevice.Viewport.Width)))
            {
                scrollingRight = false;
                scrollingLeft = false;
            }

            rectBackground.Y = (int)MathHelper.Clamp(rectBackground.Y, 
                -(rectBackground.Height - GraphicsDevice.Viewport.Height), 0);
            if ((rectBackground.Y == 0) || 
                (rectBackground.Y == -(rectBackground.Height - GraphicsDevice.Viewport.Height)))
            {
                scrollingUp = false;
                scrollingDown = false;
            }
            
            //Update NPCs, Items and doors so that they move with the background
            UpdateBackgroundThings(gameTime, rectBackground.X - previousBackgroundX, rectBackground.Y - previousBackgroundY);
        }

        /// <summary>
        /// Handles all in game mouse actions
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void HandleMouse(GameTime gameTime)
        {
            //Run to mouse if double click, walk if single click
            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                begin = true;
                if (!mousePressed)
                {
                    mousePressed = true;
                    mousePosition.X = currentMouseState.X - rectBackground.X;
                    mousePosition.Y = currentMouseState.Y - rectBackground.Y;

                    if (elapsedTime <= 500)
                    {
                        Running(true);
                    }
                    else
                    {
                        Running(false); 
                    } 
                    elapsedTime = 0;
                }
            }
            else
            {
                mousePressed = false;
                elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            //Moves it until within a 10 pixel square of the target
            if (begin && !((Math.Abs(player.Position.X - rectBackground.X - mousePosition.X) < 10) &&
                           (Math.Abs(player.Position.Y - rectBackground.Y - mousePosition.Y) < 10)))
            {
                posDelta.X = mousePosition.X - (player.Position.X - rectBackground.X);
                posDelta.Y = mousePosition.Y - (player.Position.Y - rectBackground.Y);
                posDelta.Normalize();
                posDelta = posDelta * MoveSpeed;
                player.Position = player.Position + posDelta;
            }
            else
            {
                Running(false);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            //spriteBatch.Draw(mainBackground, rectBackground, Color.White); 
            spriteBatch.Draw(mainBackground, rectBackground, null, 
                Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            
            //Draw each door
            foreach(Tuple<Door, Texture2D, Rectangle> i in doors)
            {
                spriteBatch.Draw(i.Item2, i.Item3, null,
                    Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }

            //Draw each npc
            foreach (Tuple<NPC, Animation, Rectangle> i in npcs)
            {
                i.Item2.Draw(spriteBatch, i.Item1.Scale, SpriteEffects.None);
            }

            
            //Draw each item
            foreach (Tuple<Item, Texture2D, Rectangle> i in items)
            {
                spriteBatch.Draw(i.Item2, i.Item3, null,
                    Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }


            //Draw player
            player.Draw(spriteBatch, scale);

            //End
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Adjusts MoveSpeed, based on windowsize
        /// </summary>
        /// <param name="on">True is on, false is off</param>
        private void Running(bool on)
        {
            if(on)
            {
                MoveSpeed = GraphicsDevice.Viewport.Width / 240;
                scrollSpeed = (int)MoveSpeed * 2;
            }
            else
            {
                MoveSpeed = GraphicsDevice.Viewport.Width / 480;
                scrollSpeed = (int)MoveSpeed * 2;
            }
        }
    }
}
