namespace Interactions
{
    public interface IHoldInteractable: IInteractable
    {
        void OnHoldStarted();
        void OnHoldCanceled();
    }
}