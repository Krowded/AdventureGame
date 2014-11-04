using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame
{
    class Item
    {
        static DialogueTree combinationDialogue = new DialogueTree("In-Game Objects/Items/CombinationDialogue");
        public string Name;
        public string Image { get; set; }
        public DialogueTree CommentDialogue { get; set; }
        public Tuple<int, int> Coordinates { get; set; }
        public Texture2D Texture { get; set; }
        public float Scale { get; set; }


        public Item(string name, string image, DialogueTree comment, int x, int y, float scale) 
        {
            this.Name = name;
            this.Image = image;
            this.CommentDialogue = comment;
            this.Coordinates = new Tuple<int, int>(x, y);
            this.Scale = scale;
        }

        public Item combine(Item otherItem)
        {
            int line = findCombination(this.Name, otherItem.Name);
            combinationDialogue.startConversation(line);
            return null;
        }

        public int findCombination(string item1, string item2)
        {
            return 0;
        }
    }
}
