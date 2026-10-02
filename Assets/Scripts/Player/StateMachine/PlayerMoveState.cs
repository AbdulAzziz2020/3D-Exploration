using System;

namespace Game
{
    [Serializable]
    public class PlayerMoveState : BaseState<Player, PlayerState>
    {
        public PlayerMoveState(Player entity, StateMachine<Player, PlayerState> stateMachine) : base(entity, stateMachine)
        {
        }

        public override PlayerState StateType => PlayerState.Move;

        public override void Enter()
        {
            Entity.Animator.SetMove(true);
        }
        
        public override void Update()
        {
            if (!Entity.IsMoving)
            {
                StateMachine.ChangeState(PlayerState.Idle);   
            }
        }

        public override void FixedUpdate()
        {
            Entity.Movement.FixedTick();
        }
    }
}