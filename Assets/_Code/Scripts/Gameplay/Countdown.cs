using System;
using UnityEngine;

namespace _Code.Scripts.Gameplay
{
    public class Countdown : MonoBehaviour
    {
        
        [SerializeField] bool startCountdown = true;
        [SerializeField] float defaultTimeSeconds = 120f;
        private float _remainingTime;
        
        public static Action OnCountdownEnd;
        public static Action<float> OnCountdownUpdate;

        public void StartCountdown(float time) => _remainingTime = time;
        
        void Start()
        {
            if (startCountdown) StartCountdown(defaultTimeSeconds);
        }
        
        void Update()
        {
            if (_remainingTime <= 0)
            {
                OnCountdownEnd?.Invoke();
                Destroy(gameObject);
            }
            _remainingTime -= Time.deltaTime;
            OnCountdownUpdate?.Invoke(_remainingTime);
        }
    }
}

