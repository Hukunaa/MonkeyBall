using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

namespace Utilities
{
    public class DataVersionParser
    {
        public string version;

        public DataVersionParser(string _version)
        {
            version = _version;
        }
    }

    public class PlayerNameParser
    {
        public string playerName;

        public PlayerNameParser(string _playerName)
        {
            playerName = _playerName;
        }
    }
    
    [Serializable]
    public class CurrenciesDataParser
    {
        public int softCurrency;
        public int hardCurrency;

        public CurrenciesDataParser(int _softCurrency, int _hardCurrency)
        {
            softCurrency = _softCurrency;
            hardCurrency = _hardCurrency;
        }
    }

    [Serializable]
    public class ScoreDataParser
    {
        public int playerScore;
        public int playerWins;

        public ScoreDataParser(int _score, int _wins)
        {
            playerScore = _score;
            playerWins = _wins;
        }
    }
    [Serializable]
    public class RewardsDataParser
    {
        public bool[] data;
        public RewardsDataParser(bool[] _data)
        {
            data = _data;
        }
    }

    [Serializable]
    public class BattlePassDataParser
    {
        public int xp;
        public BattlePassDataParser(int _xp)
        {
            xp = _xp;
        }
    }

    public class SkinsDataParser
    {
        public List<string> skins;

        public SkinsDataParser(string[] _skins)
        {
            skins = _skins.ToList();
        }
    }

    [Serializable]
    public class PlayerSkinDataParser
    {
        public string _headSkin;
        public string _bodySkin;
        public string _ballSkin;
        public string _gliderSkin;
        public string _faceEmotion;
        public string _skinColor;

        public PlayerSkinDataParser(string _head, string _body, string _ball, string _glider, string _face, string _color)
        {
            _headSkin = _head;
            _bodySkin = _body;
            _gliderSkin = _glider;
            _ballSkin = _ball;
            _faceEmotion = _face;
            _skinColor = _color;
        }
    }

    public static class DataLoader
    {
        private static string PersistentBasePath => Application.persistentDataPath + "/Data";
        private static string PlayerDataPath => "PlayerData";
        private static string StoreDataPath => "StoreData";
        private static string DataVersionFileName => "DataVersion.json";
        private static string PlayerCurrenciesFileName => "PlayerCurrencies.json";
        private static string PlayerScoreFileName => "PlayerScore.json";
        private static string PlayerRewardsFileName => "PlayerRewards.json";
        private static string PlayerBattlePassFileName => "PlayerBattlePass.json";
        private static string PlayerSkinsInventoryFileName => "PlayerSkinsInventory.json";
        private static string PlayerCurrentSkinsFileName => "PlayerCurrentSkins.json";
        private static string StoreSkinsFileName => "StoreSkins.json";
        private static string PersistentPlayerDataPath => Path.Combine(PersistentBasePath, PlayerDataPath);
        private static string PersistentStoreDataPath => Path.Combine(PersistentBasePath, StoreDataPath);
        public static string PersistentStoreSkinsDataPath => Path.Combine(PersistentStoreDataPath, StoreSkinsFileName);
        public static string PersistentPlayerSkinsInventoryDataPath => Path.Combine(PersistentPlayerDataPath, PlayerSkinsInventoryFileName);

        //TEMPORARY UNTIL WE GO SERVER SIDE
        public static void CopyFromResourcesToPersistent()
        {
            if (!ShouldReplaceData())
            {
                Debug.Log("Data doesn't need to be replaced");
                return;
            }
            Debug.Log("Data need to be replaced");
            ResetAllData();
        }

        public static void ResetAllData()
        {
            ResetDirectories();
            SaveDataVersion(LoadDataVersion(true));
            ResetPlayerScoreData();
            ResetPlayerBattlePassData();
            ResetPlayerCurrenciesData();
            ResetPlayerSkinsInventoryData();
            ResetStoreSkinsData();
            ResetPlayerCurrentSkinsData();
        }

        private static void ResetDirectories()
        {
            if (Directory.Exists(PersistentBasePath))
            {
                Directory.Delete(PersistentBasePath, true);
            }

            Directory.CreateDirectory(PersistentBasePath);
            Directory.CreateDirectory(PersistentPlayerDataPath);
            Directory.CreateDirectory(PersistentStoreDataPath);
        }

        public static void ResetPlayerScoreData()
        {
            CopyFromResourcesToPersistent(PlayerDataPath, PersistentPlayerDataPath, PlayerScoreFileName);
            Debug.Log("Player score data restored to default.");
        }

        public static void ResetPlayerBattlePassData()
        {
            CopyFromResourcesToPersistent(PlayerDataPath, PersistentPlayerDataPath, PlayerBattlePassFileName);
            Debug.Log("Player Battle Pass data restored to default.");
        }

        public static void ResetPlayerRewardsData()
        {
            CopyFromResourcesToPersistent(PlayerDataPath, PersistentPlayerDataPath, PlayerRewardsFileName);
            Debug.Log("Player rewards data restored to default.");
        }

        public static void ResetPlayerCurrenciesData()
        {
            CopyFromResourcesToPersistent(PlayerDataPath, PersistentPlayerDataPath, PlayerCurrenciesFileName);
            Debug.Log("Player Currencies data restored to default.");
        }
        

        public static void ResetPlayerSkinsInventoryData()
        {
            CopyFromResourcesToPersistent(PlayerDataPath, PersistentPlayerDataPath, PlayerSkinsInventoryFileName);
            Debug.Log("Player Skins Inventory data restored to default.");
        }

        public static void ResetPlayerCurrentSkinsData()
        {
            CopyFromResourcesToPersistent(PlayerDataPath, PersistentPlayerDataPath, PlayerCurrentSkinsFileName);
            Debug.Log("Player Current Skins data restored to default.");
        }

        public static void ResetStoreSkinsData()
        {
            CopyFromResourcesToPersistent(StoreDataPath, PersistentStoreDataPath, StoreSkinsFileName);
            Debug.Log("Store Skins data restored to default.");
        }
        
        private static void CopyFromResourcesToPersistent(string _resourcesDataPath, string _persistentPath, string _filename)
        {
            string resourcePath = $"{_resourcesDataPath}/{Path.GetFileNameWithoutExtension(_filename)}";
            TextAsset storeSkins = Resources.Load<TextAsset>(resourcePath);
            if (_filename != null)
            {
                var path = Path.Combine(_persistentPath, _filename);
                
                if (!Directory.Exists(_persistentPath))
                {
                    Directory.CreateDirectory(_persistentPath);
                }
                
                File.WriteAllText(path, storeSkins.text);
            }
        }
        
        private static bool ShouldReplaceData()
        {
            var replaceData = false;
            if (!Directory.Exists(PersistentBasePath))
            {
                Debug.Log($"Replace data because there is no Persistent Base Path.");
                replaceData = true;
            }
            
            else if (!File.Exists(Path.Combine(PersistentBasePath, DataVersionFileName)))
            {
                Debug.Log($"Replace data because there is no Data version file.");
                replaceData = true;
            }

            else
            {
                var currentVersion = LoadDataVersion();
                string lastVersion = LoadDataVersion(true);
                if (currentVersion != lastVersion)
                {
                    Debug.Log($"Replace data because Data version is in persistent file is different than the one in Resources.");
                    replaceData = true;
                }
            }

            return replaceData;
        }

        public static string LoadDataVersion(bool _loadFromResources = false)
        {
            if (_loadFromResources)
            {
                TextAsset dataVersion =
                    Resources.Load<TextAsset>(Path.GetFileNameWithoutExtension(DataVersionFileName));
                return JsonUtility.FromJson<DataVersionParser>(dataVersion.text).version;
            }

            string filePath = Path.Combine(PersistentBasePath, DataVersionFileName);
            if (!File.Exists(filePath))
                return null;

            string data = File.ReadAllText(filePath);
            DataVersionParser versionData = JsonUtility.FromJson<DataVersionParser>(data);

            return versionData.version;
        }

        private static void SaveDataVersion(string _version)
        {
            string filePath = Path.Combine(PersistentBasePath, DataVersionFileName);

            if (!Directory.Exists(PersistentBasePath))
                Directory.CreateDirectory(PersistentBasePath);

            DataVersionParser data = new DataVersionParser(_version);
            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, jsonData);
        }

        public static int LoadPlayerBattlePassXp()
        {
            string filePath = Path.Combine(PersistentPlayerDataPath, PlayerBattlePassFileName);
            if (!File.Exists(filePath))
                SavePlayerBattlePassXp(0);

            string jsonData = File.ReadAllText(filePath);
            BattlePassDataParser parseData = JsonUtility.FromJson<BattlePassDataParser>(jsonData);
            return parseData.xp;
        }

        public static void SavePlayerBattlePassXp(int _xp)
        {
            string filePath = Path.Combine(PersistentPlayerDataPath, PlayerBattlePassFileName);

            BattlePassDataParser data = new BattlePassDataParser(_xp);
            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, jsonData);
        }

        public static List<string> LoadPlayerCurrentSkins()
        {
            string filePath = Path.Combine(PersistentPlayerDataPath, PlayerCurrentSkinsFileName);

            if (!File.Exists(filePath))
                ResetPlayerCurrentSkinsData();

            string jsonData = File.ReadAllText(filePath);
            PlayerSkinDataParser parseData = JsonUtility.FromJson<PlayerSkinDataParser>(jsonData);
            return new List<string>() { parseData._headSkin, parseData._bodySkin, parseData._ballSkin, parseData._gliderSkin, parseData._faceEmotion, parseData._skinColor };
        }

        public static void SavePlayerCurrentSkins(List<string> _skins)
        {
            string filePath = Path.Combine(PersistentPlayerDataPath, PlayerCurrentSkinsFileName);

            PlayerSkinDataParser data = new PlayerSkinDataParser(_skins[0], _skins[1], _skins[2], _skins[3], _skins[4], _skins[5]);
            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, jsonData);
        }
        
        public static bool[] LoadPlayerRewardsState()
        {
            string filePath = Path.Combine(PersistentPlayerDataPath, PlayerRewardsFileName);
            if (!File.Exists(filePath))
            {
                SavePlayerRewardsState(new bool[] { false });
            }

            string jsonData = File.ReadAllText(filePath);
            RewardsDataParser parseData = JsonUtility.FromJson<RewardsDataParser>(jsonData);

            if (parseData == null)
                return null;

            return parseData.data;
        }

        public static void SavePlayerRewardsState(bool[] _data)
        {
            string filePath = Path.Combine(PersistentPlayerDataPath, PlayerRewardsFileName);

            RewardsDataParser data = new RewardsDataParser(_data);
            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, jsonData);
        }

        public static int[] LoadCurrencies()
        {
            string filePath = Path.Combine(PersistentPlayerDataPath, PlayerCurrenciesFileName);
            if (!File.Exists(filePath))
                SaveCurrencies(0, 0);

            string data = File.ReadAllText(filePath);
            CurrenciesDataParser currenciesData = JsonUtility.FromJson<CurrenciesDataParser>(data);
            int[] currencies = new int[2] { 0, 0 };

            currencies[0] = currenciesData.softCurrency;
            currencies[1] = currenciesData.hardCurrency;

            return currencies;
        }

        public static void SaveCurrencies(int _softCurrency, int _hardCurrency)
        {
            string filePath = Path.Combine(PersistentPlayerDataPath, PlayerCurrenciesFileName);

            if (!Directory.Exists(PersistentPlayerDataPath))
                Directory.CreateDirectory(PersistentPlayerDataPath);

            CurrenciesDataParser data = new CurrenciesDataParser(_softCurrency, _hardCurrency);
            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, jsonData);
        }

        public static int[] LoadScore()
        {
            string filePath = Path.Combine(PersistentPlayerDataPath, PlayerScoreFileName);
            if (!File.Exists(filePath))
                SaveScore(0, 0);

            string data = File.ReadAllText(filePath);
            ScoreDataParser scoreData = JsonUtility.FromJson<ScoreDataParser>(data);

            return new int[]{ scoreData.playerScore, scoreData.playerWins };
        }

        public static void SaveScore(int _score, int _wins)
        {
            string filePath = Path.Combine(PersistentPlayerDataPath, PlayerScoreFileName);

            if (!Directory.Exists(PersistentPlayerDataPath))
                Directory.CreateDirectory(PersistentPlayerDataPath);

            ScoreDataParser data = new ScoreDataParser(_score, _wins);
            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, jsonData);
        }

        public static List<string> LoadSkins(string _path)
        {
            string data = File.ReadAllText(_path);
            SkinsDataParser skins = JsonUtility.FromJson<SkinsDataParser>(data);

            return skins.skins;
        }

        public static void SaveSkins(string _path, string[] _skins)
        {
            string filePath = Path.Combine(PersistentBasePath, _path);

            SkinsDataParser data = new SkinsDataParser(_skins);
            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, jsonData);
        }
    }
}
