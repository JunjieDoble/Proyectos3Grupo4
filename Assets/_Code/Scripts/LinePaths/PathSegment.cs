using _Code.Scripts.Rooms;
using UnityEngine;
using Rooms;

namespace LinePaths
{
    [System.Serializable]
    public class PathSegment
    {
        [SerializeField] private Room room;
        [SerializeField] private Transform entryPoint;
        [SerializeField] private Transform exitPoint;
        [SerializeField] private GameObject physicalPath;

        public Room Room => room;
        public Transform EntryPoint => entryPoint;
        public Transform ExitPoint => exitPoint;
        public GameObject PhysicalPath => physicalPath;
    }
}