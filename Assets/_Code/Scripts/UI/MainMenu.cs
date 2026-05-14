using UnityEngine;
using _Code.Scripts.Gameplay;
namespace _Code.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        public void StartGame()
        {
            GameManager.Instance.StartGame();
        }

        public void Settings()
        {
            Debug.Log("Not implemented yet");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}