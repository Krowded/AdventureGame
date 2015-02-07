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
            Texture2D playerTexture = Content.Load<Texture2D>(player.PlayerTexture);
            player.Initialize(playerTexture, player.Position);

            LoadNewRoom(new Room(StartingRoom));
        }

        private void LoadNewRoom(Room newRoom, Door door = null)
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

            //Update player to new room
            player.BaseScale = CurrentRoom.PlayerScaleBase;
            player.MaxScale = CurrentRoom.PlayerScaleMax;
            player.MinScale = CurrentRoom.PlayerScaleMin;
            if (door == null)
            {
                player.Position = CurrentRoom.PlayerStartingPosition;
            }
            else
            {
                foreach (Door dr in doors)
                {
                    if (dr.Name == door.PartnerDoorName)
                    {
                        player.Position.X = dr.PositionOnBackground.X + dr.Width / 2;
                        player.Position.Y = dr.PositionOnBackground.Y + dr.Height / 2;
                    }
                }
            }
            //Initialize the background
            background = new Background(Content.Load<Texture2D>(CurrentRoom.Background), new Vector2(0,0), CurrentRoom.BackgroundScale);
            CenterPlayer();

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
        /// Centers screen around the player when a new room loads
        /// </summary>
        private void CenterPlayer()
        {
            if (player.Position.X > GraphicsDevice.Viewport.Width / 2 && player.Position.X < background.Width - GraphicsDevice.Viewport.Width / 2)
            {
                background.Position.X = -player.Position.X + GraphicsDevice.Viewport.Width / 2;
                foreach (InteractiveObject thing in AllThings)
                {
                    thing.Position.X -= background.Position.X;
                }
                player.Position.X = GraphicsDevice.Viewport.Width / 2;
            }
            if (player.Position.Y > GraphicsDevice.Viewport.Height / 2 && player.Position.Y < background.Height - GraphicsDevice.Viewport.Height / 2)
            {
                background.Position.Y = -player.Position.Y + GraphicsDevice.Viewport.Height / 2;
                foreach (InteractiveObject thing in AllThings)
                {
                    thing.Position.Y -= background.Position.Y;
                }
                player.Position.Y = GraphicsDevice.Viewport.Height / 2;
            }
        }
        /// <summary>
        /// Used to load stuff from text files
        /// </summary>
        /// <param name="filePath">Save file</param>
        private void ParseTextFile(string filePath) { }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

    }
}
