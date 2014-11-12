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
        bool MousePressed;
        bool Begin;
        bool DoubleClick { get; set; }

        //Scrolling
        Scrolling Scroller;
        
        //Elapsed time in milliseconds
        int ElapsedTime = 0;
        
        //Background
        Texture2D MainBackground;
        Vector2 BackgroundPosition;
        int BackgroundWidth = 1920;
        int BackgroundHeight = 1080;
        string BackgroundImageName;

        //The symbol for interactives
        Texture2D InteractiveSymbol;

        //Lists of interactables in the room
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

            //Initialize player variables
            player = new Player(CurrentRoom.PlayerStartingPosition);
            player.ParseTextFile(@"TextContent/Player/Player.txt");
            player.RunSpeed = GraphicsDevice.Viewport.Width / 240;
            player.WalkSpeed = GraphicsDevice.Viewport.Width / 480;

            //Initialize mouse variables
            MousePressed = false;
            Begin = false;
            ElapsedTime = 0;

            //Scroller
            Scroller = new Scrolling(GraphicsDevice.Viewport.Width / 3,
                                     2 * GraphicsDevice.Viewport.Width / 3,
                                     GraphicsDevice.Viewport.Height / 3,
                                     2 * GraphicsDevice.Viewport.Height / 3,
                                     GraphicsDevice.Viewport.Width / 2,
                                     GraphicsDevice.Viewport.Height / 2);
            
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
            player.RoomScale = CurrentRoom.PlayerScale;
            Animation playerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>(player.PlayerTexture);
            playerAnimation.Initialize(playerTexture, Vector2.Zero, 115, 69, 1, 30, Color.White, player.Scale, true);
            player.Initialize(playerAnimation, player.Position);
            
            //Initialize the background
            MainBackground = Content.Load<Texture2D>(BackgroundImageName);
            BackgroundHeight = (int)(MainBackground.Height * CurrentRoom.BackgroundScale);
            BackgroundWidth = (int)(MainBackground.Width * CurrentRoom.BackgroundScale);
            InitializeBackground();
            LoadNewRoom();

        }

        /// <summary>
        /// Makes the background match player starting coordinates
        /// </summary>
        private void InitializeBackground() 
        {
            float middleX = GraphicsDevice.Viewport.Width / 2;
            float middleY = GraphicsDevice.Viewport.Height / 2;

            //Adjust background to player on X-axis
            if ((player.Position.X > middleX) && 
                (player.Position.X < BackgroundWidth - middleX))
            {
                BackgroundPosition.X = -(player.Position.X - middleX);
                player.Position.X = middleX;
            }
            else if (player.Position.X < middleX)
            {
                BackgroundPosition.X = 0;
            }
            else if (player.Position.X > (BackgroundWidth - middleX))
            {
                BackgroundPosition.X = -(BackgroundWidth - GraphicsDevice.Viewport.Width);
                player.Position.X = BackgroundWidth - player.Position.X;
            }

            //Adjust background to player on Y-axis
            if ((player.Position.Y > middleY) &&
                (player.Position.Y < BackgroundHeight - middleY))
            {
                BackgroundPosition.Y = -(player.Position.Y - middleY);
                player.Position.Y = middleY;
            }
            else if (player.Position.Y < middleY)
            {
                BackgroundPosition.Y = 0;
            }
            else if (player.Position.Y > (BackgroundHeight - middleY))
            {
                BackgroundPosition.Y = -(BackgroundHeight - GraphicsDevice.Viewport.Height);
                player.Position.Y = BackgroundHeight - player.Position.Y;
            }
        }

        /// Loads
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
                thing.Position += BackgroundPosition;
            }
        }
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
            Vector2 targetPoint = HandleMouse(gameTime);
            player.Running = this.DoubleClick;
            
            //Move some of those into player itself?
            player.MoveToPoint(Begin, targetPoint);
            player.ScalePlayerSprite(BackgroundPosition, BackgroundHeight, GraphicsDevice.Viewport.Width, CurrentRoom.SmallestScale, NaturalScreenWidth); //Needs to be improved/changed
            player.ClampPlayer(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);    
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
            Scroller.Scroll(ref BackgroundPosition, ref MousePosition);
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

        /// <summary>
        /// Handles all in game mouse actions
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private Vector2 HandleMouse(GameTime gameTime)
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

        /// <summary>
        /// Checks if the key for revealing interactives is pressed (probably space)
        /// </summary>
        private bool RevealkeyPressed()
        {
            return false;
        }
    }
}
