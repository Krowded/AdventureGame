using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace AdventureGame
{
    internal class Item : InteractiveObject
    {
        //static readonly DialogueTree CombinationDialogue = new DialogueTree("CombinationDialogue.sav");
        protected override string Identifier { get { return "Item"; } }

        public Item(string fileName)
        {
            FileName = fileName;
            Name = fileName.Remove(fileName.Length - 4);
        }

        public override string Interact()
        {
            return base.Interact();
        }

        public override void Interact(InteractiveObject item) { }

        public Item Combine(Item otherItem)
        {
            int line = FindCombination(this.Name, otherItem.Name);
            //CombinationDialogue.StartConversation(line);
            return null;
        }

        public int FindCombination(string item1, string item2)
        {
            return 0;
        }

        public override void Save()
        {
            base.Save();
        }
    }
}
