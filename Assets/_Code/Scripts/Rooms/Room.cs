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
        public static Action<Room> OnStartRotation;
        public static Action<Room> OnEndRotation;
        private Quaternion _startRotation; //before hold
        private bool _isRotating;
        private int _currentAbsRotation; //0, 90, 180, 270

        [SerializeField] private LayerDoors[] editorDoors = new LayerDoors[2]; //0 down, 1 up. per l'inspector, però després es treballa amb la matriu de sota per facilitar l'accés
        private Door[,] _doors = new Door[2, 4]; //[layer, direction]

        private void Awake()
        {
            RoomRegistry.AddRoom(this);
            CacheDoors();
        }

        private void Update()
        {
            RotateRoom();
        }

        public void StartRotate()
        {
            if (_isRotating) return;
            _currentAbsRotation = (_currentAbsRotation + 90) % 360;
            _startRotation = transform.rotation;
            _isRotating = true;
            OnStartRotation?.Invoke(this);
        }

        public void CancelRotate()
        {
            if (!_isRotating) return;
            _currentAbsRotation = (_currentAbsRotation - 90 + 360) % 360;
            transform.rotation = _startRotation;
            _isRotating = false;
            OnEndRotation?.Invoke(this);
        }

        private void RotateRoom()
        {
            
            Quaternion targetRotation = Quaternion.Euler(0, _startRotation.eulerAngles.y + 90, 0);
            float rotationSpeed = 90 / 1.2f;
            if(_isRotating)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
                {
                    transform.rotation = targetRotation;
                    _isRotating = false;
                    OnRotationChanged?.Invoke(this);
                    OnEndRotation?.Invoke(this);
                    Debug.Log("Rotasion");
                }
            }
        }     

        public Door GetDoor(Layer layer, Direction dir) //Em demanen per la north del món
        {
            Direction realDir = GetRotatedDirection(dir); //Però si està rotada 90 graus, el que volen és la meva west
            return _doors[(int)layer, (int)realDir];
        }

        private Direction GetRotatedDirection(Direction dir)
        {
            int steps = _currentAbsRotation / 90;
            return (Direction)(((int)dir - steps + 4) % 4);
        }

        private void CacheDoors()
        {
            if (editorDoors[0] == null) editorDoors[0] = new LayerDoors();
            if (editorDoors[1] == null) editorDoors[1] = new LayerDoors();

            for (int i = 0; i < 4; i++)
            {
                Direction direction = (Direction)i;
                _doors[(int)Layer.Up, i] = editorDoors[1].GetDoor(direction);
                _doors[(int)Layer.Down, i] = editorDoors[0].GetDoor(direction);
            }   
        }

        private void OnDestroy()
        {
            RoomRegistry.RemoveRoom(this);
            OnEndRotation?.Invoke(this);
        }
    }
}

