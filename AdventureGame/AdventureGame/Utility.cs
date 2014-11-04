using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AdventureGame
{
    static class Utility
    {
        public static void parseRoomFile(string textFile, ref string background, ref List<Door> doors, ref List<Item> items, ref List<NPC> npcs)
        {
            StreamReader file = new StreamReader(textFile);
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] words = line.Split(':');
                    switch (words[0])
                    {
                        case "Background":
                            background = words[1];
                            break;
                        case "Door":
                            doors.Add(new Door(words[1]));
                            break;
                        case "NPC":
                            npcs.Add(new NPC(words[1]));
                            break;
                        case "Item":
                            items.Add(new Item(words[1]));
                            break;
                        default:
                            {
                                throw new InvalidOperationException("Text file error in " + textFile + ".txt");
                            }
                    }
                }
            }
        }

        public static void parseItemFile(string textFile, ref DialogueTree dialogue, ref string name, ref string imageName, ref float x, ref float y, ref float scale) 
        {
            StreamReader file = new StreamReader(textFile);
            string line = file.ReadLine();
            string[] words = line.Split(':');
            dialogue = new DialogueTree(words[0]);
            name = words[1];
            imageName = words[2];
            if (!float.TryParse(words[3], out x) ||
                !float.TryParse(words[4], out y) ||
                !float.TryParse(words[5], out scale))
            {
                throw new InvalidOperationException("Text file error in " + textFile + ".txt");
            }
        }

        public static void parseDoorFile(string textFile, ref DialogueTree dialogue, ref string name, ref string imageName, ref Room destination, ref string partnerDoor, ref float x, ref float y, ref float scale)
        {
            StreamReader file = new StreamReader(textFile);
            string line = file.ReadLine();
            string[] words = line.Split(':');
            dialogue = new DialogueTree(words[0]);
            name = words[1];
            imageName = words[2];
            destination = new Room(words[3]);
            partnerDoor = words[4];
            if (!float.TryParse(words[5], out x) ||
                !float.TryParse(words[6], out y) ||
                !float.TryParse(words[7], out scale))
            {
                throw new InvalidOperationException("Text file error in " + textFile + ".txt");
            }
        }


        public static void parseNPCFile(string textFile, ref DialogueTree dialogue, ref DialogueTree observation, ref string imageName, ref float x, ref float y, ref float scale, ref List<Item> items)
        {
            StreamReader file = new StreamReader(textFile);
            string line = file.ReadLine();
            string[] words = line.Split(':');
            dialogue = new DialogueTree(words[0]);
            observation = new DialogueTree(words[1]);
            imageName = words[2];
            if (!float.TryParse(words[3], out x) ||
                !float.TryParse(words[4], out y) ||
                !float.TryParse(words[5], out scale))
            {
                throw new InvalidOperationException("Text file error in " + textFile + ".txt");
            }
            if (words.Length > 6)
            {
                string[] itemFiles = words[6].Split('|');
                foreach (string fileName in itemFiles)
                {
                    items.Add(new Item(fileName));
                }
            }
        }
    }
}
