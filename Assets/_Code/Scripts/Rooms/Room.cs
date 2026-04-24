using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Code.Scripts.Rooms
{
    public class Room : MonoBehaviour
    {
        public static Action OnStartRotation;
        public static Action OnEndRotation;
        private Quaternion _startRotation; //before hold
        private bool _isRotating;
        private int _currentAbsRotation; //0, 90, 180, 270
        private List<Wall> _walls = new();

        void Awake()
        {
            _walls = GetComponentsInChildren<Wall>().ToList();
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
            OnStartRotation?.Invoke();
            foreach (Wall wall in _walls)
            {
                wall.OnRoomRotationStarted();
            }
        }

        public void CancelRotate()
        {
            if (!_isRotating) return;
            _currentAbsRotation = (_currentAbsRotation - 90 + 360) % 360;
            transform.rotation = _startRotation;
            _isRotating = false;
            OnEndRotation?.Invoke();
            foreach (Wall wall in _walls)
            {
                wall.OnRoomRotationEnded();
            }
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
                    Debug.Log("Rotation finished");
                    transform.rotation = targetRotation;
                    _isRotating = false;
                    OnEndRotation?.Invoke();
                    foreach (Wall wall in _walls)
                    {
                        wall.OnRoomRotationEnded();
                    }
                }
            }
        }     
    }
}

