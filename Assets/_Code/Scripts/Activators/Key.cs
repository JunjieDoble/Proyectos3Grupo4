using _Code.Scripts.Bases;
using Interactions;

namespace _Code.Scripts.Activators
{
    public class Key : Activator, IInteractable
    {
        private static Key _activeKey;
        public void Interact(IInteractor interactor)
        {
            SetActive(true);
            _activeKey?.SetActive(false);
            _activeKey = this;
            onActivatorUpdate?.Invoke();
        }
    }
}