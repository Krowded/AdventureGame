using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame
{
    class Door
    {
        public DialogueTree Dialogue { get; set; }
        public Tuple<int, int> Coordinates { get; set; }
        public string Image { get; set; }
        public Texture2D Texture { get; set; }
        public float Scale { get; set; }
        public Room Destination { get; set; }

        public int partnerDoorNumber { get; set; }

        public Door(DialogueTree doorDialogue, int x_coordinate, int y_coordinate, string imageName, Room destination, int partner)
        {
            Dialogue = doorDialogue;
            Coordinates = new Tuple<int, int>(x_coordinate,y_coordinate);
            Image = imageName;
            this.Destination = destination;
            partnerDoorNumber = partner;
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
