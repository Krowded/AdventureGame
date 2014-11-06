using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AdventureGame
{
    class Room
    {
        private string FileName { get; set; }

        public string[] BackgroundImages { get; set; }
        public string[] ForegroundImages { get; set; }
        public string Background;

        public List<NPC> NPCs = new List<NPC>();
        public List<Door> Doors = new List<Door>();
        public List<Item> Items = new List<Item>();
        
        public Room(string roomInformationFile)
        {
            this.FileName = roomInformationFile;
        }

        public void InitializeRoom()
        {
            ParseTextFile();
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

        private  void ParseTextFile()
        {
            StreamReader file = new StreamReader(FileName);
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] words = line.Split(':');
                    switch (words[0])
                    {
                        case "Background":
                            this.Background = words[1];
                            break;
                        case "Door":
                            this.Doors.Add(new Door(words[1]));
                            break;
                        case "NPC":
                            this.NPCs.Add(new NPC(words[1]));
                            break;
                        case "Item":
                            this.Items.Add(new Item(words[1]));
                            break;
                        default:
                            {
                                throw new InvalidOperationException("Text file error in " + FileName);
                            }
                    }
                }
            }
        }
    }
}
