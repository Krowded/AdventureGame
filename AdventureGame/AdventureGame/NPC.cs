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

        public NPC(string fileName)
        {
            this.FileName = fileName;
        }

        public void TalkTo()
        {
            Dialogue.StartConversation();
        }

        public void activateAnimation(string animationChoice) {}

        protected override void ParseTextFile()
        {
            StreamReader file = new StreamReader(FileName);
            string line = file.ReadLine();
            string[] words = line.Split(':');
            this.Dialogue = new DialogueTree(words[0]);
            this.Observation = new DialogueTree(words[1]);
            this.Name = words[2];
            this.Image = words[3];
            float scale;
            bool collidable;
            if (!float.TryParse(words[4], out this.Position.X) ||
                !float.TryParse(words[5], out this.Position.Y) ||
                !float.TryParse(words[6], out scale) ||
                !bool.TryParse(words[7], out collidable))
            {
                throw new InvalidOperationException("Text file error in " + FileName);
            }
            this.PositionOnBackground += this.Position;
            this.Scale = scale;
            this.Collidable = collidable;
        }
    }
}
