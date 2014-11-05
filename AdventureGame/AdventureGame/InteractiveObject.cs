using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame
{
    public class InteractiveObject
    {
        public string FileName { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public Vector2 Position;
        public float Scale { get; set; }
        public DialogueTree Observation { get; set; }

        public string lookAt()
        {
            return Observation.startConversation();
        }

        public virtual void initialize() { }

    }
}
