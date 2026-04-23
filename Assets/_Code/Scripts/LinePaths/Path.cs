using UnityEngine;
using System.Collections.Generic;
using Interactions;

namespace LinePaths
{
    [System.Serializable]
    public class Path
    {
        [SerializeField] private List<PathSegment> segments = new();
        [SerializeField] private bool isComplete;
        [SerializeField] private MonoBehaviour unlockTarget;

        public List<PathSegment> Segments => segments;
        public bool IsComplete => isComplete;
        public ILockable UnlockTarget => unlockTarget as ILockable; //unity inspector doesn't serialize interfaces

        public void SetComplete(bool value)
        {
            isComplete = value;
        }
    }
}