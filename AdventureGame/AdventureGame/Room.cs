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
        public string bg;


        public List<NPC> NPCs = new List<NPC>();
        public List<Door> Doors = new List<Door>();
        public List<Item> Items = new List<Item>();
        
        public Room(string roomInformationFile)
        {
            roomFile = roomInformationFile;
            NPCs.Add(new NPC(""));
            Doors.Add(new Door(""));
            Items.Add(new Item(""));
        }

        public void initializeRoom()
        {
            Utility.parseRoomFile(roomFile, ref bg, ref Doors, ref Items, ref NPCs);
        }
    }
}
