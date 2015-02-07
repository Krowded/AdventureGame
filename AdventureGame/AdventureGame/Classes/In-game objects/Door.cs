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
        static readonly string DoorDirectory = "Content/TextContent/Doors/";
        public DialogueTree Dialogue { get; set; }
        public string Destination { get; set; }
        public string PartnerDoorName { get; set; }
        
        public Door(string fileName)
        {
            this.StartingFilePath = DoorDirectory + fileName;
        }

        public override string Interact()
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

        protected override void ParseTextFile(string filePath)
        {
            base.ParseTextFile(filePath);
            using (StreamReader file = new StreamReader(filePath))
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
                            this.Destination = words[1];
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
            System.IO.File.AppendAllText(SaveHandler.CurrentSavePath + Name + ".sav", "test");
        }
    }
}
