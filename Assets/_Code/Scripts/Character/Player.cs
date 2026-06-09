using System.Collections.Generic;
using UnityEngine;
using System;
using _Code.Scripts.Gameplay;

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

        public void OnEnable()
        {
            GameManager.OnPause += DisableControllers;
            GameManager.OnResume += EnableControllers;
        }
        
        public void OnDisable()
        {
            GameManager.OnPause -= DisableControllers;
            GameManager.OnResume -= EnableControllers;
        }

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
            DisableControllers();
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

        public void Pause()
        {
            if (_isDead) return;
            Debug.Log("Player.Pause() called");
            GameManager.Instance?.ShowPauseMenu();
        }
        
        private void EnableControllers()
        {
            foreach (IController controller in _controllers)
            {
                controller.Enable();
            }
        }
        
        private void DisableControllers()
        {
            foreach (IController controller in _controllers)
            {
                controller.Disable();
            }
        }
    }
}