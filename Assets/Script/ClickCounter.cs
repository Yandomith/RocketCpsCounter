using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ClickCounter : MonoBehaviour
{
    public TextMeshProUGUI clickCounterText;
    public TextMeshProUGUI cpsText;

    public int clickCount = 0;
    
    private List<float> clickTimes = new List<float>();
    private float timeWindow = 1f;
    public int highestCPS = 0;

    void Start()
    {
        highestCPS = PlayerPrefs.GetInt("HighCPS", 0);
    }


    public void OnClick()
    {
        clickCount++;
        clickCounterText.text = "Click Count: " + clickCount;
        clickTimes.Add(Time.time);
    }

    void Update()
    {
        float currentTime = Time.time;
        clickTimes.RemoveAll(t => currentTime - t > timeWindow);

        int currentCPS = clickTimes.Count;

        // Update highest CPS if current is greater
        if (currentCPS > highestCPS){
            highestCPS = currentCPS;
            updatePlayerprefs();
        }
        cpsText.text = "CPS: " + currentCPS + " (High: " + highestCPS + ")";
    }

    void updatePlayerprefs()
    {
        if (PlayerPrefs.HasKey("HighCPS"))
        {
            int savedHighCPS = PlayerPrefs.GetInt("HighCPS");
            if (highestCPS > savedHighCPS)
            {
                PlayerPrefs.SetInt("HighCPS", highestCPS);
            }
        }
        else
        {
            PlayerPrefs.SetInt("HighCPS", highestCPS);
        }
        PlayerPrefs.Save();
    }

    public int GetCurrentCPS()
    {
        float currentTime = Time.time;
        clickTimes.RemoveAll(t => currentTime - t > timeWindow);
        return clickTimes.Count;
    }
    public int GetHighestCPS()
    {
        return highestCPS;
    }

}