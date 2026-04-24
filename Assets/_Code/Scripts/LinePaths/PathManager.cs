using UnityEngine;
using System.Collections.Generic;
using _Code.Scripts.Rooms;
using Interactions;

namespace LinePaths
{
    public class PathManager : MonoBehaviour
    {
        [SerializeField] private List<Path> allPaths = new();
        private readonly Dictionary<Room, List<Path>> _pathsByRoom = new(); //so when a room is rotated, we only check affected paths
        
        const float ALIGNMENT_THRESHOLD = 10f;
        
        private void Awake()
        {
            Initialization();
        }

        private void Initialization()
        {
            foreach (var path in allPaths)
            {
                if (path?.Segments == null || path.Segments.Count < 2)
                {
                    Debug.LogWarning("Invalid path configuration: " + path);
                    continue;
                }

                foreach (var segment in path.Segments)
                {
                    if (segment?.Room == null)
                    {
                        Debug.LogWarning("Invalid path segment configuration: " + segment);
                        continue;
                    }

                    if (!_pathsByRoom.ContainsKey(segment.Room))
                    {
                        _pathsByRoom[segment.Room] = new List<Path>();
                    }
                    _pathsByRoom[segment.Room].Add(path);
                }
            }

            foreach (var path in allPaths)
            {
                CheckPathAlignment(path);
            }
        }

        private void OnEnable() {
                Room.OnEndRotation += OnRoomRotated;
        }

        private void OnRoomRotated(Room rotatedRoom)
        {
            Debug.Log($"Room {rotatedRoom.name} was rotated");
            if (_pathsByRoom.TryGetValue(rotatedRoom, out var affectedPaths))
            {
                foreach (var path in affectedPaths)
                {
                    Debug.Log($"Checking path alignment for path affected by rotation of room {rotatedRoom.name}: {path}");
                    CheckPathAlignment(path);
                }
            }
        }

        private void CheckPathAlignment(Path path)
        {
            if (path == null || path.Segments == null || path.Segments.Count < 2)
            {
                SetPathState(path, false);
                return;
            }

            for (int i = 0; i < path.Segments.Count - 1; i++)
            {
                var current = path.Segments[i];
                var next = path.Segments[i + 1];
                
                //float distance = Vector3.Distance(current.ExitPoint.position, next.EntryPoint.position);
                //Debug.Log($"Checking segment {i} of path: Exit {current.ExitPoint.position} to Entry {next.EntryPoint.position}, Distance: {distance}");
                if (!AreSegmentsConnected(current, next))
                {
                    SetPathState(path, false);
                    return;
                }
            }

            SetPathState(path, true);
        }

        private bool AreSegmentsConnected(PathSegment current, PathSegment next)
        {
            float exitToEntry = Vector3.Distance(current.ExitPoint.position, next.EntryPoint.position);
            float exitToExit = Vector3.Distance(current.ExitPoint.position, next.ExitPoint.position);
            float entryToEntry = Vector3.Distance(current.EntryPoint.position, next.EntryPoint.position);
            float entryToExit = Vector3.Distance(current.EntryPoint.position, next.ExitPoint.position);

            return exitToEntry < ALIGNMENT_THRESHOLD || 
                exitToExit < ALIGNMENT_THRESHOLD || 
                entryToEntry < ALIGNMENT_THRESHOLD || 
                entryToExit < ALIGNMENT_THRESHOLD;
        }

        private void SetPathState(Path path, bool isComplete)
        {
            
            if (path == null) return;
  
            ILockable lockable = path.UnlockTarget;
            if (lockable == null) return;

            if (isComplete)
            {
                lockable.Unlock();
                ChangePathVisuals(path, isComplete);
            }
            else
            {
                if (path.IsComplete) //only change visuals and lock if it was previously complete
                {
                    ChangePathVisuals(path, isComplete);
                    lockable.Lock(); //opcional
                }
            }
            path.SetComplete(isComplete);
            
            Debug.Log($"Setting path state: {path} to {(isComplete ? "Complete" : "Incomplete")}");
        }

        private void ChangePathVisuals(Path path, bool isComplete)
        {
            foreach (var segment in path.Segments)
            {
                if (segment.PhysicalPath != null)
                {
                    var meshRenderer = segment.PhysicalPath.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        meshRenderer.material.color = isComplete ? Color.green : Color.blue; //Temporal
                    }
                }
            }
        }
    }
}