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
        GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;

        //The size for which the game is programmed (consistency important for scaling)
        const int NaturalScreenWidth = 1920;
        const int NaturalScreenHeight = 1080;

        //The current room, where most is loaded from
        Room CurrentRoom;

        //Player
        Player player;

        //Mousehandling variables
        MouseState CurrentMouseState;
        MouseState PreviousMouseState;
        Vector2 MousePosition = new Vector2();
        Vector2 PosDelta = new Vector2();
        bool MousePressed;
        bool Begin;

        //Scaling
        float SmallestScale = 0.5f;

        //Elapsed time in milliseconds
        int ElapsedTime = 0;

        //Variables for moving things
        float ScrollSpeed;
        float MoveSpeed;
        bool ScrollingUp, ScrollingDown, ScrollingLeft, ScrollingRight;

        //Background
        Texture2D MainBackground;
        Vector2 BackgroundVector;
        int BackgroundWidth = 1920;
        int BackgroundHeight = 1080;
        string BackgroundImageName;

        Texture2D InteractiveSymbol;

        List<Item> items = new List<Item>();
        List<NPC> npcs = new List<NPC>();
        List<Door> doors = new List<Door>();
        List<InteractiveObject> AllThings = new List<InteractiveObject>();
        List<InteractiveObject> BackgroundThings = new List<InteractiveObject>();
        List<InteractiveObject> ForegroundThings = new List<InteractiveObject>();

        public AdventureGame()
            : base()
        {
            Graphics = new GraphicsDeviceManager(this);
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
            CurrentRoom = new Room(@"TextContent/Rooms/Room1.txt");
            CurrentRoom.InitializeRoom();
            BackgroundImageName = CurrentRoom.Background;

            //Initialize all the miscellaneous variables
            player = new Player(0, 0);
            player.ParseTextFile(@"TextContent/Player/Player.txt");
            MousePressed = false;
            Begin = false;
            ElapsedTime = 0;

            //Scrollspeed (should it really just be a multiple of movespeed?
            Running(true);
            ScrollSpeed = 4 * MoveSpeed;
            Running(false);
            
            //Sets the natural screen size (supposed to resize automatically)
            Graphics.PreferredBackBufferWidth = NaturalScreenWidth;
            Graphics.PreferredBackBufferHeight = NaturalScreenHeight;

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
            
            
            //Initialize the player
            player.Position = CurrentRoom.PlayerStartingPosition;
            Animation playerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>(player.PlayerTexture);
            playerAnimation.Initialize(playerTexture, Vector2.Zero, 115, 69, 1, 30, Color.White, player.Scale, true);
            player.Initialize(playerAnimation, player.Position);
            
            //Initialize the background
            MainBackground = Content.Load<Texture2D>(BackgroundImageName);
            BackgroundHeight = (int)(MainBackground.Height * CurrentRoom.Scale);
            BackgroundWidth = (int)(MainBackground.Width * CurrentRoom.Scale);
            InitializeBackground();
            AdjustPlayerStartingPosition();
            LoadNewRoom();

        }

        private void InitializeBackground() 
        {
            BackgroundVector.X = player.Position.X - BackgroundWidth * CurrentRoom.Scale / 2;
            BackgroundVector.Y = player.Position.Y - BackgroundHeight * CurrentRoom.Scale / 2;
        }

        private void AdjustPlayerStartingPosition()
        {
            player.Position.X = GraphicsDevice.Viewport.Width / 2;
            player.Position.Y = GraphicsDevice.Viewport.Height / 2;
        }

        private void LoadNewRoom()
        {
            //Hide old room
            items.Clear();
            npcs.Clear();
            doors.Clear();
            AllThings.Clear();

            //Load new room
            LoadItems();
            LoadNPCs();
            LoadDoors();
            AllThings.AddRange(items);
            AllThings.AddRange(npcs);
            AllThings.AddRange(doors);
            LoadBackgroundAndForegroundThings();

            foreach (InteractiveObject thing in AllThings)
            {
                thing.Position += BackgroundVector;
            }
        }

/// <summary>
/// Loads
/// </summary>

        private void LoadItems()
        {
            foreach (Item item in CurrentRoom.Items)
            {
                item.Texture = Content.Load<Texture2D>(item.Image);
                items.Add(item);
            }
        }

        private void LoadDoors()
        {
            foreach (Door door in CurrentRoom.Doors)
            {
                door.Texture = Content.Load<Texture2D>(door.Image);
                doors.Add(door);
            }
        }

        private void LoadNPCs()
        {
            foreach (NPC npc in CurrentRoom.NPCs)
            {
                npc.Texture = Content.Load<Texture2D>(npc.Image);
                npcs.Add(npc);
            }
        }

        private void LoadBackgroundAndForegroundThings() 
        {
            foreach (InteractiveObject thing in AllThings)
            {
                if (thing.Foreground)
                {
                    ForegroundThings.Add(thing);
                }
                else
                {
                    BackgroundThings.Add(thing);
                }
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
            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();

            // TODO: Add your update logic here
            UpdatePlayer(gameTime);
            UpdateScrolling(gameTime);
            player.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the player character
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void UpdatePlayer(GameTime gameTime)
        {
            //Movement controlled by mouse
            HandleMouse(gameTime);
            
            //Check for direction (sprite flip)
            if (PosDelta.X < 0)
            {
                player.movingLeft = true;
            }
            else if (PosDelta.X > 0)
            {
                player.movingLeft = false;
            }

            //Compensate for screen scrolling
            if (ScrollingLeft || ScrollingRight)
            {
                player.Position.X -= PosDelta.X * MoveSpeed;
            }

            if (ScrollingUp || ScrollingDown)
            {
                player.Position.Y -= PosDelta.Y * MoveSpeed;
            }
            
            //Scale player sprite by it's Y relative to the background
            player.Scale = (((player.Position.Y - BackgroundVector.Y) / BackgroundHeight + SmallestScale)
                *((float)GraphicsDevice.Viewport.Width / (float)NaturalScreenWidth));

            //Keeps player from running off screen
            player.Position.X = MathHelper.Clamp(player.Position.X, player.Scale*player.Width/2, GraphicsDevice.Viewport.Width - player.Width*player.Scale/2);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, player.Scale*player.Height/2, GraphicsDevice.Viewport.Height - player.Height*player.Scale/2);
               
        }

        /// <summary>
        /// Updates all NPCs, Items and Doors
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="plusX"></param>
        /// <param name="plusY"></param>
        private void UpdateAllThings(GameTime gameTime, Vector2 movementVector)
        {
            UpdateInteractiveObject(AllThings);
        }

        //Makes sure things stay in sync with background
        private void UpdateInteractiveObject(List<InteractiveObject> objectList)
        {
            foreach (InteractiveObject i in objectList)
            {
                i.Position = BackgroundVector + i.PositionOnBackground;
            }
        }


        /// <summary>
        /// Takes care of scrolling
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void UpdateScrolling(GameTime gameTime)
        {
            const float epsilon = 0.001f;
            IsScrolling();

            //Make sure the screen doesnt go outside the background
            BackgroundVector.X = MathHelper.Clamp(BackgroundVector.X, -(BackgroundWidth - GraphicsDevice.Viewport.Width), 0);
            BackgroundVector.Y = MathHelper.Clamp(BackgroundVector.Y, -(BackgroundHeight - GraphicsDevice.Viewport.Height), 0);
            if ((((int)BackgroundVector.X == 0) && ScrollingLeft) ||
               (((BackgroundVector.X + (BackgroundWidth - GraphicsDevice.Viewport.Width)) < epsilon) && ScrollingRight))
            {
                ScrollingLeft = false;
                ScrollingRight = false;
                PosDelta.X = 0;
            }

            if ((((int)BackgroundVector.Y == 0) && ScrollingUp) || 
               (((BackgroundVector.Y + (BackgroundHeight - GraphicsDevice.Viewport.Height) < epsilon)) && ScrollingDown))
            {
                ScrollingUp = false;
                ScrollingDown = false;
                PosDelta.Y = 0;
            }

            if (ScrollingLeft || ScrollingRight || ScrollingUp || ScrollingDown)
            {
                BackgroundVector -= PosDelta * ScrollSpeed;
                MousePosition -= PosDelta * ScrollSpeed;
        
                //Update NPCs, Items and Doors so that they move with the background
                UpdateAllThings(gameTime, -(PosDelta * ScrollSpeed));
            }
        }

        //Check if player is trying to move towards the edge of the screen
        private void IsScrolling()
        {
            if ((player.Position.X > 2 * GraphicsDevice.Viewport.Width / 3) && PosDelta.X > 0)
            {
                ScrollingRight = true;
            }
            else if ((player.Position.X < GraphicsDevice.Viewport.Width / 3) && PosDelta.X < 0)
            {
                ScrollingLeft = true;
            }
           
            if ((player.Position.Y > 2 * GraphicsDevice.Viewport.Height / 3) && PosDelta.Y > 0)
            {
                ScrollingDown = true;
            }
            else if ((player.Position.Y < GraphicsDevice.Viewport.Height / 3) && PosDelta.Y < 0)
            {
                ScrollingUp = true;
            }
        }

        /// <summary>
        /// Handles all in game mouse actions
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void HandleMouse(GameTime gameTime)
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
                        Running(true);
                        ElapsedTime = 0;
                    }
                    else
                    {
                        Running(false); 
                    } 
                    ElapsedTime = 0;
                }
            }
            else
            {
                MousePressed = false;
                ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            }

            //Moves it until within a 10 pixel square of the target
            if (Begin && !((Math.Abs(player.Position.X - MousePosition.X) < 10) &&
                            (Math.Abs(player.Position.Y - MousePosition.Y) < 10)))
            {
                MovePlayerTowardsPoint(ref MousePosition);
            }
            else
            {
                PosDelta = Vector2.Zero;
                Running(false);
            }
        }

        private void MovePlayerTowardsPoint(ref Vector2 point)
        {
            PosDelta.X = point.X - player.Position.X;
            PosDelta.Y = point.Y - player.Position.Y;
            PosDelta.Normalize();

            player.Position += PosDelta * MoveSpeed;
            
            if (false)//CollisionCheck())
            {
                player.Position -= PosDelta * MoveSpeed;
                PosDelta = Vector2.Zero;
            }
        }

        private bool CollisionCheck()
        {
            foreach (InteractiveObject thing in AllThings)
            {
                if (thing.Collidable && CollidesWithPlayer(thing))
                {
                    return true;
                }
            }
            return false;
        }

        private bool CollidesWithPlayer(InteractiveObject thing)
        {
            return (CollidesWithPlayerX(thing) && CollidesWithPlayerY(thing));
        }

        private bool CollidesWithPlayerX(InteractiveObject thing)
        {
            return ((player.Position.X + player.Width * player.Scale/2 > thing.Position.X) && (player.Position.X - player.Width * player.Scale/2 < (thing.Position.X + thing.Texture.Width*thing.Scale)));
        }

        private bool CollidesWithPlayerY(InteractiveObject thing)
        {
            return ((player.Position.Y + player.Height * player.Scale/2 > thing.Position.Y) && (player.Position.Y - player.Height * player.Scale/2 < (thing.Position.Y + thing.Texture.Height*thing.Scale)));
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

            //Draw the background
            spriteBatch.Draw(MainBackground, BackgroundVector, null, Color.Red, 0, Vector2.Zero, CurrentRoom.Scale, 
                SpriteEffects.None, 0);

            //Draw all background things in the room
            DrawInteractiveObjects(BackgroundThings);

            //Draw player
            player.Draw(spriteBatch);

            //Draw all foreground things
            DrawInteractiveObjects(ForegroundThings);

            if (RevealkeyPressed())
            {
                DrawInteractiveSymbol();
            }

            //End
            spriteBatch.End();
            base.Draw(gameTime);
        }

        //Draw a list of 
        private void DrawInteractiveObjects(List<InteractiveObject> objectList)
        {
            foreach (InteractiveObject i in objectList)
            {
                spriteBatch.Draw(i.Texture, i.Position, null, Color.White, 0f, Vector2.Zero, i.Scale, SpriteEffects.None, 0f);
            }
        }

        //Draw a symbol to mark anything interactable (to prevent pixelhunting problems)
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

        //Check if the key for revealing interactives is pressed (probably space)
        private bool RevealkeyPressed()
        {
            return false;
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
            }
            else
            {
                MoveSpeed = GraphicsDevice.Viewport.Width / 480;
            }
        }
    }
}
