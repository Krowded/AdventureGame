using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace AdventureGame
{
    internal class Room
    {
        private string Identifier { get { return "Room"; } }
        private string SaveInfo = "";
        public string FileName { get; set; }
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
        
        public Room(string fileName)
        {
            FileName = fileName;
            Name = fileName.Remove(fileName.Length - 4);
        }

        public void Initialize()
        {
            ParseTextFile(SaveHandler.GetFilePath(Identifier, FileName));
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
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
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

            try { SaveHandler.DeleteCurrentFile(FileName); } catch {}

            SaveInfo += "Background:" + Background + Environment.NewLine;
            foreach(Door door in Doors)
            {
                SaveInfo += "Door:" + door.FileName + Environment.NewLine;
            }
            foreach(NPC npc in NPCs)
            {
                SaveInfo += "NPC:" + npc.FileName + Environment.NewLine;
            }
            foreach(Item item in Items)
            {
                SaveInfo += "Item:" + item.FileName + Environment.NewLine;
            }
            SaveInfo += "PlayerStartingXOnBackground:" + PlayerStartingPosition.X + Environment.NewLine;
            SaveInfo += "PlayerStartingYOnBackground:" + PlayerStartingPosition.Y + Environment.NewLine;
            SaveInfo += "PlayerScaleBase:" + PlayerScaleBase + Environment.NewLine;
            SaveInfo += "PlayerScaleMin:" + PlayerScaleMin + Environment.NewLine;
            SaveInfo += "PlayerScaleMax:" + PlayerScaleMax + Environment.NewLine;
            SaveInfo += "BackgroundScale:" + BackgroundScale + Environment.NewLine;
            SaveHandler.SaveToCurrent(SaveInfo, FileName);
        }
    }
}
