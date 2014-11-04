using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame
{
    class Door
    {
        public string DoorFile;

        public DialogueTree Dialogue { get; set; }
        public Vector2 Position;
        public string Image { get; set; }
        public Texture2D Texture { get; set; }
        public float Scale { get; set; }
        public Room Destination { get; set; }
        public string partnerDoorName { get; set; }

        public Door(string fileName)
        {
            this.DoorFile = fileName;
        }

        public void initializeDoor(DialogueTree doorDialogue, int x, int y, string imageName, Room destination, string partner, float scale)
        {
            this.Dialogue = doorDialogue;
            this.Position.X = x;
            this.Position.Y = y;
            this.Image = imageName;
            this.Destination = destination;
            this.partnerDoorName = partner;
            this.Scale = scale;
        }

        public Room enterDoor()
        {
            string passageGranted = "true";
            if (Dialogue != null)
            {
                passageGranted = Dialogue.startConversation();
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
    }
}
