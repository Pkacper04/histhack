using System.IO;
using UnityEngine;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Histhack.Core;

namespace Histhack.Core.SaveLoadSystem
{
    public static class SaveSystem
    {
        private const string savingFolder = "Saves/";
        private const string ending = ".xml";

        private static string absoluteSavingPath = Application.persistentDataPath + "/" + savingFolder;
        private static string coreSavingPath = Application.persistentDataPath + "/";

        #region LoadTypes

        private static string GetRawData(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            CryptoStream oStream = MainGameController.Instance.DataManager.Encrypter.CreateDecryptoCryptoStream(fs, CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(oStream);

            string rawData = reader.ReadToEnd();

            reader.Close();
            oStream.Close();
            fs.Close();

            return rawData;
        }

        public static T Load<T>(string savingPath, T ifFileNotExistValue, SaveDirectories additionalSaveDirectory = SaveDirectories.None)
        {
            string fullSavingPath = GetAddictionalDirectory(additionalSaveDirectory) + savingPath;

            if (!CheckIfFileExists(savingPath, additionalSaveDirectory))
                return ifFileNotExistValue;

            string path = CreatePath(fullSavingPath, additionalSaveDirectory);

            T data = JsonConvert.DeserializeObject<T>(GetRawData(path));

            return data;
        }


        public static T LoadClass<T>(string savingPath, SaveDirectories additionalSaveDirectory = SaveDirectories.None) where T : class
        {
            string fullSavingPath = GetAddictionalDirectory(additionalSaveDirectory) + savingPath;

            if (!CheckIfFileExists(savingPath, additionalSaveDirectory))
                return null;

            string path = CreatePath(fullSavingPath, additionalSaveDirectory);

            T data = JsonConvert.DeserializeObject<T>(GetRawData(path));

            return data;
        }

        #endregion LoadTypes


        #region SaveTypes

        public static void Save<T>(T savingValue, string savingPath, SaveDirectories additionalSaveDirectory = SaveDirectories.None)
        {
            string fullSavingPath = GetAddictionalDirectory(additionalSaveDirectory) + savingPath;

            string path = CreatePath(fullSavingPath,additionalSaveDirectory);
            CreateDirectoryForSaves(path);

            FileStream fs = new FileStream(path, FileMode.Create);
            CryptoStream iStream = MainGameController.Instance.DataManager.Encrypter.CreateEncryptoCryptoStream(fs, CryptoStreamMode.Write);
            StreamWriter sWriter = new StreamWriter(iStream);

            string JsonFile = JsonConvert.SerializeObject(savingValue);
            sWriter.Write(JsonFile);

            sWriter.Close();
            iStream.Close();
            fs.Close();
        }


        public static void SaveClass<T>(T savingValue, string savingPath, SaveDirectories additionalSaveDirectory = SaveDirectories.None) where T : new()
        {
            string fullSavingPath = GetAddictionalDirectory(additionalSaveDirectory) + savingPath;

            string path = CreatePath(fullSavingPath, additionalSaveDirectory);
            CreateDirectoryForSaves(path);

            FileStream fs = new FileStream(path, FileMode.Create);
            CryptoStream iStream = MainGameController.Instance.DataManager.Encrypter.CreateEncryptoCryptoStream(fs, CryptoStreamMode.Write);
            StreamWriter sWriter = new StreamWriter(iStream);

            T customData = new T();
            customData = savingValue;
            string JsonFile = JsonConvert.SerializeObject(customData);
            sWriter.Write(JsonFile);

            sWriter.Close();
            iStream.Close();
            fs.Close();
        }


        #endregion SaveTypes

        public static bool CheckIfFileExists(string savingPath, SaveDirectories additionalSaveDirectory)
        {
            string fullSavingPath = GetAddictionalDirectory(additionalSaveDirectory) + savingPath;

            string path = CreatePath(fullSavingPath,additionalSaveDirectory);
            return File.Exists(path);
        }

        public static void DeleteAllSaves()
        {
            if(Directory.Exists(absoluteSavingPath))
            {
                Directory.Delete(absoluteSavingPath, true);
            }
        }

        public static void DeleteOneSave(string savingPath)
        {
            string path = CreatePath(savingPath, SaveDirectories.None);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public static bool CheckIfSaveExists()
        {
            string fixedPath = absoluteSavingPath.Remove(absoluteSavingPath.Length - 1, 1);
            return Directory.Exists(fixedPath);
        }

        public static string GetAddictionalDirectory(SaveDirectories directory)
        {
            switch (directory)
            {
                case SaveDirectories.Level:
                    return "CurrentLevelData/";
                case SaveDirectories.Player:
                    return "PlayerData/";
                case SaveDirectories.Collectibles:
                    return "CollectiblesData/";
                case SaveDirectories.Core:
                    return "CoreValues/";
                default:
                    return "";
            }
        }


        private static void CreateDirectoryForSaves(string path)
        {
            int lastIndicator = path.LastIndexOf("/");
            int numberOfWordsToDelete = (path.Length - lastIndicator);
            string newPath = path.Remove(lastIndicator, numberOfWordsToDelete);

            if (Directory.Exists(newPath))
                return;

            Directory.CreateDirectory(newPath);
        }

        private static string CreatePath(string rawPath, SaveDirectories saveDirectiory)
        {
            string finalPath = "";

            if (saveDirectiory != SaveDirectories.Core)
                finalPath = absoluteSavingPath + rawPath + ending;
            else
                finalPath = coreSavingPath + rawPath + ending;

            return finalPath;
        }
    }

    public enum SaveDirectories
    {
        None,
        Level,
        Player,
        Collectibles,
        Core
    }
}
