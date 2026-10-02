using System;

namespace Game
{
    public interface INotify<T>
    {
        event Action<T> OnChanged;
        void Notify();
    }

    public interface IInteractable
    {
        bool CanInteract { get; }
        void Interact();
    }
}