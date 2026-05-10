using System.Collections.Generic;
using _Code.Scripts.CheckPoint;
using UnityEngine;

namespace _Code.Scripts.Character
{
    public class Player : MonoBehaviour, IDie
    {

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
                if (controller is MovementController movementController)
                {
                    movementController.Teleport(spawnPoint);
                }
            }
        }
        
        public void Die()
        {
            foreach (IController controller in _controllers)
            {
                controller.Disable();
            }
            Checkpoint.RespawnPlayer();
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