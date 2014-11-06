using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace AdventureGame
{
    class Item : InteractiveObject
    {
        static readonly DialogueTree CombinationDialogue = new DialogueTree("TextContent/Items/CombinationDialogue.txt");

        public Item(string fileName)
        {
            this.FileName = fileName;
        }

        public Item Combine(Item otherItem)
        {
            int line = FindCombination(this.Name, otherItem.Name);
            CombinationDialogue.StartConversation(line);
            return null;
        }

        public int FindCombination(string item1, string item2)
        {
            return 0;
        }

        protected override void ParseTextFile()
        {
            StreamReader file = new StreamReader(this.FileName);
            string line = file.ReadLine();
            string[] words = line.Split(':');
            this.Observation = new DialogueTree(words[0]);
            this.Name = words[1];
            this.Image = words[2];
            float scale;
            bool collidable;
            if (!float.TryParse(words[3], out this.Position.X) ||
                !float.TryParse(words[4], out this.Position.Y) ||
                !float.TryParse(words[5], out scale) ||
                !bool.TryParse(words[6], out collidable))
            {
                throw new InvalidOperationException("Text file error in " + this.FileName);
            }
            this.PositionOnBackground += this.Position;
            this.Scale = scale;
            this.Collidable = collidable;
        }
    }
}
