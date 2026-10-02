using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class UserModel
    {
        [SerializeField, JsonProperty] private string m_id;
        [SerializeField, JsonProperty] private MemoryMatchModel m_memoryMatch = new();
        [SerializeField, JsonProperty] private SlidingPuzzleModel m_slidingPuzzle = new();

        [JsonIgnore] public string Id => m_id;
        [JsonIgnore] public MemoryMatchModel MemoryMatch => m_memoryMatch;
        [JsonIgnore] public SlidingPuzzleModel SlidingPuzzle => m_slidingPuzzle;

        public UserModel()
        {

        }

        public UserModel(string p_id)
        {
            m_id = p_id;
        }
    }
}