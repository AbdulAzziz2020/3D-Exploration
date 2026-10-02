using System;
using UnityEngine;

namespace Game
{
    public sealed class LookAtCamera : MonoBehaviour
    {
        private Camera m_camera;

        private void Awake()
        {
            m_camera = Camera.main;
        }

        private void LateUpdate()
        {
            if (m_camera == null)
                return;

            Vector3 l_direction = m_camera.transform.position - transform.position;
            l_direction.y = 0f;

            if (l_direction.sqrMagnitude <= 0.001f)
                return;

            transform.rotation =
                Quaternion.LookRotation(l_direction) *
                Quaternion.Euler(0f, 180f, 0f);
        }
    }
}