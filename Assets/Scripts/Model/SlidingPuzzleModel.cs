using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class SlidingPuzzleModel
    {
        [SerializeField, JsonProperty] private int m_moves = 0;
        [SerializeField, JsonProperty] private float m_time = 0f;
        [SerializeField, JsonProperty] private string m_datetime;
        
        [JsonIgnore] public int Moves => m_moves;
        [JsonIgnore] public float Time => m_time;
        [JsonIgnore] public string DateTime => m_datetime;
        
        public bool IsBetterThan(int p_moves, float p_time)
        {
            if (m_moves != p_moves)
                return p_moves < m_moves;

            return p_time < m_time;
        }

        public void SetScore(int p_moves, float p_time, string m_iso)
        {
            m_moves = p_moves;
            m_time = p_time;
            m_datetime = m_iso;
        }
        
    }
}