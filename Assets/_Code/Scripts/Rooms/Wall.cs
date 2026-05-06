using System.Collections.Generic;
using System.Linq;
using _Code.Scripts.Revamp.Bases;
using UnityEngine;

namespace _Code.Scripts.Rooms
{
    public class Wall : MonoBehaviour
    {
        private List<IConnector> _connectors = new();
        private List<Connector> _connectors2 = new();

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
            
            _connectors2 = GetComponentsInChildren<Connector>().ToList();
        }

        public void OnRoomRotationEnded()
        {
            foreach (var connector in _connectors)
            {
                connector.CheckConnection();
            }
            foreach (var connector in _connectors2)
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
            foreach (var connector in _connectors2)
            {
                connector.Disconnect();
            }
        }
    }
}