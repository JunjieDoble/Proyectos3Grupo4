using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Code.Scripts.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] 
        private GameplayParameters parameters;
        public static GameManager Instance;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void StartGame()
        {
            SceneManager.LoadScene(parameters?.scenes[0] ?? "LlucScene");
        }
    }
}