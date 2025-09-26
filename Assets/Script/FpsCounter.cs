using System.Timers;
using UnityEngine;
using TMPro;

public class SimpleFPSCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;

    private float timer = 0f;
    private float updateInterval = 0.5f;

    void Update()
    {
        timer += Time.unscaledDeltaTime;
        if (timer >= updateInterval)
        {
            float fps = 1f / Time.unscaledDeltaTime;
            fpsText.text = "FPS: " + Mathf.RoundToInt(fps);
            timer = 0f;
        }
    }
}

