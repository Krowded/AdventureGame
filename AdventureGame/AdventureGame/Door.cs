using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;


namespace AdventureGame
{
    class Door : InteractiveObject
    {
        public DialogueTree Dialogue { get; set; }
        public Room Destination { get; set; }
        public string PartnerDoorName { get; set; }
        
        public Door(string fileName)
        {
            this.FileName = fileName;
        }

        public Room EnterDoor()
        {
            string passageGranted = "true";
            if (Dialogue != null)
            {
                passageGranted = Dialogue.StartConversation();
            }

            if (passageGranted == "true")
            {
                return Destination;
            }
            else
            {
                return null;
            }
        }

        protected override void ParseTextFile()
        {
            StreamReader file = new StreamReader(FileName);
            string line = file.ReadLine();
            string[] words = line.Split(':');
            this.Observation = new DialogueTree(words[0]);
            this.Name = words[1];
            this.Image = words[2];
            this.Destination = new Room(words[3]);
            this.PartnerDoorName = words[4];
            float scale;
            bool collidable;
            bool foreground;
            if (!float.TryParse(words[5], out this.Position.X) ||
                !float.TryParse(words[6], out this.Position.Y) ||
                !float.TryParse(words[7], out scale) ||
                !bool.TryParse(words[8], out collidable) ||
                !bool.TryParse(words[9], out foreground))
            {
                throw new InvalidOperationException("Text file error in " + this.FileName);
            }
            this.PositionOnBackground += Position;
            this.Scale = scale;
            this.Collidable = collidable;
            this.Foreground = foreground;
        }
    }
}
