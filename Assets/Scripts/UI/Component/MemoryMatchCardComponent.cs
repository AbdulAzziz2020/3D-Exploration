using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public sealed class MemoryMatchCardComponent : UIComponent, IPointerClickHandler
    {
        [Header("Visual")]
        [SerializeField] private GameObject m_view;
        [SerializeField] private GameObject m_front;
        [SerializeField] private GameObject m_back;
        [SerializeField] private Image m_image;

        private int m_index;
        private int m_pairId;
        private Sprite m_sprite;

        private bool m_isRevealed;
        private bool m_isCompleted;
        private bool m_canInteract;

        public int Index => m_index;
        public int PairId => m_pairId;

        public bool IsRevealed => m_isRevealed;
        public bool IsCompleted => m_isCompleted;
        public Sprite Sprite => m_sprite;

        public event Action<MemoryMatchCardComponent> Clicked;

        public void Setup(
            int p_index,
            int p_pairId,
            Sprite p_sprite)
        {
            m_index = p_index;
            m_pairId = p_pairId;
            m_sprite = p_sprite;

            m_image.sprite = p_sprite;

            ResetCard();
        }

        public void Reveal()
        {
            if (m_isCompleted)
                return;

            m_isRevealed = true;

            m_front.SetActive(true);
            m_back.SetActive(false);
        }

        public void Hidden()
        {
            if (m_isCompleted)
                return;

            m_isRevealed = false;

            m_front.SetActive(false);
            m_back.SetActive(true);
        }

        public void Complete()
        {
            m_isCompleted = true;
            m_isRevealed = true;
            m_canInteract = false;

            // Hide only the visual.
            // Component itself remains active so GridLayoutGroup
            // keeps the slot.
            m_view.SetActive(false);
        }

        public void SetInteractable(bool p_value)
        {
            m_canInteract = p_value;
        }

        public void ResetCard()
        {
            m_isRevealed = false;
            m_isCompleted = false;
            m_canInteract = true;

            m_view.SetActive(true);
            m_front.SetActive(false);
            m_back.SetActive(true);
        }

        public void OnPointerClick(PointerEventData p_eventData)
        {
            if (!m_canInteract)
                return;

            if (m_isRevealed || m_isCompleted)
                return;

            Clicked?.Invoke(this);
        }
    }
}