using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame
{
    class NPC : InteractiveObject
    {
        public DialogueTree Dialogue { get; set; }
        public Texture2D Texture { get; set; }
        


        public NPC(string fileName)
        {
            this.FileName = fileName;
        }

        public override void Initialize()
        {
            if (FileName != "")
            {
                DialogueTree conversation = new DialogueTree("");
                DialogueTree observation = new DialogueTree("");
                string imageName = "";
                string name = "";
                float scale = 0;

                Utility.ParseNPCFile(FileName, ref conversation, ref observation, ref name, ref imageName, ref this.Position.X, ref this.Position.Y, ref scale);

                this.Dialogue = conversation;
                this.Observation = observation;
                this.Name = name;
                this.Image = imageName;
                this.Scale = scale;
            }
        }       

        public void TalkTo()
        {
            Dialogue.StartConversation();
        }

        public void activateAnimation(string animationChoice) {}

    }
}
