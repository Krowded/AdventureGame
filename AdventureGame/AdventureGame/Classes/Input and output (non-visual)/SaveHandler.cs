using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AdventureGame
{
    static class SaveHandler
    {
        private static readonly string CurrentSavePath = "Savefiles/Current/";
        private static readonly string SaveDirectory = "Savefiles/Savegames/";
        private static readonly string ItemDirectory = "Content/TextContent/Items/";
        private static readonly string DoorDirectory = "Content/TextContent/Doors/";
        private static readonly string NPCDirectory = "Content/TextContent/NPCs/";
        private static readonly string RoomDirectory = "Content/TextContent/Rooms/";
        private static readonly string PlayerDirectory = "Content/TextContent/Player/";
        private static readonly string DialogueDirectory = "Content/TextContent/Dialogues/";

        internal static void Save(string savename)
        {
            string[] filePaths = Directory.GetFiles(CurrentSavePath);

            DeleteDirectory(savename);

            CreateDirectory(savename);

            foreach (string filePath in filePaths)
            {
                File.Move(filePath, SaveDirectory + savename);
            }
        }

        internal static List<string> GetSavegames()
        {
            List<string> list = new List<string>();
            foreach (string directory in Directory.GetDirectories(SaveDirectory))
            {
                list.Add(Path.GetDirectoryName(directory));
            }
            return list;
        }
        private static List<string> GetSaveFiles(string savename)
        {
            string[] filePaths = Directory.GetDirectories(SaveDirectory + savename);
            List<string> list = new List<string>();
            foreach (string str in filePaths)
            {
                list.Add(str);
            }
            return list;
        }
        internal static string GetFilePath(string identifier, string filename)
        {
            string filepath = CurrentSavePath + filename;
            if (File.Exists(filepath))
            {
                return filepath;
            }
            else
            {
                switch (identifier)
                {
                    case "Door":
                        filepath = DoorDirectory;
                        break;
                    case "Item":
                        filepath = ItemDirectory;
                        break;
                    case "NPC":
                        filepath = NPCDirectory;
                        break;
                    case "Room":
                        filepath = RoomDirectory;
                        break;
                    case "Player":
                        filepath = PlayerDirectory;
                        break;
                    case "Dialogue":
                        filepath = DialogueDirectory;
                        break;
                    default:
                        throw new ArgumentException("Unknown identifier for filename: " + filename);
                }
                return filepath + filename;
            }
        }

        private static void CreateDirectory(string savename)
        {
            Directory.CreateDirectory(SaveDirectory + savename);
        }
        private static void DeleteDirectory(string savename)
        {
            try
            {
                Directory.Delete(SaveDirectory + savename);
            }
            catch { }
        }
        private static void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        internal static void SaveToCurrent(string saveInfo, string filename)
        {
            File.WriteAllText(CurrentSavePath + filename, saveInfo);
        }
        internal static void DeleteCurrent()
        {
            string[] filePaths = Directory.GetFiles(CurrentSavePath);
            foreach (string filePath in filePaths)
            {
                if (filePath.EndsWith(".sav"))
                    DeleteFile(filePath);
            }
        }
        internal static void DeleteCurrentFile(string filename)
        {
            DeleteFile(CurrentSavePath + filename + ".sav");
        }

        internal static void LoadSavegame(string savename)
        {
            DeleteCurrent();
            foreach (string file in GetSaveFiles(savename))
            {
                string fileName = Path.GetFileName(file);
                string destinationFile = Path.Combine(CurrentSavePath, fileName);
                File.Copy(file, destinationFile);
            }
        }
    }
}
