using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;


namespace AdventureGame
{
    internal class Door : InteractiveObject
    {
        protected override string Identifier { get {return "Door";} }
        public DialogueTree Dialogue { get; set; }
        public string Destination { get; set; }
        public string PartnerDoorName { get; set; }
        
        public Door(string fileName)
        {
            FileName = fileName;
            Name = fileName.Remove(fileName.Length - 4);
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
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
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

        public override void Save()
        {
            SaveInfo += "Destination:" + Destination + Environment.NewLine;
            SaveInfo += "PartnerDoorName:" + PartnerDoorName + Environment.NewLine;
            base.Save();
        }
    }
}
