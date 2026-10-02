using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class EncryptSaveDecorator : ISaveLoad
    {
        private string GetFilePath(string key)
        {
            string dir = Path.Combine(Application.persistentDataPath, "Saves");

            // ✅ pastikan folder ada
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return Path.Combine(dir, $"{key}.sav");
        }

        public bool Exist(string key) => File.Exists(GetFilePath(key));

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
            json = Encrypt(json);
            
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
                json = Decrypt(json);

                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"Load failed: {e}");
                return default;
            }
        }

        // ================= ENCRYPTION =================
        

        private string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            using var aes = Aes.Create();
            aes.Key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(SystemInfo.deviceUniqueIdentifier));
            aes.GenerateIV();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor();

            byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encrypted = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

            byte[] result = new byte[aes.IV.Length + encrypted.Length];
            Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
            Buffer.BlockCopy(encrypted, 0, result, aes.IV.Length, encrypted.Length);

            return Convert.ToBase64String(result);
        }

        private string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            try
            {
                byte[] fullData = Convert.FromBase64String(cipherText);

                using var aes = Aes.Create();
                aes.Key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(SystemInfo.deviceUniqueIdentifier));
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                byte[] iv = new byte[16];
                byte[] encrypted = new byte[fullData.Length - 16];

                Buffer.BlockCopy(fullData, 0, iv, 0, 16);
                Buffer.BlockCopy(fullData, 16, encrypted, 0, encrypted.Length);

                aes.IV = iv;

                using var decryptor = aes.CreateDecryptor();
                byte[] decrypted = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);

                return Encoding.UTF8.GetString(decrypted);
            }
            catch (Exception e)
            {
                Debug.LogError($"Decrypt failed: {e}");
                return null;
            }
        }
    }
}