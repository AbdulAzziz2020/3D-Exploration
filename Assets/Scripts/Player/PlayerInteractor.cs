using UnityEngine;

namespace Game
{
    public sealed class PlayerInteractor : MonoBehaviour
    {
        [Header("Detection")]
        [SerializeField] private float m_radius = 0.75f;
        [SerializeField] private float m_distance = 2.5f;
        [SerializeField] private float m_offsetY = 0.5f;

        [SerializeField, Range(0f, 180f)]
        private float m_coneAngle = 60f;

        [SerializeField] private LayerMask m_interactableLayer;

        private readonly RaycastHit[] m_hits = new RaycastHit[16];

        [SerializeField] private Player m_player;
        private IInteractable m_target;

        public void Initialize(Player p_player)
        {
            m_player = p_player;
        }

        public void Tick()
        {
            FindTarget();
        }

        public bool TryInteract()
        {
            if (m_target == null)
                return false;

            if (!m_target.CanInteract)
                return false;

            m_target.Interact();

            return true;
        }

        private void FindTarget()
        {
            m_target = null;

            Transform l_transform = m_player.Body;

            Vector3 l_origin = l_transform.position + Vector3.up * m_offsetY;
            Vector3 l_forward = l_transform.forward;

            int l_count = Physics.SphereCastNonAlloc(
                l_origin,
                m_radius,
                l_forward,
                m_hits,
                m_distance,
                m_interactableLayer,
                QueryTriggerInteraction.Collide);

            float l_bestAngle = float.MaxValue;
            float l_bestDistance = float.MaxValue;

            for (int i = 0; i < l_count; i++)
            {
                Collider l_collider = m_hits[i].collider;

                if (!l_collider.TryGetComponent(out IInteractable l_interactable))
                    continue;

                if (!l_interactable.CanInteract)
                    continue;

                Vector3 l_direction = l_collider.bounds.center - l_origin;

                float l_distance = l_direction.magnitude;

                if (l_distance <= 0.001f)
                    continue;

                l_direction /= l_distance;

                float l_angle = Vector3.Angle(l_forward, l_direction);

                if (l_angle > m_coneAngle * 0.5f)
                    continue;

                if (l_angle > l_bestAngle)
                    continue;

                if (Mathf.Approximately(l_angle, l_bestAngle) &&
                    l_distance >= l_bestDistance)
                    continue;

                l_bestAngle = l_angle;
                l_bestDistance = l_distance;
                m_target = l_interactable;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (m_player == null)
                return;

            Transform l_transform = m_player.Body;

            Vector3 l_origin = l_transform.position + Vector3.up * m_offsetY;
            Vector3 l_forward = l_transform.forward;

            // SphereCast volume
            Gizmos.DrawWireSphere( l_origin, m_radius);

            Vector3 l_end = l_origin + l_forward * m_distance;

            Gizmos.DrawWireSphere( l_end, m_radius);
            Gizmos.DrawLine( l_origin + Vector3.right * m_radius, l_end + Vector3.right * m_radius);
            Gizmos.DrawLine( l_origin - Vector3.right * m_radius, l_end - Vector3.right * m_radius);

            // Cone
            float l_halfAngle = m_coneAngle * 0.5f;

            Quaternion l_leftRotation = Quaternion.AngleAxis(-l_halfAngle, Vector3.up);
            Quaternion l_rightRotation = Quaternion.AngleAxis(l_halfAngle, Vector3.up);

            Vector3 l_left = l_leftRotation * l_forward * m_distance;
            Vector3 l_right = l_rightRotation * l_forward * m_distance;

            Gizmos.DrawRay( l_origin, l_left);
            Gizmos.DrawRay( l_origin, l_right);

            // Current target
            if (m_target is Component l_component)
            {
                Gizmos.DrawLine( l_origin, l_component.transform.position);
            }
        }
#endif
    }
}