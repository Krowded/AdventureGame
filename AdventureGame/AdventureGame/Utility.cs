using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AdventureGame
{
    static class Utility
    {
        public static void ParseRoomFile(string textFile, ref string background, ref List<Door> doors, ref List<Item> items, ref List<NPC> npcs)
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

        public static void ParseItemFile(string textFile, ref DialogueTree dialogue, ref string name, ref string imageName, ref float x, ref float y, ref float scale) 
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

        public static void ParseDoorFile(string textFile, ref DialogueTree dialogue, ref string name, ref string imageName, ref Room destination, ref string partnerDoor, ref float x, ref float y, ref float scale)
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

        public static void ParseNPCFile(string textFile, ref DialogueTree dialogue, ref DialogueTree observation, ref string name, ref string imageName, ref float x, ref float y, ref float scale)
        {
            StreamReader file = new StreamReader(textFile);
            string line = file.ReadLine();
            string[] words = line.Split(':');
            dialogue = new DialogueTree(words[0]);
            observation = new DialogueTree(words[1]);
            name = words[2];
            imageName = words[3];
            if (!float.TryParse(words[4], out x) ||
                !float.TryParse(words[5], out y) ||
                !float.TryParse(words[6], out scale))
            {
                throw new InvalidOperationException("Text file error in " + textFile + ".txt");
            }
        }

        public static string ParsePlayer(string textFile)
        {
            StreamReader file = new StreamReader(textFile);
            string line = file.ReadLine();
            return line;
        }
    }
}
