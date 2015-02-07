using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AdventureGame
{
    static class SaveHandler
    {
        public static string CurrentSavePath = "Savefiles/Current/";
        private static string SaveDirectory = "Savefiles/Savegames/";

        public static void Save(string savename)
        {
            string[] filePaths = Directory.GetFiles(CurrentSavePath);

            DeleteDirectory(savename);

            CreateDirectory(savename);

            foreach (string filePath in filePaths)
            {
                File.Move(filePath, SaveDirectory + savename);
            }
        }

        //To display alternatives when loading/overwriting
        public static List<string> GetSavegames()
        {
            List<string> list = new List<string>();
            foreach (string directory in Directory.GetDirectories(SaveDirectory))
            {
                list.Add(Path.GetDirectoryName(directory));
            }
            return list;
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

        public static void DeleteCurrent()
        {
            string[] filePaths = Directory.GetFiles(CurrentSavePath);
            foreach (string filePath in filePaths)
            {
                if(filePath.EndsWith(".sav"))
                    DeleteFile(filePath);
            }
        }

        public static void DeleteFile(string filePath)
        {
            File.Delete(filePath);
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

        public static void Load(string savename)
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
