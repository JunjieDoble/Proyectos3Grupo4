using Interactions;
using UnityEngine;

namespace _Code.Scripts.Interactions
{
    public interface IInteractable
    {
        GameObject GameObject { get; }
        void Interact(IInteractor interactor);
    }
}