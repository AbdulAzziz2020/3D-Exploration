using System;

namespace Game
{
    [Serializable]
    public class PlayerInteractState : BaseState<Player, PlayerState>
    {
        public PlayerInteractState(Player entity, StateMachine<Player, PlayerState> stateMachine) : base(entity, stateMachine)
        {
        }

        public override PlayerState StateType => PlayerState.Interact;

        public override void Enter()
        {
            base.Enter();

            UIContext.Singleton.InteractFinish += HandleInteractFinish;
            
            Entity.Input.Disable();

            Entity.Animator.TriggerInteract();
            Entity.Animator.SetMove(false);
            
            Entity.Movement.Stop();
        }

        public override void Exit()
        {
            base.Exit();

            UIContext.Singleton.InteractFinish -= HandleInteractFinish;
            
            Entity.Input.Enable();
        }
        
        private void HandleInteractFinish()
        {
            StateMachine.ChangeState(PlayerState.Idle);
        }
    }
}