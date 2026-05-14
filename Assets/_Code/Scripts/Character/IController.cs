using UnityEngine;

namespace _Code.Scripts.Character
{
    public interface IController
    {
        public bool IsEnabled { get; set; }
        void Enable() => IsEnabled = true;
        void Disable() => IsEnabled = false;
        
        void LoadPlayerParameters (PlayerParameters playerParameters);
        void OnPlayerRespawn(Vector3 spawnPoint);
    }
}