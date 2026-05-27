using _Code.Scripts.Bases;
using Interactions;

namespace _Code.Scripts.Activators
{
    public class Terminal : Activator, IInteractable
    {
        private static Terminal _activeTerminal;
        public void Interact(IInteractor interactor)
        {
            _activeTerminal?.SetActive(false);
            SetActive(true);
            _activeTerminal = this;
            onActivatorUpdate?.Invoke();
        }
    }
}