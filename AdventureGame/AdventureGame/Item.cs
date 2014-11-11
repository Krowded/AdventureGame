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
    }
}
