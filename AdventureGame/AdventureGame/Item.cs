using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame
{
    class Item
    {
        public string ItemFile;

        static DialogueTree combinationDialogue = new DialogueTree("TextContent/Items/CombinationDialogue.txt");
        public string Name;
        public string Image { get; set; }
        public DialogueTree CommentDialogue { get; set; }
        public Vector2 Position;
        public Texture2D Texture { get; set; }
        public float Scale { get; set; }

        public Item(string fileName)
        {
            this.ItemFile = fileName;
        }

        public void initializeItem(DialogueTree comment, int x, int y, string name, string image, float scale) 
        {
            this.Name = name;
            this.Image = image;
            this.CommentDialogue = comment;
            this.Position.X = x;
            this.Position.Y = y;
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
