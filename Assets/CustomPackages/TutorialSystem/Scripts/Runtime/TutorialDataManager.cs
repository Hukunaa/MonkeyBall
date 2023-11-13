using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TutorialSystem.Scripts.Runtime
{
    public static class TutorialDataManager
    {
        private static string PersistentTutorialDataPath => Application.persistentDataPath + TutorialDataPath;
        private static string TutorialDataPath => "/data/TutorialData";
        private static string TutorialDataFileName => "TutorialData";

        private static List<string> _completedTutorial;
        
        public static bool IsTutorialComplete(string _tutorialName)
        {
            _completedTutorial ??= LoadList(PersistentTutorialDataPath, TutorialDataFileName);

            return _completedTutorial.Contains(_tutorialName);
        }
        
        public static void TutorialComplete(string _tutorialName)
        {
            _completedTutorial ??= LoadList(PersistentTutorialDataPath, TutorialDataFileName);
            
            if(_completedTutorial.Contains(_tutorialName) == false)
            {
                _completedTutorial.Add(_tutorialName);
                SaveList(PersistentTutorialDataPath, TutorialDataFileName, _completedTutorial);
            }
        }
        
        private static List<string> LoadList(string _persistentPath, string _filename)
        {
            string filePath = Path.Combine(_persistentPath, _filename);

            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"There is no file name {_filename} at {_persistentPath}");
                return new List<string>();
            }
            
            string data = File.ReadAllText(filePath);
            ListDataParser listData = JsonUtility.FromJson<ListDataParser>(data);
            return listData.items;
        }

        private static void SaveList(string _persistentPath, string _filename,  List<string> _list)
        {
            string filePath = Path.Combine(_persistentPath, _filename);

            if (Directory.Exists(_persistentPath) == false)
            {
                Directory.CreateDirectory(_persistentPath);
                Debug.Log($"Created directory at {_persistentPath}.");
            }
            
            ListDataParser data = new ListDataParser(_list);
            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, jsonData);
            Debug.Log($"Wrote {_list.ToString()} to {_filename} at {_persistentPath}");
        }

        public static void DeleteTutorialData()
        {
            if (Directory.Exists(PersistentTutorialDataPath) == false)
            {
                Debug.Log("No Tutorial Data to delete.");
                return;
            }
            
            Directory.Delete(PersistentTutorialDataPath, true);
            Debug.Log("Tutorial Data deleted.");
        }
        
        [Serializable]
        public class ListDataParser
        {
            public List<string> items;

            public ListDataParser(List<string> _items)
            {
                items = _items;
            }
        }
    }
}