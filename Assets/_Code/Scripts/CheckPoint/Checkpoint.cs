using System;
using _Code.Scripts.Character;
using _Code.Scripts.Interactions;
using Interactions;
using UnityEngine;

namespace _Code.Scripts.CheckPoint
{
    public class Checkpoint : MonoBehaviour, IInteractable
    {
        public static Action OnCheckpointChange;
        private static Checkpoint CurrentCheckpoint { get; set; }
        private Vector3 _spawnPosition;
        private Player _player;

        public void Awake() {
            if (CurrentCheckpoint == null)
            {
                CurrentCheckpoint = this;
                _player = FindFirstObjectByType<Player>();
                _spawnPosition = _player.transform.position;
            }
        }

        public void Interact(IInteractor interactor)
        {
            if (interactor is PlayerInteractor playerInteractor)
            {
                OnCheckpointChange?.Invoke();
                CurrentCheckpoint = this;
                _spawnPosition = playerInteractor.Transform.position;
                _player = playerInteractor.GetComponent<Player>();
            }
        }
    
        public static void RespawnPlayer()
        {
            if (CurrentCheckpoint == null) return;
            CurrentCheckpoint._player.Revive(CurrentCheckpoint._spawnPosition);
        }
    
    }
}
