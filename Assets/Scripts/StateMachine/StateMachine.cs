using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [Serializable]
    public abstract class StateMachine<TEntity, TEnum> where TEnum : Enum
    {
        [SerializeReference] protected BaseState<TEntity, TEnum> m_current;
        [ShowInInspector] protected Dictionary<TEnum, BaseState<TEntity, TEnum>> m_states = new();
        
        public BaseState<TEntity, TEnum> Current => m_current;
        protected TEntity m_owner;
        
        public abstract void Initialize(TEntity p_entity, TEnum p_startType);

        public virtual void ChangeState(TEnum newState)
        {
            m_current?.Exit();
            m_current = m_states[newState];
            m_current.Enter();
        }
        
        protected void Register(BaseState<TEntity, TEnum> p_state)
        {
            m_states[p_state.StateType] = p_state;
        }
        
        public TState GetState<TState>()
            where TState : BaseState<TEntity, TEnum>
        {
            foreach (var l_state in m_states.Values)
            {
                if (l_state is TState typedState)
                    return typedState;
            }

            throw new InvalidOperationException(
                $"State of type {typeof(TState).Name} " +
                $"is not registered."
            );
        }

        public bool TryGetState<TState>(out TState p_state) where TState : BaseState<TEntity, TEnum>
        {
            foreach (var registeredState in m_states.Values)
            {
                if (registeredState is TState typedState)
                {
                    p_state = typedState;
                    return true;
                }
            }

            p_state = null;
            return false;
        }
        
        public virtual void Update() => m_current?.Update();
        
        public virtual void FixedUpdate() => m_current?.FixedUpdate();
    }
}