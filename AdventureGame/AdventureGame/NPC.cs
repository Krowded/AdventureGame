using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame
{
    class NPC
    {
        public string NPCFile;

        public DialogueTree Dialogue { get; set; }
        public DialogueTree Observation { get; set; }
        public string Image { get; set; }
        public List<Item> items;
        public Vector2 Position;
        public Texture2D Texture { get; set; }
        public float Scale { get; set; }


        public NPC(string fileName)
        {
            this.NPCFile = fileName;
        }

        public void initializeNPC()
        {
            DialogueTree conversation = new DialogueTree("");
            DialogueTree observation = new DialogueTree("");
            string imageName = "";
            float scale = 0;

            Utility.parseNPCFile(NPCFile, ref conversation, ref observation, ref imageName, ref this.Position.X, ref this.Position.Y, ref scale, ref items);

            this.Dialogue = conversation;
            this.Observation = observation;
            this.Image = imageName;
            this.Scale = scale;
        }

        public void talkTo()
        {
            Dialogue.startConversation();
        }

        public void lookAt()
        {
            Observation.startConversation();
        }

        public void activateAnimation(string animationChoice) {}

    }
}
