using System;
using UnityEngine;

namespace Game
{
    public class UIContext : MonoBehaviour
    {
        public static UIContext Singleton { get; private set; }

        [SerializeField] private MemoryMatchCardPresenter m_memoryMatch;
        [SerializeField] private SlidingPuzzlePresenter m_slidingPuzzle;
        
        public MemoryMatchCardPresenter MemoryMatch => m_memoryMatch;
        public SlidingPuzzlePresenter SlidingPuzzle => m_slidingPuzzle;
        
        public event Action InteractFinish;

        private void Awake()
        {
            Singleton = this;

            m_memoryMatch.InteractFinish += HandleInteractFinish;
            m_slidingPuzzle.InteractFinish += HandleInteractFinish;
        }

        private void HandleInteractFinish()
        {
            InteractFinish?.Invoke();
        }
    }
}