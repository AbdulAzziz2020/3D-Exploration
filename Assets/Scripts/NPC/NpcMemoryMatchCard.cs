using System;
using Game;
using UnityEngine;

namespace NPC
{
    public sealed class NpcMemoryMatchCard : MonoBehaviour, IInteractable
    {
        [Serializable]
        public struct CardData
        {
            public int pairId;
            public Sprite sprite;
        }

        [Header("Cards")]
        [SerializeField] private CardData[] m_cards;

        public bool CanInteract => m_isInteracting;

        private bool m_isInteracting = true;
        
        public async void Interact()
        {
            try
            {
                m_isInteracting = false;

                UserModel l_model = await PlayerSave.Singleton.GetAsync();
                UIContext.Singleton.MemoryMatch.Setup(m_cards, l_model.MemoryMatch);
            }
            finally
            {
                m_isInteracting = true;
            }
        }
    }
}