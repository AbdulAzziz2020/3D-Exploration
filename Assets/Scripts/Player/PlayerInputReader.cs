using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public sealed class PlayerInputReader : PlayerInputAction.IGameplayActions
    {
        public event Action Interact;
        public event Action<Vector2> Move;

        private readonly PlayerInputAction m_inputSystemActions = new();

        public void Enable()
        {
            m_inputSystemActions.Gameplay.SetCallbacks(this);
            m_inputSystemActions.Gameplay.Enable();
        }

        public void Disable()
        {
            m_inputSystemActions.Gameplay.Disable();
            m_inputSystemActions.Gameplay.SetCallbacks(null);
        }

        public void OnMove(InputAction.CallbackContext p_context)
        {
            Move?.Invoke(p_context.ReadValue<Vector2>());
        }

        public void OnInteract(InputAction.CallbackContext p_context)
        {
            if (p_context.phase != InputActionPhase.Performed)
                return;

            Interact?.Invoke();
        }

        public void Dispose()
        {
            Disable();
            m_inputSystemActions.Dispose();
        }
    }
}