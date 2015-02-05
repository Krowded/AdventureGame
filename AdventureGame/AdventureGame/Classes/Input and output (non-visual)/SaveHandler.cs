using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AdventureGame
{
    static class SaveHandler
    {
        public static string CurrentSave = "Savefiles/Current/";
        public static string SaveDirectory = "Savefiles/Savegames/";

        public static void Save(string savename)
        {
            string[] filePaths = Directory.GetFiles(CurrentSave);

            DeleteDirectory(savename);

            CreateDirectory(savename);

            foreach (string filePath in filePaths)
            {
                File.Move(filePath, SaveDirectory + savename);
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

        public static void DeleteCurrent()
        {
            string[] filePaths = Directory.GetFiles(CurrentSave);
            foreach (string filePath in filePaths)
            {
                DeleteFile(filePath);
            }
        }

        public static void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        public static List<string> GetSaveFiles()
        {
            string[] filePaths = Directory.GetDirectories(SaveDirectory);
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
            foreach (string file in GetSaveFiles())
            {
                string fileName = Path.GetFileName(file);
                string destinationFile = Path.Combine(CurrentSave, fileName);
                File.Copy(file, destinationFile);
            }
        }
    }
}
