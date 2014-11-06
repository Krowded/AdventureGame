using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventureGame
{
    class Room
    {
        private string RoomFile { get; set; }

        public string[] BackgroundImages { get; set; }
        public string[] ForegroundImages { get; set; }
        public string Background;


        public List<NPC> NPCs = new List<NPC>();
        public List<Door> Doors = new List<Door>();
        public List<Item> Items = new List<Item>();
        
        public Room(string roomInformationFile)
        {
            RoomFile = roomInformationFile;
        }

        public void InitializeRoom()
        {
            Utility.ParseRoomFile(RoomFile, ref Background, ref Doors, ref Items, ref NPCs);
            InitializeLists();           
        }

        private void InitializeLists()
        {
            foreach (Door door in Doors)
            {
                door.Initialize();
            }

            foreach (Item item in Items)
            {
                item.Initialize();
            }

            foreach (NPC npc in NPCs)
            {
                npc.Initialize();
            }
        }
    }
}
