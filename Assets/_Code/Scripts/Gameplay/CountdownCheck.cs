using System;
using UnityEngine;

namespace _Code.Scripts.Gameplay
{
    public class CountdownCheck : MonoBehaviour
    {
    
        void OnEnable()
        {
            Countdown.OnCountdownEnd += CountdownEnded;
        }

        private void CountdownEnded()
        {
            Debug.Log("Countdown ended!");
        }
        
        void OnDisable()
        {
            Countdown.OnCountdownEnd -= CountdownEnded;
        }
    }

}
