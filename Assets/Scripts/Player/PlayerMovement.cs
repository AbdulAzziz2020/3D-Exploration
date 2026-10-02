using UnityEngine;

namespace Game
{
    public sealed class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody m_rigidbody;
        [SerializeField] private float m_speed = 5f;
        [SerializeField] private float m_rotationSpeed = 15f;

        private Player m_player;
        private Vector2 m_moveInput;

        public Vector2 MoveInput => m_moveInput;
        public bool IsMoving => m_moveInput.sqrMagnitude > 0.001f;

        public void Initialize(Player p_player)
        {
            m_player = p_player;
        }

        public void SetInput(Vector2 p_input)
        {
            m_moveInput = p_input;
        }

        public void FixedTick()
        {
            Vector3 l_direction = new Vector3(
                m_moveInput.x,
                0f,
                m_moveInput.y);

            if (l_direction.sqrMagnitude > 1f)
                l_direction.Normalize();

            if (l_direction.sqrMagnitude <= 0.001f)
                return;

            Vector3 l_position = m_rigidbody.position;
            l_position += l_direction * (m_speed * Time.fixedDeltaTime);

            m_rigidbody.MovePosition(l_position);

            Quaternion l_targetRotation = Quaternion.LookRotation(l_direction);
            Quaternion l_rotation = Quaternion.Slerp(
                m_player.Body.rotation,
                l_targetRotation,
                m_rotationSpeed * Time.fixedDeltaTime);

            m_player.Body.rotation = l_rotation;
        }

        public void Stop()
        {
            m_moveInput = Vector2.zero;

            m_rigidbody.linearVelocity = Vector3.zero;
            m_rigidbody.angularVelocity = Vector3.zero;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (m_player == null)
                return;

            Gizmos.DrawRay(
                m_player.Body.position,
                m_player.transform.forward);
        }
#endif
    }
}