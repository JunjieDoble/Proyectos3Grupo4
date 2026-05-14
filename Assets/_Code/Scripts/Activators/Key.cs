using _Code.Scripts.Bases;
using Interactions;

namespace _Code.Scripts.Activators
{
    public class Key : Activator, IInteractable
    {
        private static Key _activeKey;
        public void Interact(IInteractor interactor)
        {
            _activeKey?.SetActive(false);
            SetActive(true);
            _activeKey = this;
            onActivatorUpdate?.Invoke();
        }
    }
}