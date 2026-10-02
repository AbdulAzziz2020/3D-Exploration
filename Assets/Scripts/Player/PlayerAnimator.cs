using UnityEngine;

namespace Game
{
    public sealed class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator m_animator;

        private Player m_player;
        
        private readonly int MoveHash = Animator.StringToHash("IsMove");
        private readonly int InteractHash = Animator.StringToHash("Interact");
        
        public void Initialize(Player p_player)
        {
            m_player = p_player;
        }

        public void SetMove(bool p_isMove)
        {
            m_animator.SetBool(MoveHash, p_isMove);
        }

        public void TriggerInteract()
        {
            m_animator.SetTrigger(InteractHash);
        }
    }
}