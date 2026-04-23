namespace Interactions
{
    public interface ILockable
    {
        bool IsLocked();
        void Lock();
        void Unlock();
    }
}