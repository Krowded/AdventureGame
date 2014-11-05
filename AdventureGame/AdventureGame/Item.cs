using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame
{
    class Item : InteractiveObject
    {
        static readonly DialogueTree combinationDialogue = new DialogueTree("TextContent/Items/CombinationDialogue.txt");

        public Texture2D Texture { get; set; }

        public Item(string fileName)
        {
            this.FileName = fileName;
        }

        public override void initialize() 
        {
            if (FileName != "")
            {
                DialogueTree tempDialogue = new DialogueTree("");
                string name = "";
                string image = "";
                float scale = 0;

                Utility.parseItemFile(this.FileName, ref tempDialogue, ref name, ref image, ref this.Position.X, ref this.Position.Y, ref scale);
                
                this.Name = name;
                this.Image = image;
                this.Observation = tempDialogue;
                this.Scale = scale;
            }
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
