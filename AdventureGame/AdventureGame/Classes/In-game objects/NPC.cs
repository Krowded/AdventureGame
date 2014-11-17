﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace AdventureGame
{
    class NPC : InteractiveObject
    {
        public DialogueTree Dialogue { get; set; }

        public NPC(string fileName)
        {
            this.FileName = fileName;
        }

        public void TalkTo()
        {
            Dialogue.StartConversation();
        }

        public void activateAnimation(string animationChoice) { }


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
                        case "Dialogue":
                            this.Dialogue = new DialogueTree(words[1]);
                            break;
                    }
                }
            }
        }
    }
}