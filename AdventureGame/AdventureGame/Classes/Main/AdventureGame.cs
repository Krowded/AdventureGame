#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using System.Collections.Generic;
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
        SpriteFont font;

        //The size for which the game is programmed (consistency important for scaling)
        const int NaturalScreenWidth = 1920;
        const int NaturalScreenHeight = 1080;

        float WindowScale;

        const string Font = "TestFont1";

        //The current room, where most is loaded from
        Room CurrentRoom;
        const string StartingRoom = @"Content/TextContent/Rooms/Room1.txt";

        //Player
        Player player;
        const string PlayerFile = @"Content/TextContent/Player/Player.txt";

        //Mousehandling variables
        InputHandling InputHandler = new InputHandling();

        //Scrolling
        Scrolling Scroller;

        //Collision
        Collision Collider;
        
        //State variables
        Vector2 LastTargetPoint;
        
        //Background
        Texture2D MainBackground;
        Vector2 BackgroundPosition;
        int BackgroundWidth = 1920;
        int BackgroundHeight = 1080;

        //The symbol for interactives
        Texture2D InteractiveSymbol;

        //Lists of interactables in the room
        List<Item> items = new List<Item>();
        List<NPC> npcs = new List<NPC>();
        List<Door> doors = new List<Door>();
        List<InteractiveObject> Collidables = new List<InteractiveObject>();
        List<InteractiveObject> AllThings = new List<InteractiveObject>();

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

            //Initialize mouse
            InputHandler.Begin = false;
            InputHandler.MousePressed = false;
            InputHandler.DoubleClick = false;

            //Initialize savefile
            //???

            //Initialize player variables
            player = new Player(PlayerFile);

            //Probably Room dependent
            player.RunSpeed = GraphicsDevice.Viewport.Width / 240;
            player.WalkSpeed = GraphicsDevice.Viewport.Width / 480;

            //Scroller
            Scroller = new Scrolling(GraphicsDevice.Viewport.Width / 3,
                                     2 * GraphicsDevice.Viewport.Width / 3,
                                     GraphicsDevice.Viewport.Height / 3,
                                     2 * GraphicsDevice.Viewport.Height / 3,
                                     GraphicsDevice.Viewport.Width / 2,
                                     GraphicsDevice.Viewport.Height / 2);

            //Collision
            Collider = new Collision();
            
            //Sets the natural screen size (supposed to resize automatically)
            Graphics.PreferredBackBufferWidth = NaturalScreenWidth;
            Graphics.PreferredBackBufferHeight = NaturalScreenHeight;
            WindowScale = GraphicsDevice.Viewport.Width / NaturalScreenWidth;;

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
            font = Content.Load<SpriteFont>(Font);

            // TODO: use this.Content to load your game content here
            
            //Initialize the player
            player.ParseTextFile();
            Texture2D playerTexture = Content.Load<Texture2D>(player.PlayerTexture);
            player.Initialize(playerTexture, player.Position);

            LoadNewRoom(new Room(StartingRoom));
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

        // Load methods
        private void LoadNewRoom(Room newRoom)
        {
            //Initialize new room
            CurrentRoom = newRoom;
            CurrentRoom.Initialize();

            //Hide old room
            items.Clear();
            npcs.Clear();
            doors.Clear();
            AllThings.Clear();

            //Load new things
            LoadItems();
            LoadNPCs();
            LoadDoors();
            AllThings.AddRange(items);
            AllThings.AddRange(npcs);
            AllThings.AddRange(doors);
            LoadCollidables();
            foreach (InteractiveObject thing in AllThings)
            {
                thing.Position += BackgroundPosition;
            }

            //Update player to new room
            player.Position = CurrentRoom.PlayerStartingPosition;
            player.BaseScale = CurrentRoom.PlayerScaleBase;
            player.MaxScale = CurrentRoom.PlayerScaleMax;
            player.MinScale = CurrentRoom.PlayerScaleMin;

            //Initialize the background
            MainBackground = Content.Load<Texture2D>(CurrentRoom.Background);
            BackgroundHeight = (int)(MainBackground.Height * CurrentRoom.BackgroundScale);
            BackgroundWidth = (int)(MainBackground.Width * CurrentRoom.BackgroundScale);
            InitializeBackground();

            //Reset basic variables
            InputHandler.MousePressed = false;
            InputHandler.Begin = false;
            InputHandler.ElapsedTime = 0;

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
        private void LoadCollidables()
        {
            foreach (InteractiveObject thing in AllThings)
            {
                if (thing.Collidable)
                {
                    Collidables.Add(thing);
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
                        thing.LookAt();
                    }
                    else
                    {

                    }
                }
                //Check for collisions
                else if(Collider.ManagedCollisionCheck(player, Collidables, LastTargetPoint, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height))
                {
                    player.TargetPoint = player.Position;
                    InputHandler.MousePosition = player.Position; //This should be changed, MousePosition shouldnt have to be changed anymore here
                    player.Direction = Vector2.Zero;
                }
                LastTargetPoint = player.TargetPoint;
                player.MoveToTargetPoint();
            }
            player.ScalePlayerSprite(BackgroundPosition, BackgroundHeight);
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
