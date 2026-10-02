using System;
using UnityEngine;

namespace Game
{
    public class UIPresenter<T> : MonoBehaviour where T : UIView
    {
        [SerializeField] private bool m_isHideOnStart = true;
        [SerializeField] protected T m_view;

        public virtual void BeforeInitialize()
        {
            if (m_isHideOnStart) 
                Hide();
        }
        
        public virtual void Dispose()
        { }

        public virtual void Show() => m_view.Show();
        
        public virtual void Hide() => m_view.Hide();
    }
}