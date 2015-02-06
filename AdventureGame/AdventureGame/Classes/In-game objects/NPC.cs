using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace AdventureGame
{
    class NPC : InteractiveObject
    {
        public DialogueTree Dialogue { get; set; }

        public NPC(string filePath)
        {
            this.StartingFilePath = filePath;
        }

        public override void Interact()
        {
            Dialogue.StartConversation();
        }

        public override void Interact(InteractiveObject item)
        {
            base.Interact(item);
        }

        public void activateAnimation(string animationChoice) { }

        protected override void ParseTextFile(string filePath)
        {
            base.ParseTextFile(filePath);
            StreamReader file = new StreamReader(filePath);
            {
                string line;
                while ((line = file.ReadLine()) != null)
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
        }

        public override void Save()
        {
            base.Save();
            Dialogue.Save();
        }
    }
}
