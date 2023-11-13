using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace CustomUtilities
{
    public static class FileHandler {

        public static void SaveToJson<T> (List<T> _toSave, string _filename) {
            Debug.Log (GetPath (_filename));
            string content = JsonHelper.ToJson<T> (_toSave.ToArray ());
            WriteFile (GetPath (_filename), content);
        }

        public static void SaveToJson<T> (T _toSave, string _filename) {
            string content = JsonUtility.ToJson (_toSave);
            WriteFile (GetPath (_filename), content);
        }

        public static List<T> ReadListFromJson<T> (string _filename, bool _readFromResources = false) {
            string content = GetContent(_filename, _readFromResources);

            if (string.IsNullOrEmpty (content) || content == "{}") {
                return new List<T> ();
            }

            List<T> res = JsonHelper.FromJson<T>(content).ToList ();

            return res;
        }

        public static T ReadFromJson<T> (string _filename, bool _readFromResources = false)
        {
            string content = GetContent(_filename, _readFromResources);

            if (string.IsNullOrEmpty (content) || content == "{}") {
                return default (T);
            }

            T res = JsonUtility.FromJson<T>(content);

            return res;
        }

        private static string GetContent(string _filename, bool _readFromResources = true)
        {
            if (!_readFromResources) return ReadFile(GetPath(_filename));
        
            var dataToParse = Resources.Load<TextAsset>("PlayerData/" + _filename);
            return dataToParse.text;
        }

        private static string GetPath (string _filename) {
            return Application.persistentDataPath + "/" + _filename;
        }

        private static void WriteFile (string _path, string _content) {
            FileStream fileStream = new FileStream (_path, FileMode.Create);

            using (StreamWriter writer = new StreamWriter (fileStream)) {
                writer.Write (_content);
            }
        }

        private static string ReadFile (string _path) {
            if (File.Exists (_path)) {
                using (StreamReader reader = new StreamReader (_path)) {
                    string content = reader.ReadToEnd ();
                    return content;
                }
            }
            return "";
        }
    }

    public static class JsonHelper {
        public static T[] FromJson<T> (string _json) {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>> (_json);
            return wrapper.Items;
        }

        public static string ToJson<T> (T[] _array) {
            Wrapper<T> wrapper = new Wrapper<T> ();
            wrapper.Items = _array;
            return JsonUtility.ToJson (wrapper);
        }

        public static string ToJson<T> (T[] _array, bool _prettyPrint) {
            Wrapper<T> wrapper = new Wrapper<T> ();
            wrapper.Items = _array;
            return JsonUtility.ToJson (wrapper, _prettyPrint);
        }

        [Serializable]
        private class Wrapper<T> {
            public T[] Items;
        }
    }
}