using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Code.Scripts.Rooms
{
    public class Wall : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private List<HallConnector> halls = new();

        private void Awake()
        {
            halls = GetComponentsInChildren<HallConnector>().ToList();
        }

        public void OnRoomRotationEnded()
        {
            foreach (var hall in halls)
            {
                hall.CheckConnections();
            }
        }

        public void OnRoomRotationStarted()
        {
            foreach (var hall in halls)
            {
                hall.Disconnect();
            }
        }
    }
}