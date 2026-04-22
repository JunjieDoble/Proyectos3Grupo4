namespace Interactions
{
    public interface IHoldInteractable: IInteractable
    {
        void OnHoldStarted(IInteractor interactor);
        void OnHoldCanceled(IInteractor interactor);
    }
}