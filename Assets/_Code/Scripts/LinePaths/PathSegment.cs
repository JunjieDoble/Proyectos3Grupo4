using UnityEngine;
using Rooms;

namespace LinePaths
{
    [System.Serializable]
    public class PathSegment
    {
        public Room room;
        public Transform entryPoint;
        public Transform exitPoint;
        public GameObject physicalPath;
    }
}