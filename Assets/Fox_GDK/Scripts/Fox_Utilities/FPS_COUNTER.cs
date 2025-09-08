using TMPro;
using UnityEngine;

public class FPS_COUNTER : FoxObject
{
    public TMP_Text fpsDisplay;

    float interval = 0.25f;
    float minFps = 100f, maxFPS = 0f;
    float currentCoolDown;

    void Update()
    {
        // Calculates the time between frames
        currentCoolDown -= Time.deltaTime;

        if (currentCoolDown <= 0)
        {
            float currentFPS = (int)(1.0f / Time.deltaTime);
            minFps = minFps > currentFPS ? currentFPS : minFps;
            maxFPS = maxFPS < currentFPS ? currentFPS : maxFPS;

            fpsDisplay.text = $"| FPS: {currentFPS} |";
            currentCoolDown = interval;
        }
    }
}
