using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace AdventureGame
{
    class Room
    {
        private string FileName { get; set; }
        
        public string Name { get; set; }
        public string[] BackgroundImages { get; set; }
        public string[] ForegroundImages { get; set; }
        public string Background;
        public float BackgroundScale = 1;
        public float PlayerScaleMax = 1;
        public float PlayerScaleMin = 1;
        public float PlayerScaleBase = 1;
        public Vector2 PlayerStartingPosition = Vector2.Zero;

        public List<NPC> NPCs = new List<NPC>();
        public List<Door> Doors = new List<Door>();
        public List<Item> Items = new List<Item>();
        
        public Room(string roomInformationFile)
        {
            this.FileName = roomInformationFile;
        }

        public void Initialize()
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

        private void ParseTextFile()
        {
            StreamReader file = new StreamReader(FileName);
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] words = line.Split(':');
                    switch (words[0])
                    {
                        case "Name":
                            this.Name = words[1];
                            break;
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
                        case "PlayerStartingXOnBackground":
                             float.TryParse(words[1], out this.PlayerStartingPosition.X);
                            break;
                        case "PlayerStartingYOnBackground":
                            float.TryParse(words[1], out this.PlayerStartingPosition.Y);
                            break;
                        case "PlayerScaleBase":
                            float.TryParse(words[1], out this.PlayerScaleBase);
                            break;
                        case "PlayerScaleMin":
                            float.TryParse(words[1], out this.PlayerScaleMin);
                            break;
                        case "PlayerScaleMax":
                            float.TryParse(words[1], out this.PlayerScaleMax);
                            break;
                        case "BackgroundScale":
                            float.TryParse(words[1], out this.BackgroundScale);
                            break;
                        default:
                            {
                                throw new InvalidOperationException("Text file error in " + FileName);
                            }
                    }
                }
            }
        }

        public void Save()
        {
            foreach (NPC npc in NPCs)
            {
                npc.Save();
            }
            foreach (Door door in Doors)
            {
                door.Save();
            }
            foreach (Item item in Items)
            {
                item.Save();
            }

            System.IO.File.WriteAllText(SaveHandler.CurrentSave + Name + ".sav", "test");
        }

        public void Load()
        {
            foreach (NPC npc in NPCs)
            {
                npc.Load();
            }
            foreach (Door door in Doors)
            {
                door.Load();
            }
            foreach (Item item in Items)
            {
                item.Load();
            }

            FileName = SaveHandler.CurrentSave;
            ParseTextFile();
        }
    }
}
