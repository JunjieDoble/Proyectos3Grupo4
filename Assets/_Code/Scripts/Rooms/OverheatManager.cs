using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rooms
{
    public class OverheatManager : MonoBehaviour
    {
        private float _overheatValue;
        private const float MaxOverheatValue = 100f;
        private HashSet<Room> _overheatingRooms = new HashSet<Room>(); //There can be multiple rooms rotating at once, for example the buggy alien
        [SerializeField] private float _overheatIncreaseMultiplier = 15f;
        [SerializeField] private float _overheatDecreaseMultiplier = 10f;

        private bool _wasPreviousZero = false; //to avoid invoking OnOverheatValueChanged every frame when at 0
        private bool _wasAtMax = false; //to avoid invoking OnOverheatMaxed every frame when at max

        public static Action<float> OnOverheatValueChanged;
        public static Action OnOverheatMaxed;

        private void OnEnable()
        {
            Room.OnStartRotation += HandleStartRotation;
            Room.OnEndRotation += HandleEndRotation;
        }

        private void OnDisable()
        {
            Room.OnStartRotation -= HandleStartRotation;
            Room.OnEndRotation -= HandleEndRotation;
        }

        private void Update()
        {
            int overheatingSources = _overheatingRooms.Count;
            if (overheatingSources > 0)
            {
                _overheatValue += Time.deltaTime * overheatingSources * _overheatIncreaseMultiplier;
                _overheatValue = Mathf.Clamp(_overheatValue, 0f, MaxOverheatValue);
            }
            else
            {
                _overheatValue -= Time.deltaTime * _overheatDecreaseMultiplier;
                _overheatValue = Mathf.Clamp(_overheatValue, 0f, MaxOverheatValue);
            }
           
            if (_overheatValue == 0f)
            {
                if (!_wasPreviousZero)
                {
                    OnOverheatValueChanged?.Invoke(0f);
                    _wasPreviousZero = true;
                }
            }
            else
            {
                _wasPreviousZero = false;
                OnOverheatValueChanged?.Invoke(_overheatValue / MaxOverheatValue);
            }

            bool isAtMax = _overheatValue >= MaxOverheatValue;
            if (isAtMax && !_wasAtMax)
            {
                OnOverheatMaxed?.Invoke(); //TODO: de moment no ho rep ningú
            }
            _wasAtMax = isAtMax;
        }

        private void HandleStartRotation(Room room)
        {
            _overheatingRooms.Add(room);
        }

        private void HandleEndRotation(Room room)
        {
            _overheatingRooms.Remove(room);
        }
    }
}