namespace _Code.Scripts.Rooms
{
    public interface IConnector
    {
        void CheckConnection();
        void Connect();
        void Disconnect();
    }
}
