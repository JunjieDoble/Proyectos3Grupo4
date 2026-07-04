using System;
using System.Collections.Generic;
using System.Linq;
using _Code.Scripts.CheckPoint;
using _Code.Scripts.Gameplay;
using UnityEngine;

namespace _Code.Scripts.Rooms
{
    public class Room : MonoBehaviour
    {
        public static Action OnStartRotation;
        public static Action OnEndRotation;
        public static Action OnRoomReset;
        
        [Header("Room Properties")] 
        [SerializeField]
        private float rotationTime = 1.2f;
        [SerializeField]
        private float cancelSpeedMultiplier = 2f;
        [SerializeField]
        private float rotationDegree = 90;
        [SerializeField]
        private Vector3 rotatorVector = new (0, 1, 0);
        [SerializeField]
        private FMODUnity.EventReference interactionSound;
        private FMOD.Studio.EventInstance _interactionSoundInstance;
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
            Checkpoint.OnCheckpointChange += UpdateOrigin;
        }
        
        void OnDisable()
        {
            GameManager.OnPlayerRespawn -= ResetRoom;
            Checkpoint.OnCheckpointChange -= UpdateOrigin;
            if (_interactionSoundInstance.isValid())
            {
                _interactionSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                _interactionSoundInstance.release();
            }
        }       
        
        void ResetRoom()
        {
            transform.rotation = _originalRotation;
            if (_interactionSoundInstance.isValid())
            {
                _interactionSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                _interactionSoundInstance.release();
            }
            OnRoomReset?.Invoke();
        }
        
        void UpdateOrigin()
        {
            _originalRotation = _targetRotation;
        }

        private void FixedUpdate()
        {
            RotateRoom();
        }

        public void StartRotate(float? degree = null, float? time = null, Vector3? axis = null, float? cancelSpeed = null)
        {
            if (degree.HasValue) rotationSpeed = degree.Value / (time ?? rotationTime);
            if (cancelSpeed.HasValue) cancelSpeedMultiplier = cancelSpeed.Value;
            if (!_isRotating) _startRotation = transform.rotation;
            _targetRotation = _startRotation * Quaternion.AngleAxis(degree ?? rotationDegree, axis ?? rotatorVector);
            StartRotation();
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
                    EndRotation(_targetRotation);
                }
            }
        }

        private void StartRotation()
        {
            _isRotating = true;
            if (!interactionSound.IsNull)
            {
                _interactionSoundInstance = FMODUnity.RuntimeManager.CreateInstance(interactionSound);
                _interactionSoundInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
                _interactionSoundInstance.start();
            }
            OnStartRotation?.Invoke();
            foreach (Wall wall in _walls)
            {
                wall.OnRoomRotationStarted();
            }
            _speedMultiplier = 1f;
        }

        private void EndRotation(Quaternion targetRotation)
        {
            transform.rotation = targetRotation;
            _isRotating = false;
            if (_interactionSoundInstance.isValid())
            {
                _interactionSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                _interactionSoundInstance.release();
            }
            OnEndRotation?.Invoke();
            foreach (Wall wall in _walls)
            {
                wall.OnRoomRotationEnded();
            }
        }
    }
}

