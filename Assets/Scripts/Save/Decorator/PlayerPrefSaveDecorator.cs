using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class PlayerPrefSaveDecorator : ISaveLoad
    {
        private string GetFilePath(string key) => "";

        public bool Exist(string key) => PlayerPrefs.HasKey(key);

        public void Delete(string key)
        {
            string json = PlayerPrefs.GetString(key);
            if(string.IsNullOrEmpty(json)) 
                return;
            
            PlayerPrefs.DeleteKey(key);
        }

        public UniTask SaveAsync<T>(string key, T data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            
            try
            {
                PlayerPrefs.SetString(key, json);
                return UniTask.CompletedTask;
            }
            catch (Exception e)
            {
                Debug.LogError($"Save failed: {e}");
                return UniTask.FromException(e);
            }
        }

        public UniTask<T> LoadAsync<T>(string key)
        {
            if (!Exist(key))
            {
                return default;
            }

            try
            {
                string json = PlayerPrefs.GetString(key);
                T data = JsonConvert.DeserializeObject<T>(json);
                return UniTask.FromResult(data);
            }
            catch (Exception e)
            {
                Debug.LogError($"Load failed: {e}");
                return default;
            }
        }
    }
}