#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using System.Collections.Generic;
using System.IO;
#endregion

namespace AdventureGame
{
    public partial class AdventureGame : Game
    {
        ////////////Needs expansion
        public void SaveProgress()
        {
            player.Save();
            CurrentRoom.Save();
            SaveHandler.DeleteCurrentFile("game");
            File.AppendAllText(SaveHandler.GetFilePath("game", "game"), "CurrentRoom:" + CurrentRoom.FileName + System.Environment.NewLine);
        }
        public void SaveSettings() { }
    }
}
