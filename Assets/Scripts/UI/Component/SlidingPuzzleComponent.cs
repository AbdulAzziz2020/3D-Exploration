using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public sealed class SlidingPuzzleComponent :
        UIComponent,
        IPointerClickHandler
    {
        [SerializeField] private TMP_Text m_text;
        [SerializeField] private GameObject m_view;

        private int m_index;
        private int m_tileId;

        private bool m_canInteract;

        public int Index => m_index;
        public int TileId => m_tileId;

        public event Action<SlidingPuzzleComponent> Clicked;

        public void Setup(int p_index, int p_tileId)
        {
            m_index = p_index;

            SetTile(p_tileId);
        }

        public void SetTile(int p_tileId)
        {
            m_tileId = p_tileId;

            bool l_isEmpty = p_tileId == 0;

            m_text.gameObject.SetActive(!l_isEmpty);
            m_view.SetActive(!l_isEmpty);

            if (!l_isEmpty)
                m_text.text = p_tileId.ToString();
        }

        public void SetInteractable(bool p_value)
        {
            m_canInteract = p_value;
        }

        public void OnPointerClick(
            PointerEventData p_eventData)
        {
            if (!m_canInteract)
                return;

            if (m_tileId == 0)
                return;

            Clicked?.Invoke(this);
        }
    }
}