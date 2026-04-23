using UnityEngine;
using System.Collections.Generic;
using Interactions;
using Rooms;

namespace LinePaths
{
    [System.Serializable]
    public class Path
    {
        public List<PathSegment> segments = new();
        public bool isComplete;
        [SerializeField] private MonoBehaviour unlockTarget;

        public ILockable UnlockTarget => unlockTarget as ILockable; //unity inspector doesn't serialize interfaces
    }
}