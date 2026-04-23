using UnityEngine;

namespace _Code.Scripts.Character
{
    public interface IDie
    {
        public void Die();
        public bool IsDead();
        public void Revive(Vector3 spawnPoint);
    }
}
