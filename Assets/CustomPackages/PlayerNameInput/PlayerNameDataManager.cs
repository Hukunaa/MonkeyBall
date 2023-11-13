using System;
using System.IO;
using UnityEngine;

namespace PlayerNameInput
{
    public static class PlayerNameDataManager
    {
        private static string PersistentBasePath => Application.persistentDataPath + "/Data";
        private static string PlayerDataPath => "PlayerData";
        private static string PersistentPlayerDataPath => Path.Combine(PersistentBasePath, PlayerDataPath);
        private static string PlayerNameFileName => "PlayerName.json";

        public static string LoadPlayerName()
        {
            string filePath = Path.Combine(PersistentPlayerDataPath, PlayerNameFileName);
            if (File.Exists(filePath) == false)
            {
                Debug.Log("No Player name data to load");
                return "";
            }
            string jsonData = File.ReadAllText(filePath);
            PlayerNameParser parseData = JsonUtility.FromJson<PlayerNameParser>(jsonData);
            Debug.Log($"Loaded Player name: {parseData.playerName}");
            return parseData.playerName;
        }
        
        public static void SavePlayerName(string _playerName)
        {
            string filePath = Path.Combine(PersistentPlayerDataPath, PlayerNameFileName);

            PlayerNameParser data = new PlayerNameParser(_playerName);
            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, jsonData);
            Debug.Log($"Saved Player name: {_playerName}");
        }

        public static void DeletePlayerNameData()
        {
            string filePath = Path.Combine(PersistentPlayerDataPath, PlayerNameFileName);
            File.Delete(filePath);
            Debug.Log("Player name data deleted.");
        }
        
        [Serializable]
        public class PlayerNameParser
        {
            public string playerName;

            public PlayerNameParser(string _playerName)
            {
                playerName = _playerName;
            }
        }
    }
}