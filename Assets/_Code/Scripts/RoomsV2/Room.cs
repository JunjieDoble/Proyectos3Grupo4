using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Code.Scripts.RoomsV2
{
    
    public enum Direction
    {
        North = 0,
        South = 1,
        East = 2,
        West = 3
    }
    public class Room : MonoBehaviour
    {
        [Header("Walls")]
        [SerializeField] private Wall northWall;
        [SerializeField] private Wall southWall;
        [SerializeField] private Wall eastWall;
        [SerializeField] private Wall westWall;

        [Header("Detection")] 
        [SerializeField] private LayerMask roomMask;
        [SerializeField] private float adjacentRoomDetectionDistance = 1.5f;
        [SerializeField] private Vector3 adjacentRoomDetectionExtents = new(0.4f, 1.5f, 0.4f);
        
        private readonly Dictionary<Direction, Room> _adjacentRooms = new();
        private int _rotationSteps;
        
        public IReadOnlyDictionary<Direction, Room> AdjacentRooms => _adjacentRooms;

        private void Start()
        {
            RefreshRoom();
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying) return;
            RefreshRoom();
        }
#endif

        public void RefreshRoom()
        {
            RefreshAdjacentRooms();
            RefreshDoors();

            foreach (Room room in _adjacentRooms.Values)
            {
                if (room != null)
                    room.RefreshDoors();
            }
        }
        
        public void AddRotationStepClockwise()
        {
            _rotationSteps = (_rotationSteps + 1) % 4;
        }

        public Room GetAdjacentRoom(Direction dir)
        {
            _adjacentRooms.TryGetValue(dir, out Room room);
            return room;
        }

        public Wall GetWall(Direction worldDir)
        {
            Direction localDirection = GetLocalDirectionFromWorld(worldDir);

            return localDirection switch
            {
                Direction.North => northWall,
                Direction.South => southWall,
                Direction.East => eastWall,
                Direction.West => westWall,
                _ => null
            };
        }

        public void RefreshAdjacentRooms()
        {
            _adjacentRooms.Clear();

            foreach (Direction worldDirection in System.Enum.GetValues(typeof(Direction)))
            {
                Vector3 dir = GetWorldDirectionVector(worldDirection);
                Vector3 checkCenter = transform.position + dir * adjacentRoomDetectionDistance;

                Collider[] hits = Physics.OverlapBox(
                    checkCenter,
                    adjacentRoomDetectionExtents,
                    Quaternion.identity,
                    roomMask
                );

                Room foundRoom = null;

                foreach (Collider hit in hits)
                {
                    Room room = hit.GetComponentInParent<Room>();
                    if (room != null && room != this)
                    {
                        foundRoom = room;
                        break;
                    }
                }

                _adjacentRooms[worldDirection] = foundRoom;
            }
        }

        public void RefreshDoors()
        {
            foreach (Direction worlDir in System.Enum.GetValues(typeof(Direction)))
            {
                Wall myWall = GetWall(worlDir);
                if (myWall == null) continue;
                
                Room otherRoom = GetAdjacentRoom(worlDir);

                if (otherRoom == null)
                {
                    myWall.CloseALlDoors();
                    continue;
                }
                
                Direction oppositeDir = GetOpposite(worlDir);
                Wall otherWall = otherRoom.GetWall(oppositeDir);

                if (otherWall == null)
                {
                    myWall.CloseALlDoors();
                    continue;
                }
                
                SyncDoorLevel(myWall, otherWall, DoorLevel.Top);
                SyncDoorLevel(myWall, otherWall, DoorLevel.Bottom);
            }
        }

        private void SyncDoorLevel(Wall myWall, Wall otherWall, DoorLevel level)
        {
            bool canConnect = myWall.HasDoor(level) && otherWall.HasDoor(level);
            myWall.SetDoorOpen(level, canConnect);
            otherWall.SetDoorOpen(level, canConnect);
        }
        
        private Direction GetLocalDirectionFromWorld(Direction worldDir)
        {
            return (Direction)(((int)worldDir - _rotationSteps + 4) % 4);
        }

        private Direction GetOpposite(Direction dir)
        {
            return dir switch
            {
                Direction.North => Direction.South,
                Direction.South => Direction.North,
                Direction.East => Direction.West,
                Direction.West => Direction.East,
                _ => dir
            };
        }
        
        private Vector3 GetWorldDirectionVector(Direction direction)
        {
            return direction switch
            {
                Direction.North => Vector3.forward,
                Direction.East  => Vector3.right,
                Direction.South => Vector3.back,
                Direction.West  => Vector3.left,
                _ => Vector3.zero
            };
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.3f);

            foreach (Direction dir in System.Enum.GetValues(typeof(Direction)))
            {
                Vector3 dirVector = GetWorldDirectionVector(dir);
                Vector3 checkCenter = transform.position + dirVector * adjacentRoomDetectionDistance;

                // detection box
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(checkCenter, adjacentRoomDetectionExtents * 2f);

                // line to check point
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(transform.position, checkCenter);

                // if adjacent exists, draw green line to it
                Room adjacent = GetAdjacentRoom(dir);
                if (adjacent != null)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(checkCenter, adjacent.transform.position);
                    Gizmos.DrawSphere(adjacent.transform.position, 0.2f);
                }
                else
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(checkCenter, 0.15f);
                }
            }
        }
    }
}