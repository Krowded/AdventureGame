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
        public string Name { get; set; }
        public string PartnerDoorName { get; set; }

        public Door(string fileName)
        {
            this.DoorFile = fileName;
        }

        public void initializeDoor()
        {
            DialogueTree tempDialogue = new DialogueTree("");
            Room tempDestination = new Room("");
            string imageName = "";
            float x = 0;
            float y = 0;
            float scale = 0;
            string name = "";
            string partner = "";

            Utility.parseDoorFile(DoorFile, ref tempDialogue, ref name, ref imageName, ref tempDestination, ref partner, ref this.Position.X, ref this.Position.Y, ref scale);

            this.Dialogue = tempDialogue;
            this.Position.X = x;
            this.Position.Y = y;
            this.Image = imageName;
            this.Destination = tempDestination;
            this.PartnerDoorName = partner;
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
