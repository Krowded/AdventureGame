using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public override void Initialize()
        {
            if (FileName != "")
            {
                DialogueTree tempDialogue = new DialogueTree("");
                Room tempDestination = new Room("");
                string imageName = "";
                float scale = 0;
                string name = "";
                string partner = "";

                Utility.ParseDoorFile(FileName, ref tempDialogue, ref name, ref imageName, ref tempDestination, ref partner, ref this.Position.X, ref this.Position.Y, ref scale);

                this.PositionOnBackground = this.Position;
                this.Observation = tempDialogue;
                this.Scale = scale;
                this.Image = imageName;

                this.Destination = tempDestination;
                this.PartnerDoorName = partner;
            }
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
    }
}
