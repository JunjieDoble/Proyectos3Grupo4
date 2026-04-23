using UnityEngine;
using System.Collections.Generic;
using Rooms;
using Interactions;
using Unity.VisualScripting;

namespace LinePaths
{
    public class PathManager : MonoBehaviour
    {
        [SerializeField] private List<Path> allPaths = new();
        private readonly Dictionary<Room, List<Path>> _pathsByRoom = new();
        
        const float ALIGNMENT_THRESHOLD = 1f;
        
        private void Awake()
        {
            Initialization();
        }

        private void Initialization()
        {
            foreach (var path in allPaths)
            {
                if (path?.segments == null || path.segments.Count < 2)
                {
                    Debug.LogWarning("Invalid path configuration: " + path);
                    continue;
                }

                foreach (var segment in path.segments)
                {
                    if (segment?.room == null)
                    {
                        Debug.LogWarning("Invalid path segment configuration: " + segment);
                        continue;
                    }

                    if (!_pathsByRoom.ContainsKey(segment.room))
                    {
                        _pathsByRoom[segment.room] = new List<Path>();
                    }
                    _pathsByRoom[segment.room].Add(path);
                }
            }
    
            foreach (var room in RoomRegistry.GetRooms())
            {
                room.OnRotationChanged += OnRoomRotated;
            }

            foreach (var path in allPaths)
            {
                CheckPathAlignment(path);
            }
        }

        private void OnRoomRotated(Room rotatedRoom)
        {
            if (_pathsByRoom.TryGetValue(rotatedRoom, out var affectedPaths))
            {
                foreach (var path in affectedPaths)
                {
                    CheckPathAlignment(path);
                }
            }
        }

        private void CheckPathAlignment(Path path)
        {
            Debug.Log("Checking path alignment: " + path);
            if (path == null || path.segments == null || path.segments.Count < 2)
            {
                SetPathState(path, false);
                return;
            }

            for (int i = 0; i < path.segments.Count - 1; i++)
            {
                var current = path.segments[i];
                var next = path.segments[i + 1];
                
                float distance = Vector3.Distance(current.exitPoint.position, next.entryPoint.position);
                Debug.Log($"Checking segment {i} of path: Exit {current.exitPoint.position} to Entry {next.entryPoint.position}, Distance: {distance}");
                if (distance > ALIGNMENT_THRESHOLD)
                {
                    SetPathState(path, false);
                    return;
                }
            }

            SetPathState(path, true);
        }

        private static void SetPathState(Path path, bool isComplete)
        {
            Debug.Log($"Setting path state: {path} to {(isComplete ? "Complete" : "Incomplete")}");
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
                if (path.isComplete) //only change visuals and lock if it was previously complete
                {
                    ChangePathVisuals(path, isComplete);
                    lockable.Lock(); //opcional
                }
            }
            path.isComplete = isComplete;
        }

        private static void ChangePathVisuals(Path path, bool isComplete)
        {
            foreach (var segment in path.segments)
            {
                if (segment.physicalPath != null)
                {
                    var meshRenderer = segment.physicalPath.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        meshRenderer.material.color = isComplete ? Color.green : Color.blue;
                    }
                }
            }
        }

        private void OnDestroy()
        {
            foreach (var room in RoomRegistry.GetRooms())
            {
                room.OnRotationChanged -= OnRoomRotated;
            }
        }
    }
}