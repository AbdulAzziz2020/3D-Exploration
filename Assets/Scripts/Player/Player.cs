using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public sealed class Player : MonoBehaviour
    {
        [SerializeField] private Transform m_body;
        [SerializeField] private PlayerMovement m_movement;
        [SerializeField] private PlayerAnimator m_animator;
        [SerializeField] private PlayerInteractor m_interactor;

        private PlayerInputReader m_input;
        private PlayerStateMachine m_machine;
        
        public bool IsMoving => m_movement.IsMoving;
        [ShowInInspector] public PlayerState CurrentState => m_machine?.Current?.StateType ?? PlayerState.Idle;

        public PlayerAnimator Animator => m_animator;
        public PlayerInteractor Interactor => m_interactor;
        public PlayerInputReader Input => m_input;
        public PlayerMovement Movement => m_movement;
        public Transform Body => m_body;
        
        private void Awake()
        {
            m_input = new PlayerInputReader();
            m_machine = new PlayerStateMachine();

            m_movement.Initialize(this);
            m_interactor.Initialize(this);
            m_animator.Initialize(this);
            m_machine.Initialize(this, PlayerState.Idle);

            m_input.Move += m_movement.SetInput;
            m_input.Interact += OnInteract;

            m_input.Enable();
        }
        
        private void OnDestroy()
        {
            m_input.Move -= m_movement.SetInput;
            m_input.Interact -= OnInteract;

            m_input.Dispose();
        }

        private void Update()
        {
            m_interactor.Tick();
            m_machine?.Update();
        }

        private void FixedUpdate()
        {
            m_machine?.FixedUpdate();
        }

        private void OnInteract()
        {
            if (!m_interactor.TryInteract())
                return;
            
            m_machine.ChangeState(PlayerState.Interact);
        }
    }
}
