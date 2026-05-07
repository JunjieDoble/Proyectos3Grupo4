using _Code.Scripts.Bases;
using Interactions;

namespace _Code.Scripts.Activators
{
    public class Key : Activator, IInteractable
    {
        public void Interact(IInteractor interactor)
        {
            SetActive(true);
            onActivatorUpdate?.Invoke();
            Destroy(gameObject);
        }
    }
}