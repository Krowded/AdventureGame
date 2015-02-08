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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class AdventureGame : Game
    {
        static readonly bool NewGame = true;

        //Window  and graphics managing
        internal static GraphicsDeviceManager Graphics;
        internal static SpriteBatch spriteBatch;
        internal static SpriteFont font;

        //The size for which the game is programmed (consistency important for scaling)
        internal static readonly int NaturalScreenWidth = 1920;
        internal static readonly int NaturalScreenHeight = 1080;
        internal static int ViewportWidth;
        internal static int ViewportHeight;

        internal static float WindowScale;

        internal static readonly string Font = "TestFont1";

        //The current room, where most is loaded from
        internal static Room CurrentRoom;
        internal static readonly string StartingRoom = "Room1.sav";

        //Player
        internal static Player player;
        internal static readonly string PlayerFile = "Player.sav";

        //Handler instances
        internal static InputHandling InputHandler = new InputHandling();
        internal static LoadHandler Loader;
        private static UpdateHandler Updater;
        private static DrawHandler Drawer = new DrawHandler();

        //Background
        internal static Background background = new Background();

        //The symbol for marking interactives
        internal static Texture2D InteractiveSymbol;

        //Lists of interactables in the room
        internal static List<Item> items = new List<Item>();
        internal static List<NPC> npcs = new List<NPC>();
        internal static List<Door> doors = new List<Door>();
        internal static List<InteractiveObject> Collidables = new List<InteractiveObject>();
        internal static List<InteractiveObject> AllThings = new List<InteractiveObject>();
        internal static List<Script> Scripts = new List<Script>();

        //Text
        internal static string CurrentStatementToDisplay;
        internal static List<string> CurrentAnswersToDisplay = new List<string>();
        internal static float TextSize = 1;

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
            //Test
            CurrentStatementToDisplay = "TestTestTesticles";
            CurrentAnswersToDisplay.Add("Test1");
            CurrentAnswersToDisplay.Add("Test2");

            // TODO: Add your initialization logic here
            //NEW GAME
            if (NewGame)
            {
                SaveHandler.DeleteCurrent();
            }

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>(Font);

            //Initialize player variables
            player = new Player(PlayerFile);

            //Probably Room dependent
            player.RunSpeed = GraphicsDevice.Viewport.Width / 240;
            player.WalkSpeed = GraphicsDevice.Viewport.Width / 480;

            //Sets the natural screen size (supposed to resize automatically)
            Graphics.PreferredBackBufferWidth = NaturalScreenWidth;
            Graphics.PreferredBackBufferHeight = NaturalScreenHeight;
            WindowScale = GraphicsDevice.Viewport.Width / NaturalScreenWidth;

            //Save window size
            ViewportWidth = GraphicsDevice.Viewport.Width;
            ViewportHeight = GraphicsDevice.Viewport.Height;

            //TouchPanel.EnabledGestures = GestureType.FreeDrag;  <- fix this at the end so that it works for phones, etc. as well

            InitializeHandlers();

            //The end
            base.Initialize();
        }

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

            Updater.Update(gameTime);
            base.Update(gameTime);
        }

        private void InitializeHandlers()
        {
            InputHandler.Begin = false;
            InputHandler.MousePressed = false;
            InputHandler.DoubleClick = false;
            Loader = new LoadHandler(Content);
            Updater = new UpdateHandler();
 
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Initialize the player
            Texture2D playerTexture = Content.Load<Texture2D>(player.PlayerTexture);
            player.Initialize(playerTexture, player.Position);

            Loader.LoadNewRoom(new Room(StartingRoom));
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
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Drawer.Draw();
            base.Draw(gameTime);
        }


        ////////////Needs fixing////////////////
        private string SaveInfo = "";
        private string FileName = "game.sav";
        public void SaveProgress()
        {
            player.Save();
            CurrentRoom.Save();
            SaveHandler.DeleteCurrentFile("game");
            File.AppendAllText(SaveHandler.GetFilePath("game", "game"), "CurrentRoom:" + CurrentRoom.FileName + System.Environment.NewLine);
            SaveHandler.DeleteCurrentFile(FileName);
            SaveInfo += "CurrentRoom:" + CurrentRoom.FileName + System.Environment.NewLine;
            SaveHandler.SaveToCurrent(SaveInfo, FileName);
            SaveInfo = "";
        }
        public void SaveSettings() { }
        ///////////////////////////////////////
    }
}
