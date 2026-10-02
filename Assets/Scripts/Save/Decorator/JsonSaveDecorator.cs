using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class JsonSaveDecorator : ISaveLoad
    {
        private string GetFilePath(string key)
        {
            string dir = Path.Combine(Application.persistentDataPath, "Saves");

            // ✅ pastikan folder ada
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return Path.Combine(dir, $"{key}.sav");
        }

        public bool Exist(string key)
            => File.Exists(GetFilePath(key));

        public void Delete(string key)
        {
            string path = GetFilePath(key);
            if (File.Exists(path))
                File.Delete(path);
        }

        public async UniTask SaveAsync<T>(string key, T data)
        {
            string path = GetFilePath(key);
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            
            try
            {
                await File.WriteAllTextAsync(path, json);
                Debug.Log("Save successful, in path: " + path);
            }
            catch (Exception e)
            {
                Debug.LogError($"Save failed: {e}");
            }
        }

        public async UniTask<T> LoadAsync<T>(string key)
        {
            string path = GetFilePath(key);

            if (!File.Exists(path))
            {
                Debug.LogWarning("File not found: " + path);
                return default;
            }

            try
            {
                string json = await File.ReadAllTextAsync(path);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"Load failed: {e}");
                return default;
            }
        }
    }
}