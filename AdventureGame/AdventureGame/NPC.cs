using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame
{
    class NPC
    {
        public DialogueTree Dialogue { get; set; }
        public DialogueTree Comment { get; set; }
        public string Image { get; set; }
        public Item[] items { get; set; }
        public Tuple<int, int> Coordinates { get; set; }
        public Texture2D Texture { get; set; }
        public float Scale { get; set; }

        public NPC(DialogueTree conversation, DialogueTree comment, int x, int y, string img, Item[] item, float scale)
        {
            this.Dialogue = conversation;
            this.Comment = comment;
            this.Coordinates = new Tuple<int, int>(x, y);
            this.Image = img;
            this.Scale = scale;
        }

        public void talkTo()
        {
            Dialogue.startConversation();
        }

        public void lookAt()
        {
            Comment.startConversation();
        }

        public void activateAnimation(string animationChoice) {}

    }
}
