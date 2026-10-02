using UnityEngine;

namespace Game
{
    public class UIComponent : MonoBehaviour
    {
        public bool IsActiveSelf => gameObject.activeSelf;
        public bool IsActiveInHierarchy => gameObject.activeInHierarchy;
        
        public void Show()
        {
            OnShown();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            OnHidden();
        }

        public virtual void OnShown() { }
        public virtual void OnHidden() { }
    }
}