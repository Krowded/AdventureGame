using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace AdventureGame
{
    public class InteractiveObject
    {
        public string FileName { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public Vector2 PositionOnBackground { get; set; }
        public Vector2 Position;
        public Texture2D Texture { get; set; }
        public float Scale { get; set; }
        public DialogueTree Observation { get; set; }
        public bool Collidable { get; set; }
        public bool Foreground { get; set; }

        public string LookAt()
        {
            return Observation.StartConversation();
        }

        public virtual void Initialize() 
        {
            if (FileName != "")
            {
                ParseTextFile();
            }
            else
            {
                throw new ArgumentException("FileName can not be empty");
            }
        }

        protected virtual void ParseTextFile()
        {
            StreamReader file = new StreamReader(FileName);
            {
                string line;
                float scale = 0;
                bool collidable = false;
                bool foreground = false;
                while ((line = file.ReadLine()) != null)
                {
                    string[] words = line.Split(':');
                    switch (words[0])
                    {
                        case "PositionX":
                            if (!float.TryParse(words[1], out this.Position.X))
                            {
                                this.Position.X = 0;
                            }
                            break;
                        case "PositionY":
                            if (!float.TryParse(words[1], out this.Position.Y))
                            {
                                this.Position.Y = 0;
                            }
                            break;
                        case "Observation":
                            this.Observation = new DialogueTree(words[1]);
                            break;
                        case "Name":
                            this.Name = words[1];
                            break;
                        case "Image":
                            this.Image = words[1];
                            break;
                        case "Scale":
                            float.TryParse(words[1], out scale);
                            break;
                        case "Collidable":
                            bool.TryParse(words[1], out collidable);
                            break;
                        case "Foreground":
                            bool.TryParse(words[1], out foreground);
                            break;
                    }
                }
                this.PositionOnBackground += this.Position;
                this.Scale = scale;
                this.Collidable = collidable;
                this.Foreground = foreground;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, null, Color.White, 0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0f);
        }

    }
}
