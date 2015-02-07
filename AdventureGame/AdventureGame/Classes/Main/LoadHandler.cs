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
    class LoadHandler
    {
        private ContentManager Content;

        public LoadHandler(ContentManager content)
        {
            Content = content;
        }

        public void LoadNewRoom(Room newRoom, Door door = null)
        {
            //Initialize new room
            AdventureGame.CurrentRoom = newRoom;
            AdventureGame.CurrentRoom.Initialize();

            //Hide old room
            AdventureGame.items.Clear();
            AdventureGame.npcs.Clear();
            AdventureGame.doors.Clear();
            AdventureGame.AllThings.Clear();

            //Load new things
            LoadItems();
            LoadNPCs();
            LoadDoors();
            AdventureGame.AllThings.AddRange(AdventureGame.items);
            AdventureGame.AllThings.AddRange(AdventureGame.npcs);
            AdventureGame.AllThings.AddRange(AdventureGame.doors);
            LoadCollidables();

            //Update player to new room
            AdventureGame.player.BaseScale = AdventureGame.CurrentRoom.PlayerScaleBase;
            AdventureGame.player.MaxScale = AdventureGame.CurrentRoom.PlayerScaleMax;
            AdventureGame.player.MinScale = AdventureGame.CurrentRoom.PlayerScaleMin;
            if (door == null)
            {
                AdventureGame.player.Position = AdventureGame.CurrentRoom.PlayerStartingPosition;
            }
            else
            {
                foreach (Door dr in AdventureGame.doors)
                {
                    if (dr.Name == door.PartnerDoorName)
                    {
                        AdventureGame.player.Position.X = dr.PositionOnBackground.X + dr.Width / 2;
                        AdventureGame.player.Position.Y = dr.PositionOnBackground.Y + dr.Height / 2;
                    }
                }
            }
            //Initialize the background
            AdventureGame.background = new Background(Content.Load<Texture2D>(AdventureGame.CurrentRoom.Background), new Vector2(0, 0), AdventureGame.CurrentRoom.BackgroundScale);
            CenterPlayer();

            //Reset basic variables
            AdventureGame.InputHandler.MousePressed = false;
            AdventureGame.InputHandler.Begin = false;
            AdventureGame.InputHandler.ElapsedTime = 0;

        }
        private void LoadItems()
        {
            foreach (Item item in AdventureGame.CurrentRoom.Items)
            {
                item.Texture = Content.Load<Texture2D>(item.Image);
                AdventureGame.items.Add(item);
            }
        }
        private void LoadDoors()
        {
            foreach (Door door in AdventureGame.CurrentRoom.Doors)
            {
                door.Texture = Content.Load<Texture2D>(door.Image);
                AdventureGame.doors.Add(door);
            }
        }
        private void LoadNPCs()
        {
            foreach (NPC npc in AdventureGame.CurrentRoom.NPCs)
            {
                npc.Texture = Content.Load<Texture2D>(npc.Image);
                AdventureGame.npcs.Add(npc);
            }
        }
        private void LoadCollidables()
        {
            foreach (InteractiveObject thing in AdventureGame.AllThings)
            {
                if (thing.Collidable)
                {
                    AdventureGame.Collidables.Add(thing);
                }
            }
        }
        /// <summary>
        /// Centers screen around the player when a new room loads
        /// </summary>
        private void CenterPlayer()
        {
            if (AdventureGame.player.Position.X > AdventureGame.ViewportWidth / 2 && AdventureGame.player.Position.X < AdventureGame.background.Width - AdventureGame.ViewportWidth / 2)
            {
                AdventureGame.background.Position.X = -AdventureGame.player.Position.X + AdventureGame.ViewportWidth / 2;
                foreach (InteractiveObject thing in AdventureGame.AllThings)
                {
                    thing.Position.X -= AdventureGame.background.Position.X;
                }
                AdventureGame.player.Position.X = AdventureGame.ViewportWidth / 2;
            }
            if (AdventureGame.player.Position.Y > AdventureGame.ViewportHeight / 2 && AdventureGame.player.Position.Y < AdventureGame.background.Height - AdventureGame.ViewportHeight / 2)
            {
                AdventureGame.background.Position.Y = -AdventureGame.player.Position.Y + AdventureGame.ViewportHeight / 2;
                foreach (InteractiveObject thing in AdventureGame.AllThings)
                {
                    thing.Position.Y -= AdventureGame.background.Position.Y;
                }
                AdventureGame.player.Position.Y = AdventureGame.ViewportHeight / 2;
            }
        }
        /// <summary>
        /// Used to load stuff from text files
        /// </summary>
        /// <param name="filePath">Save file</param>
        private void ParseTextFile(string filePath) { }


    }
}
