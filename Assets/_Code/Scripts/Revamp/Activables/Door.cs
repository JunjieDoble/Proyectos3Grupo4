using _Code.Scripts.Revamp.Bases;

namespace _Code.Scripts.Revamp.Activables
{
    public class Door : Activable
    {
        public override bool IsActive()
        {
            return !base.IsActive();
        }

        public override void ActivatorUpdate()
        {
            SetDoorOpen(IsActive());
        }
        
        public void SetDoorOpen(bool open)
        {
            gameObject.SetActive(open);
        }
    }
}