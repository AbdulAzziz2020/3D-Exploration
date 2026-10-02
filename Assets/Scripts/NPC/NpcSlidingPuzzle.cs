using System;
using UnityEngine;

namespace Game
{
    public sealed class NpcSlidingPuzzle : MonoBehaviour, IInteractable
    {
        [SerializeField, Min(1)] private int m_shuffleMoves = 100;

        public bool CanInteract => m_isInteracting;
        
        private bool m_isInteracting = true;
        
        public async void Interact()
        {
            try
            {
                m_isInteracting = false;

                UserModel l_model = await PlayerSave.Singleton.GetAsync();
                UIContext.Singleton.SlidingPuzzle.Setup(m_shuffleMoves, l_model.SlidingPuzzle);
            }
            finally
            {
                m_isInteracting = true;
            }
            
        }
    }
}