using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace AdventureGame
{
    internal class NPC : InteractiveObject
    {
        protected override string Identifier { get { return "NPC"; } }

        public DialogueTree Dialogue { get; set; }

        public NPC(string fileName)
        {
            FileName = fileName;
            Name = fileName.Remove(fileName.Length - 4);
        }

        public override string Interact()
        {
            Dialogue.StartConversation();
            return "test";
        }

        public override void Interact(InteractiveObject item)
        {
            base.Interact(item);
        }

        public void activateAnimation(string animationChoice) { }

        protected override void ParseTextFile(string filePath)
        {
            base.ParseTextFile(filePath);
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] words = line.Split(':');
                switch (words[0])
                {
                    case "Dialogue":
                        this.Dialogue = new DialogueTree(words[1]);
                        break;
                }
            }
        }

        public override void Save()
        {
            base.Save();
            //Dialogue.Save();
        }
    }
}
