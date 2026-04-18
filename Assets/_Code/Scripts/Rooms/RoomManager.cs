using UnityEngine;
using System.Collections.Generic;
using Doors;

namespace Rooms
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private int gridSize; //2 si serà 2x2...
        [SerializeField] private float roomSize;
        private Dictionary<Room, Vector2Int> _roomDictionary = new(); //for faster lookup of rooms
        private Room[,] _rooms;

        private static readonly Vector2Int[] Offsets = new Vector2Int[]
        {
            new Vector2Int(0, 1), //North
            new Vector2Int(1, 0), //East
            new Vector2Int(0, -1), //South
            new Vector2Int(-1, 0) //West
        };
        private void Start()
        {
            InitializeRooms();
        }

        private void InitializeRooms()
        {
            _rooms = new Room[gridSize, gridSize];
            var rooms = RoomRegistry.GetRooms();
            if (rooms == null) return;

            foreach (var room in rooms)
            {
                var gridPosition = WorldToGridPosition(room.transform.position);
                if (IsValidGridPosition(gridPosition))
                {
                    if (_rooms[gridPosition.x, gridPosition.y] != null)
                    {
                        Debug.LogWarning($"Two rooms in same grid position {gridPosition}");
                        continue;
                    }

                    _rooms[gridPosition.x, gridPosition.y] = room;
                    room.OnRotationChanged += HandleRoomRotated;
                    _roomDictionary.Add(room, gridPosition);
                }
                else
                {
                    Debug.LogWarning("Room out of grid bounds: " + room.transform.position + " (Grid Position: " + gridPosition + ")");
                }
            }

            //DebugGrid();
            SyncAllDoors();
        }

        private Vector2Int WorldToGridPosition(Vector3 worldPosition)
        {
            var gridX = Mathf.FloorToInt(worldPosition.x / roomSize);
            var gridY = Mathf.FloorToInt(worldPosition.z / roomSize);
            return new Vector2Int(gridX, gridY);
        }

        private bool IsValidGridPosition(Vector2Int gridPosition)
        {
            return gridPosition.x >= 0 && gridPosition.x < gridSize &&
                    gridPosition.y >=0 && gridPosition.y < gridSize;
        }

        private void HandleRoomRotated(Room room)
        {
            if (!_roomDictionary.TryGetValue(room, out Vector2Int roomPosition)) return;
            
            for (int i = 0; i < 4; i++)
            {
                Vector2Int dirOffset = Offsets[i];
                Vector2Int neighborPos = roomPosition + dirOffset;

                Room neighbor = GetRoom(neighborPos);
                SyncDoors(room, neighbor, (Direction)i);
            }
        }

        private Room GetRoom(Vector2Int pos)
        {
            if (pos.x < 0 || pos.y < 0 || pos.x >= gridSize || pos.y >= gridSize) return null;
            return _rooms[pos.x, pos.y];
        }

        private void SyncDoors(Room a, Room b, Direction dirFromA)
        {
            Direction opposite = (Direction)(((int)dirFromA + 2) % 4);

            for (int layer = 0; layer < 2; layer++)
            {
                Layer l = (Layer)layer;

                Door doorA = a.GetDoor(l, dirFromA);

                if (b == null)
                {
                    if (doorA != null) doorA.OpenDoor(false);
                    continue;
                }

                Door doorB = b.GetDoor(l, opposite);

                bool shouldOpen = doorA != null && doorB != null;

                if (doorA != null) doorA.OpenDoor(shouldOpen);

                if (doorB != null) doorB.OpenDoor(shouldOpen);
            }
        }

        private void SyncAllDoors()
        {
            foreach (var room in _roomDictionary.Keys)
            {
                HandleRoomRotated(room);
            }
        }

        private void OnDestroy()
        {
            foreach (var room in _roomDictionary.Keys)
            {
                room.OnRotationChanged -= HandleRoomRotated;
            }
        }

        private void DebugGrid()
        {
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    var room = _rooms[x, y];
                    if (room != null)
                    {
                        Debug.Log($"Room at grid ({x}, {y}): {room.name}");
                    }
                    else
                    {
                        Debug.Log($"No room at grid ({x}, {y})");
                    }
                }
            }
        }
    }
}