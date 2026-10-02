using System;
using UnityEngine.TextCore.Text;

namespace Game
{
    public enum PlayerState
    {
        Idle,
        Move,
        Interact
    }
    
    [Serializable]
    public class PlayerStateMachine : StateMachine<Player, PlayerState>
    {
        public override void Initialize(Player p_entity, PlayerState p_startType)
        {
            m_owner = p_entity;
            
            Register(new PlayerIdleState(p_entity, this));
            Register(new PlayerMoveState(p_entity, this));
            Register(new PlayerInteractState(p_entity, this));
            
            ChangeState(p_startType);
        }
    }
}