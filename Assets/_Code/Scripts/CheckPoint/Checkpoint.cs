using System;
using _Code.Scripts.Character;
using _Code.Scripts.Interactions;
using Interactions;
using Unity.VisualScripting;
using UnityEngine;

namespace _Code.Scripts.CheckPoint
{
    public class Checkpoint : MonoBehaviour, IInteractable {
        [SerializeField]
        private FMODUnity.EventReference interactionSound;
        [SerializeField]
        private FMODUnity.EventReference respawnSound;
        public static Action OnCheckpointChange;
        private static Checkpoint CurrentCheckpoint { get; set; }
        private Vector3 _spawnPosition;
        private Player _player;
        
        public GameObject GameObject => gameObject;

        public void Awake() {
            if (CurrentCheckpoint == null) {
                CurrentCheckpoint = this;
                _player = FindFirstObjectByType<Player>();
                _spawnPosition = _player.transform.position;
            }
        }

        public void Interact(IInteractor interactor) {
            if (interactor is PlayerInteractor playerInteractor) {
                OnCheckpointChange?.Invoke();
                CurrentCheckpoint = this;
                _spawnPosition = playerInteractor.Transform.position;
                _player = playerInteractor.GetComponent<Player>();
                if (!interactionSound.IsNull)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(interactionSound, transform.position);
                }
            }
        }

        public static void RespawnPlayer() {
            if (CurrentCheckpoint == null) return;
            CurrentCheckpoint._player.Revive(CurrentCheckpoint._spawnPosition);
            if (!CurrentCheckpoint.respawnSound.IsNull)
            {
                FMODUnity.RuntimeManager.PlayOneShot(CurrentCheckpoint.respawnSound, CurrentCheckpoint.transform.position);
            }
            Debug.Log("Respawned player at " + CurrentCheckpoint._spawnPosition);
        }

        public void OnTriggerEnter(Collider other) {
            if (other.GetComponent<Player>() == null) return;
            OnCheckpointChange?.Invoke();
            CurrentCheckpoint = this;
            _spawnPosition = this.transform.position;
            _player = other.GetComponent<Player>();
            Debug.Log("New checkpoint at " + _spawnPosition);
        }
    }
}

