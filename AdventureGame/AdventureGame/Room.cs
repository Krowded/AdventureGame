using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventureGame
{
    class Room
    {
        private string roomFile { get; set; }

        public string[] BackgroundImages { get; set; }
        public string[] ForegroundImages { get; set; }
        public string background;


        public List<NPC> NPCs = new List<NPC>();
        public List<Door> Doors = new List<Door>();
        public List<Item> Items = new List<Item>();
        
        public Room(string roomInformationFile)
        {
            roomFile = roomInformationFile;
        }

        public void initializeRoom()
        {
            Utility.parseRoomFile(roomFile, ref background, ref Doors, ref Items, ref NPCs);
            initializeLists();           
        }

        private void initializeLists()
        {
            foreach (Door door in Doors)
            {
                door.initialize();
            }

            foreach (Item item in Items)
            {
                item.initialize();
            }

            foreach (NPC npc in NPCs)
            {
                npc.initialize();
            }
        }
    }
}
