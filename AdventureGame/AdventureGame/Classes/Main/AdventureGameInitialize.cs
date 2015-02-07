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
    public partial class AdventureGame : Game
    {
        static readonly bool NewGame = true;

        //Window  and graphics managing
        GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        //The size for which the game is programmed (consistency important for scaling)
        const int NaturalScreenWidth = 1920;
        const int NaturalScreenHeight = 1080;

        float WindowScale;

        const string Font = "TestFont1";

        private static string CurrentFilePath = SaveHandler.CurrentSavePath + "game.sav";

        //The current room, where most is loaded from
        Room CurrentRoom;
        const string StartingRoom = "Room1.sav";

        //Player
        Player player;
        const string PlayerFile = "Content/TextContent/Player/Player.sav";

        //Mousehandling variables
        InputHandling InputHandler = new InputHandling();

        //Scrolling
        ScrollHandler Scroller;

        //Collision
        Collision Collider;

        //State variables
        Vector2 LastTargetPoint;

        Background background = new Background();

        //The symbol for marking interactives
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
            //NEW GAME
            if (NewGame)
            {
                SaveHandler.DeleteCurrent();
            }
            /////////////////////////////////////FIX//////////////////////////////////////////////////////////////77
            if (File.Exists(CurrentFilePath))
            {
                ParseTextFile(CurrentFilePath);
            }

            //Initialize mouse
            InputHandler.Begin = false;
            InputHandler.MousePressed = false;
            InputHandler.DoubleClick = false;

            //Initialize savefile
            //???

            //Initialize player variables
            player = new Player();

            //Probably Room dependent
            player.RunSpeed = GraphicsDevice.Viewport.Width / 240;
            player.WalkSpeed = GraphicsDevice.Viewport.Width / 480;

            //Scroller
            Scroller = new ScrollHandler(GraphicsDevice.Viewport.Width / 3,
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
            WindowScale = GraphicsDevice.Viewport.Width / NaturalScreenWidth; ;

            //TouchPanel.EnabledGestures = GestureType.FreeDrag;  <- fix this at the end so that it works for phones, etc. as well

            //The end
            base.Initialize();
        }
    }
}
