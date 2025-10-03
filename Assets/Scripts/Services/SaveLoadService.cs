using UnityEngine;
using System.IO;
using Services.Interfaces;

namespace Services
{
    public class SaveLoadService : ISaveLoadService
    {
        private string GetPath(string key)
        {
            return Path.Combine(Application.persistentDataPath, key + ".json");
        }

        public void Save<T>(string key, T data)
        {
            string path = GetPath(key);
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(path, json);
        }

        public T Load<T>(string key)
        {
            string path = GetPath(key);
            if (!File.Exists(path))
            {
                return default;
            }

            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(json);
        }

        public bool HasKey(string key)
        {
            string path = GetPath(key);
            return File.Exists(path);
        }

        public void DeleteKey(string key)
        {
            string path = GetPath(key);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}