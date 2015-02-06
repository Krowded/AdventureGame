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
        public string StartingFilePath { get; set; }
        private string CurrentFilePath { get; set; }
        
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
            this.StartingFilePath = roomInformationFile;
        }

        public void Initialize()
        {
            CurrentFilePath = SaveHandler.CurrentSavePath + Path.GetFileName(StartingFilePath);
            if (File.Exists(CurrentFilePath))
            {
                ParseTextFile(CurrentFilePath);
            }
            else
            {
                ParseTextFile(StartingFilePath);
            }
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

        private void ParseTextFile(string filePath)
        {
            StreamReader file = new StreamReader(filePath);
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
                                throw new InvalidOperationException("Text file error in " + filePath);
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

            try
            {
                File.Delete(CurrentFilePath);   
            }
            catch {}

            File.AppendAllText(CurrentFilePath, "Name:" + this.Name + Environment.NewLine);
            File.AppendAllText(CurrentFilePath, "Background:" + this.Background + Environment.NewLine);
            foreach(Door door in Doors)
            {
                File.AppendAllText(CurrentFilePath, "Door:" + door.StartingFilePath + Environment.NewLine);
            }
            foreach(NPC npc in NPCs)
            {
                File.AppendAllText(CurrentFilePath, "NPC:" + npc.StartingFilePath + Environment.NewLine);
            }
            foreach(Item item in Items)
            {
                File.AppendAllText(CurrentFilePath, "Item:" + item.StartingFilePath + Environment.NewLine);
            }
            File.AppendAllText(CurrentFilePath, "PlayerStartingXOnBackground:" + PlayerStartingPosition.X + Environment.NewLine);
            File.AppendAllText(CurrentFilePath, "PlayerStartingYOnBackground:" + PlayerStartingPosition.Y + Environment.NewLine);
            File.AppendAllText(CurrentFilePath, "PlayerScaleBase:" + PlayerScaleBase + Environment.NewLine);
            File.AppendAllText(CurrentFilePath, "PlayerScaleMin:" + PlayerScaleMin + Environment.NewLine);
            File.AppendAllText(CurrentFilePath, "PlayerScaleMax:" + PlayerScaleMax + Environment.NewLine);
            File.AppendAllText(CurrentFilePath, "BackgroundScale:" + BackgroundScale + Environment.NewLine);
        }
    }
}
