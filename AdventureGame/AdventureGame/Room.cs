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

        public NPC[] NPCs = null;
        public Door[] Doors = null;
        public Item[] Items = null;
        
        public Room(string roomInformationFile)
        {
            roomFile = roomInformationFile;
        }

        public void initializeRoom()
        {
        }
    }
}
