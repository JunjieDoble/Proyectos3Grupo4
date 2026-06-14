using TMPro;
using UnityEngine;

public class MenuRoboticStyleTextAuto : MonoBehaviour
{
    [SerializeField] private float cooldownDuration = 3f;
    [SerializeField] private float glitchingSpeed = 0.1f;
    [SerializeField] private float glitchingDuration = 1f;

    private TextMeshProUGUI textMeshPro;
    private bool isGlitching;

    private float detaltime;
    private float cooldownTimer;
    private float glitchingTimer;
    private float glitchingResetTimer;

    private const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        detaltime = Time.deltaTime;

        if (!isGlitching)
        {
            cooldownTimer += detaltime;
            if (cooldownTimer >= cooldownDuration)
            {
                isGlitching = true;
                cooldownTimer = 0f;
            }
        }
        else
        {
            glitchingTimer += detaltime;
            glitchingResetTimer += detaltime;

            if (glitchingResetTimer >= glitchingDuration)
            {
                isGlitching = false;
                glitchingResetTimer = 0f;
            }

            if (glitchingTimer >= glitchingSpeed)
            {
                int glitchingTextLength = Random.Range(6, 14);
                textMeshPro.text = GenerateRandomString(glitchingTextLength);
                glitchingTimer = 0f;
            }
            
        }
    }

    private string GenerateRandomString(int length)
    {
        char[] randomChars = new char[length];
        for (int i = 0; i < length; i++)
        {
            randomChars[i] = letters[Random.Range(0, letters.Length)];
        }
        return new string(randomChars);
    }
}
