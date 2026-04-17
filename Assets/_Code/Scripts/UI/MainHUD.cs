using System;
using System.Threading;
using UnityEngine;
using _Code.Scripts.Gameplay;
using TMPro;

namespace _Code.Scripts.UI
{
    public class MainHUD : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI countdownText;
        
        void OnEnable()
        {
            Countdown.OnCountdownUpdate += UpdateCountdown;
        }

        private void OnDisable()
        {
            Countdown.OnCountdownUpdate -= UpdateCountdown;
        }

        private void UpdateCountdown(float remainingTime)
        {
            String time = TimeSpan.FromSeconds(remainingTime).ToString(@"mm\:ss");
            countdownText.SetText(time);
        }
    }
}
