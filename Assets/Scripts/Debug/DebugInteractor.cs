using UnityEngine;

namespace Game
{
    public class DebugInteractor : MonoBehaviour, IInteractable
    {
        public bool CanInteract => true;
        
        public void Interact()
        {
            Debug.Log("Interact: " + name, this);
        }
    }
}