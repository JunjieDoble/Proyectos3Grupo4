using UnityEngine;

//De moment no serveix per a res. Més endavant pot servir per mirar connexions entre rooms, portes obertes o tancades...
namespace Rooms
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private int gridSize; //2 si serà 2x2...
        [SerializeField] private float roomSize;
        private Room[,] _rooms;
        private void Start()
        {
            InitializeRooms();
        }

        private void InitializeRooms()
        {
            _rooms = new Room[gridSize, gridSize];
            var rooms = RoomRegistry.GetRooms();
            foreach (var room in rooms)
            {
                var gridPosition = WorldToGridPosition(room.transform.position);
                if (IsValidGridPosition(gridPosition))
                {
                    _rooms[gridPosition.x, gridPosition.y] = room;
                }
                else
                {
                    Debug.LogWarning("Room out of grid bounds: " + room.transform.position + " (Grid Position: " + gridPosition + ")");
                }
            }

            DebugGrid();
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