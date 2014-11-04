using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventureGame
{
    static class Utility
    {
        public void ParseRoomFile(string textFile, ref string background, ref List<Door> doors, ref List<Item> items, ref List<NPC> npcs)
        {
            using (System.IO.StreamReader file =
                    new System.IO.StreamReader(textFile))
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
                            doors.Add(parseDoor(words[1]));
                            break;
                        case "NPC":
                            npcs.Add(parseNPC(words[1]));
                            break;
                        case "Item":
                            items.Add(parseItem(words[1]));
                            break;
                        default:
                            { 
                                throw new InvalidOperationException("Text file error in " + textFile + ".txt"); 
                                break;
                            }
                    }
        }

        public Item parseItem(string fileName) 
        {
            return new Item( fileName );
        }
        public Door parseDoor(string fileName)
        {
            return new Door( fileName );
        }
        public NPC parseNPC(string fileName) 
        {
            return new NPC( fileName ); 
        }
    }
}
