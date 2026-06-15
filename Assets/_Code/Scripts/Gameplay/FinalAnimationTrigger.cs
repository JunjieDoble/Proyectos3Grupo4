using System.Linq;
using _Code.Scripts.Character;
using _Code.Scripts.Gameplay;
using Unity.VisualScripting;
using UnityEngine;

public class FinalAnimationTrigger : MonoBehaviour
{
    
    public void DestroyPlayer()
    {
        Destroy(FindAnyObjectByType(typeof(Player)).GameObject());
    }
    
    public void GoToMainMenu()
    {
        GameManager.Instance?.ReturnToMenu();
    }
}
