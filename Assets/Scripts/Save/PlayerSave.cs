using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public sealed class PlayerSave : MonoBehaviour
    {
        private const string PLAYER_SAVE_KEY = "player";

        [SerializeReference]
        private ISaveLoad m_save;

        [SerializeField] private UserModel m_model;

        public static PlayerSave Singleton { get; private set; }
        
        private void Awake()
        {
            if (Singleton != null && Singleton != this)
            {
                Destroy(gameObject);
                return;
            }

            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }

        public async UniTask<UserModel> GetAsync()
        {
            if (m_model != null && !string.IsNullOrEmpty(m_model.Id))
                return m_model;

            m_model = await GetOrCreateAsync();

            return m_model;
        }

        public UniTask SaveAsync()
        {
            if (m_model == null)
                return UniTask.CompletedTask;

            return m_save.SaveAsync(PLAYER_SAVE_KEY, m_model);
        }

        private async UniTask<UserModel> GetOrCreateAsync()
        {
            if (m_save.Exist(PLAYER_SAVE_KEY))
            {
                UserModel l_model = await m_save.LoadAsync<UserModel>(PLAYER_SAVE_KEY);

                if (l_model != null)
                    return l_model;
            }

            UserModel l_newModel = Create();

            await m_save.SaveAsync(PLAYER_SAVE_KEY, l_newModel);

            return l_newModel;
        }

        private UserModel Create() => new UserModel(Guid.NewGuid().ToString());
    }
}