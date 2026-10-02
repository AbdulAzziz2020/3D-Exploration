using System;

namespace Game
{
    [Serializable]
    public class PlayerIdleState : BaseState<Player, PlayerState>
    {
        public PlayerIdleState(Player entity, StateMachine<Player, PlayerState> stateMachine) : base(entity, stateMachine)
        {
        }

        public override PlayerState StateType => PlayerState.Idle;
        
        public override void Enter()
        {
            base.Enter();
            
            Entity.Movement.Stop();
            Entity.Animator.SetMove(false);
        }

        public override void Update()
        {
            if (Entity.IsMoving)
            {
                StateMachine.ChangeState(PlayerState.Move);
            }
        }
    }
}