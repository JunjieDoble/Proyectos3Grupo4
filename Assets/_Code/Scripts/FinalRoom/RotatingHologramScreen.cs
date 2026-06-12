using TMPro;
using UnityEngine;

public class RotatingHologramScreen : MonoBehaviour
{
    public TMP_Text north;
    public TMP_Text east;
    public TMP_Text south;
    public TMP_Text west;

    public string message = "";

    private float timer;
    public float speed = 10f;

    private int offset;

    void Update()
    {
        timer += Time.deltaTime * speed;

        if (timer >= 1f)
        {
            timer = 0f;
            offset++;

            UpdateDisplays();
        }
    }

    void UpdateDisplays()
    {
        north.text = GetSegment(offset + 0);
        east.text = GetSegment(offset + 15);
        south.text = GetSegment(offset + 30);
        west.text = GetSegment(offset + 45);
    }

    string GetSegment(int start)
    {
        string result = "";

        for (int i = 0; i < 15; i++)
        {
            int index = (start + i) % message.Length;
            result += message[index];
        }

        return result;
    }
}