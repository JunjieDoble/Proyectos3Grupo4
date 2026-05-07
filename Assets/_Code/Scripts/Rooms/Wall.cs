using System.Collections.Generic;
using System.Linq;
using _Code.Scripts.Revamp.Bases;
using UnityEngine;

namespace _Code.Scripts.Rooms
{
    public class Wall : MonoBehaviour
    {
        private List<Connector> _connectors = new();

        private void Awake()
        {
            _connectors = GetComponentsInChildren<Connector>().ToList();
            foreach (var connector in _connectors)
            {
                if (connector is Revamp.Activators.Connectors.HallConnector hallConnector)
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