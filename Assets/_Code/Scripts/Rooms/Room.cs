using System;
using System.Collections.Generic;
using System.Linq;
using _Code.Scripts.Gameplay;
using UnityEngine;

namespace _Code.Scripts.Rooms
{
    public class Room : MonoBehaviour
    {
        public static Action OnStartRotation;
        public static Action OnEndRotation;

        [Header("Room Properties")] 
        [SerializeField]
        private float rotationTime = 1.2f;
        [SerializeField]
        private float cancelSpeedMultiplier = 2f;
        [SerializeField]
        private float rotationDegree = 90;
        [SerializeField]
        private Vector3 rotatorVector = new (0, 1, 0);
        private float rotationSpeed;
        private Quaternion _startRotation; //before hold
        private bool _isRotating;
        private Quaternion _targetRotation;
        private List<Wall> _walls = new();
        private float _speedMultiplier = 1f;
        
        private Quaternion _originalRotation;

        void Awake()
        {
            _walls = GetComponentsInChildren<Wall>().ToList();
            rotationSpeed = rotationDegree / rotationTime;
            _originalRotation = transform.rotation;
        }

        void OnEnable()
        {
            GameManager.OnPlayerRespawn += ResetRoom;
        }
        
        void OnDisable()
        {
            GameManager.OnPlayerRespawn -= ResetRoom;
        }       
        
        void ResetRoom()
        {
            _isRotating = true;
            _targetRotation = _originalRotation;
        }

        private void FixedUpdate()
        {
            RotateRoom();
        }

        public void StartRotate()
        {
            if (!_isRotating) _startRotation = transform.rotation;
            _targetRotation = _startRotation * Quaternion.AngleAxis(90, rotatorVector);
            _isRotating = true;
            OnStartRotation?.Invoke();
            foreach (Wall wall in _walls)
            {
                wall.OnRoomRotationStarted();
            }
            _speedMultiplier = 1f;
        }

        public void CancelRotate()
        {
            if (!_isRotating) return;
            _targetRotation = _startRotation;
            _speedMultiplier = cancelSpeedMultiplier;
        }

        private void RotateRoom()
        {
            if(_isRotating)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, rotationSpeed * _speedMultiplier * Time.deltaTime);
                if (Quaternion.Angle(transform.rotation, _targetRotation) < 0.1f)
                {
                    transform.rotation = _targetRotation;
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

