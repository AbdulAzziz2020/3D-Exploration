using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public sealed class SlidingPuzzleView : UIView
    {
        [Header("Default")]
        [SerializeField] private Button m_closeButton;
        
        [Header("Tiles")]
        [SerializeField] private GameObject m_boardPanel;
        [SerializeField] private SlidingPuzzleComponent[] m_tiles;

        [Header("Completed")] 
        [SerializeField] private GameObject m_completePanel;
        [SerializeField] private Button m_restartButton;       
        
        [Header("Game Info")]
        [SerializeField] private TMP_Text m_moveText;
        [SerializeField] private TMP_Text m_timerText;
        
        [Header("Best Score")]
        [SerializeField] private TMP_Text m_bestMoveText;
        [SerializeField] private TMP_Text m_bestTimeText;
        [SerializeField] private TMP_Text m_bestDateText;       

        public SlidingPuzzleComponent[] Tiles => m_tiles;

        public event Action Close;
        public event Action Restart;

        private void OnEnable()
        {
            m_closeButton.onClick.AddListener(() => Close?.Invoke());
            m_restartButton.onClick.AddListener(() => Restart?.Invoke());           
        }
        
        private void OnDisable()
        {
            m_closeButton.onClick.RemoveListener(() => Close?.Invoke());
            m_restartButton.onClick.RemoveListener(() => Restart?.Invoke());          
        }

        public void SetMoves(int p_moves)
        {
            m_moveText.text = $"Moves: {p_moves}";
        }

        public void SetTimer(float p_time)
        {
            var (l_minutes, l_seconds) = GetTime(p_time);
            m_timerText.text = $"Time: {l_minutes:00}:{l_seconds:00}";
        }

        private (int l_minutes, int l_seconds) GetTime(float p_time)
        {
            int l_minutes = Mathf.FloorToInt(p_time / 60f);
            int l_seconds = Mathf.FloorToInt(p_time % 60f);
            
            return (l_minutes, l_seconds);
        }

        public void RenderBestScore(SlidingPuzzleModel p_slidingPuzzleModel)
        {
            if (p_slidingPuzzleModel is not { Moves: > 0 })
            {
                m_bestMoveText.text = "Moves: -";
                m_bestTimeText.text = "Time: -";
                m_bestDateText.text = "Date: -";
                return;
            }

            var (l_minutes, l_seconds) = GetTime(p_slidingPuzzleModel.Time);

            m_bestMoveText.text = $"Moves: {p_slidingPuzzleModel.Moves}";
            m_bestTimeText.text = $"Time: {l_minutes:00}:{l_seconds:00}";

            if (DateTimeOffset.TryParse(p_slidingPuzzleModel.DateTime, out DateTimeOffset l_datetime))
            {
                m_bestDateText.text = "Date: " + l_datetime.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
            }
            else
            {
                m_bestDateText.text = "Date: -";
            }
        }

        public void CompletePanel(bool p_isShow)
        {
            m_completePanel.SetActive(p_isShow);;
        }

        public void BoardPanel(bool p_isShow)
        {
            m_boardPanel.SetActive(p_isShow);
        }
    }
}