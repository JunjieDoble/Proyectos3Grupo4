using UnityEngine;
using System;
using Doors;

namespace Rooms
{
    public enum Layer {
        Up = 0,
        Down = 1
    }

    public enum Direction {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    public class Room : MonoBehaviour
    {
        [Serializable]
        private class LayerDoors
        {
            public Door North;
            public Door East;
            public Door South;
            public Door West;

            public Door GetDoor(Direction direction)
            {
                switch (direction)
                {
                    case Direction.North: return North;
                    case Direction.East: return East;
                    case Direction.South: return South;
                    case Direction.West: return West;
                    default: return null;
                }
            }
        }

        public Action<Room> OnRotationChanged;
        private Quaternion _startRotation; //before hold
        private int _currentAbsRotation; //0, 90, 180, 270

        [SerializeField] private LayerDoors upperLayerDoors;
        [SerializeField] private LayerDoors lowerLayerDoors;

        private Door[,] _doors = new Door[2, 4]; //[layer, direction]

        private void Awake()
        {
            RoomRegistry.AddRoom(this);
            CacheDoors();
        }

        public void StartRotate()
        {
            _currentAbsRotation = (_currentAbsRotation + 90) % 360;
            _startRotation = transform.rotation;
            transform.Rotate(0, 90, 0); //TODO: això és de mentres, fer-ho visual...
            OnRotationChanged?.Invoke(this); //TODO: quan es faci visual s'hauria de fer al final de la rotació, quan ja no es pugui cancel·lar
        }

        public void CancelRotate()
        {
            _currentAbsRotation = (_currentAbsRotation - 90 + 360) % 360;
            transform.rotation = _startRotation;
            OnRotationChanged?.Invoke(this); //quan l'altre es faci al final, aquest potser ni cal
        }        

        public Door GetDoor(int layer, Direction dir) //Em demanen per la north del món
        {
            Direction realDir = GetRotatedDirection(dir); //Però si està rotada 90 graus, el que volen és la meva west
            if (layer < 0 || layer > 1) return null;
            return _doors[layer, (int)realDir];
        }

        private Direction GetRotatedDirection(Direction dir)
        {
            int steps = _currentAbsRotation / 90;
            return (Direction)(((int)dir - steps + 4) % 4);
        }

        private void CacheDoors()
        {
            if (upperLayerDoors == null) upperLayerDoors = new LayerDoors();
            if (lowerLayerDoors == null) lowerLayerDoors = new LayerDoors();

            for (int i = 0; i < 4; i++)
            {
                Direction direction = (Direction)i;
                _doors[(int)Layer.Up, i] = upperLayerDoors.GetDoor(direction);
                _doors[(int)Layer.Down, i] = lowerLayerDoors.GetDoor(direction);
            }
        }

        private void OnDestroy()
        {
            RoomRegistry.RemoveRoom(this);
        }
    }
}

