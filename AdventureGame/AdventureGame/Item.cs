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
        static readonly DialogueTree CombinationDialogue = new DialogueTree("TextContent/Items/CombinationDialogue.txt");

        public Item(string fileName)
        {
            this.FileName = fileName;
        }

        public override void Initialize() 
        {
            if (FileName != "")
            {
                DialogueTree tempDialogue = new DialogueTree("");
                string name = "";
                string image = "";
                float scale = 0;

                Utility.ParseItemFile(this.FileName, ref tempDialogue, ref name, ref image, ref this.Position.X, ref this.Position.Y, ref scale);

                this.PositionOnBackground = this.Position;
                this.Name = name;
                this.Image = image;
                this.Observation = tempDialogue;
                this.Scale = scale;
            }
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
