using System.Collections.Generic;
using UnityEngine;
using System;
using _Code.Scripts.Interactions;

namespace _Code.Scripts.Character
{
    public class Player : MonoBehaviour, IDie
    {

        public static Action OnPlayerDied;

        [Header("References")]
        [SerializeField] private PlayerParameters playerParameters;
        
        private readonly List<IController> _controllers = new();
        private bool _isDead;
        public bool IsDead() => _isDead;

        public void Revive(Vector3 spawnPoint)
        {
            _isDead = false;
            foreach (IController controller in _controllers)
            {
                controller.Enable();
                controller.OnPlayerRespawn(spawnPoint);
            }
        }
        
        public void Die()
        {
            foreach (IController controller in _controllers)
            {
                controller.Disable();
            }
            OnPlayerDied?.Invoke();
            _isDead = true;
        }
        
        public void AddController(IController controller)
        {
            if (!playerParameters) throw new MissingReferenceException("PlayerParameters is not assigned in the inspector.");
            _controllers.Add(controller);
            controller.LoadPlayerParameters(playerParameters);
            controller.Enable();
        }
    }
}