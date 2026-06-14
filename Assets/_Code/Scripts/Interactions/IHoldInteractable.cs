using _Code.Scripts.Interactions;

namespace Interactions
{
    public interface IHoldInteractable: IInteractable
    {
        void OnHoldStarted(IInteractor interactor);
        void OnHoldCanceled(IInteractor interactor);
        void OnHoldCompleted(IInteractor interactor);
    }
}