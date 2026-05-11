using System;
using System.Threading;
using UnityEngine;
using _Code.Scripts.Gameplay;
using _Code.Scripts.Rooms;
using TMPro;
using UnityEngine.UI;
using Rooms;

namespace _Code.Scripts.UI
{
    public class MainHUD : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI countdownText;
        [SerializeField] private Image _overheatBarFill;
        
        void OnEnable()
        {
            Countdown.OnCountdownUpdate += UpdateCountdown;
            OverheatManager.OnOverheatValueChanged += UpdateOverheatBar;
        }

        private void OnDisable()
        {
            Countdown.OnCountdownUpdate -= UpdateCountdown;
            OverheatManager.OnOverheatValueChanged -= UpdateOverheatBar;
        }

        private void Awake()
        {
            if (countdownText == null) Debug.LogError("No countdown text assigned in MainHUD");
            if (_overheatBarFill == null) Debug.LogError("No overheat bar fill assigned in MainHUD");
            UpdateOverheatBar(0f);
        }

        private void UpdateCountdown(float remainingTime)
        {
            String time = TimeSpan.FromSeconds(remainingTime).ToString(@"mm\:ss");
            countdownText.SetText(time);
        }

        private void UpdateOverheatBar(float overheatValue)
        {
            _overheatBarFill.fillAmount = overheatValue;
        }
    }
}
