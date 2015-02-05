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

        public override void Interact()
        {
            string passageGranted = "true";
            if (Dialogue != null)
            {
                passageGranted = Dialogue.StartConversation();
            }

            if (passageGranted == "true")
            {
                //return Destination;
            }
            else
            {
                //return null;
            }
        }

        protected override void ParseTextFile()
        {
            base.ParseTextFile();
            StreamReader file = new StreamReader(FileName);
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] words = line.Split(':');
                    switch (words[0])
                    {
                        case "PartnerDoorName":
                            this.PartnerDoorName = words[1];
                            break;
                        case "Destination":
                            this.Destination = new Room(words[1]);
                            break;
                        case "Dialogue":
                            this.Dialogue = new DialogueTree(words[1]);
                            break;
                    }

                }
            }
        }

        public override void Save()
        {
            base.Save();
            System.IO.File.AppendAllText(SaveHandler.CurrentSave + Name + ".sav", "test");
        }
    }
}
