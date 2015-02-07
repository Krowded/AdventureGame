using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace AdventureGame
{
    internal class InteractiveObject
    {
        //Base file
        public string FileName { get; set; }
        protected string SaveInfo = "";
        protected virtual string Identifier { get { return "Object"; } }

        //Appearance
        public string Name { get; set; }
        public string Image { get; set; }
        public Vector2 PositionOnBackground { get; set; }
        public Vector2 Position;
        public Vector2 MidPointPosition { get { return new Vector2(Position.X + Width / 2, Position.Y + Height / 2); } } //Roundabout, there's probably a better way to do this. Redesign?
        public Texture2D Texture { get; set; }
        public float Scale { get; set; }
        public DialogueTree Observation { get; set; }
        public float Width { get { return this.Texture.Width * this.Scale; } }
        public float Height { get { return this.Texture.Height * this.Scale; } }


        //Player interaction
        public bool Collidable { get; set; }
        public bool Foreground { get; set; }
        public int DistanceToInteract = 100; //Test, should be property? { get; set; }

        //Collision
        private int collidableAreaTop = 0;
        public float CollidableAreaTop { get { return this.Position.Y + collidableAreaTop; } }
        private int collidableAreaBottom = 0;
        public float CollidableAreaBottom { get { return this.Position.Y + this.Height + collidableAreaBottom; } }
        private int collidableAreaLeftSide = 0;
        public float CollidableAreaLeftSide { get { return this.Position.X + collidableAreaLeftSide; } }
        private int collidableAreaRightSide = 0;
        public float CollidableAreaRightSide { get { return this.Position.X + this.Width + collidableAreaRightSide; } }

        public virtual string LookAt()
        {
            return Observation.StartConversation();
        }

        public virtual string Interact() 
        {
            return null;
        }

        public virtual void Interact(InteractiveObject item) { }

        public virtual void Initialize() 
        {
            ParseTextFile(SaveHandler.GetFilePath(Identifier, FileName));
        }

        protected virtual void ParseTextFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            float scale = 0;
            bool collidable = false;
            bool foreground = false;
            foreach (string line in lines)
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
                    case "CollidableAreaTop":
                        int.TryParse(words[1], out collidableAreaTop);
                        break;
                    case "CollidableAreaBottom":
                        int.TryParse(words[1], out collidableAreaBottom);
                        break;
                    case "CollidableAreaLeftSide":
                        int.TryParse(words[1], out collidableAreaLeftSide);
                        break;
                    case "CollidableAreaRightSide":
                        int.TryParse(words[1], out collidableAreaRightSide);
                        break;
                }
            }
            this.PositionOnBackground += this.Position;
            this.Scale = scale;
            this.Collidable = collidable;
            this.Foreground = foreground;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, null, Color.White, 0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0f);
        }

        public virtual void Save()
        {
            SaveHandler.DeleteCurrentFile(FileName);
            SaveInfo += "PositionX:" + PositionOnBackground.X + Environment.NewLine;
            SaveInfo += "PositionY:" + PositionOnBackground.Y + Environment.NewLine;
            try { SaveInfo += "Observation:" + Observation.StartingFilePath + Environment.NewLine; } catch { }
            SaveInfo += "Name:" + Name + Environment.NewLine;
            SaveInfo += "Image:" + Image + Environment.NewLine;
            SaveInfo += "Scale:" + Scale + Environment.NewLine;
            SaveInfo += "Collidable:" + Collidable + Environment.NewLine;
            SaveInfo += "Foreground:" + Foreground + Environment.NewLine;
            SaveInfo += "CollidableAreaTop:" + collidableAreaTop + Environment.NewLine;
            SaveInfo += "CollidableAreaBottom:" + collidableAreaBottom+ Environment.NewLine;
            SaveInfo += "CollidableAreaLeftSide:" + collidableAreaLeftSide + Environment.NewLine;
            SaveInfo += "CollidableAreaRightSide:" + collidableAreaRightSide + Environment.NewLine;
            SaveHandler.SaveToCurrent(SaveInfo, FileName);
            SaveInfo = "";
        }

    }
}
