using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Code.Scripts.Rooms
{
    public class Wall : MonoBehaviour
    {
        private List<IConnector> _connectors = new();

        private void Awake()
        {
            _connectors = GetComponentsInChildren<IConnector>().ToList();
            foreach (var connector in _connectors)
            {
                if (connector is HallConnector hallConnector)
                {
                    hallConnector.SetWall(this);
                }
            }
        }

        public void OnRoomRotationEnded()
        {
            foreach (var connector in _connectors)
            {
                connector.CheckConnection();
            }
        }

        public void OnRoomRotationStarted()
        {
            foreach (var connector in _connectors)
            {
                connector.Disconnect();
            }
        }
    }
}