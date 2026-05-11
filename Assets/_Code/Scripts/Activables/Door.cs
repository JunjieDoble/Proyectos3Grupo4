using _Code.Scripts.Bases;

namespace _Code.Scripts.Activables
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

        private void SetDoorOpen(bool open)
        {
            gameObject.SetActive(open);
        }
    }
}