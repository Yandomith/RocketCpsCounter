using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    public ClickCounter clickCounter;
    public RocketDialog rocketDialog;
    public Button clickButton;
    public Button startButton;
    public Button resetBtn;
    public TextMeshProUGUI timerText;

    private float countdown = 3f;
    private bool isCounting = false;
    public static GameManager instance;
    
    public TextMeshProUGUI highScoreText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("HighCPS"))
        {
            int savedHighCPS = PlayerPrefs.GetInt("HighCPS");
            highScoreText.text = "High CPS: " + savedHighCPS;
        }
        else
        {
            highScoreText.text = "High CPS: 0";
        }
        clickButton.gameObject.SetActive(false);
        resetBtn.gameObject.SetActive(false);
        timerText.text = "Ready?";
    }

    public void StartClickPhase()
    {
        clickCounter.clickCount = 0;
        countdown = 3f;
        isCounting = true;
        
        clickButton.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
        resetBtn.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isCounting)
        {
            countdown -= Time.deltaTime;
            timerText.text = "Time: " + countdown.ToString("F1");

            if (countdown <= 0)
            {
                isCounting = false;
                EndClickPhase();
            }
        }
    }

    void EndClickPhase()
    {
        StartCoroutine(EndClickPhaseCoroutine());
    }

    IEnumerator EndClickPhaseCoroutine()
    {
        clickButton.gameObject.SetActive(false);
        timerText.text = "Done!";

        int finalCPS = clickCounter.GetCurrentCPS();
        Debug.Log("Final CPS: " + finalCPS);

        // Wait 2 seconds before launching rocket
        yield return new WaitForSeconds(2f);

        // Launch the rocket
        if (rocketDialog != null)
            rocketDialog.LaunchRocket();
    }

    public void Reset()
    {
        if (PlayerPrefs.HasKey("HighCPS"))
        {
            int savedHighCPS = PlayerPrefs.GetInt("HighCPS");
            highScoreText.text = "High CPS: " + savedHighCPS;
        }
        else
        {
            highScoreText.text = "High CPS: 0";
        }
        clickCounter.clickCount = 0;
        clickCounter.highestCPS = 0;
        clickCounter.cpsText.text = "CPS: 0(High: " + 0 + ")";
        clickCounter.clickCounterText.text = "Click Count: 0";
        clickButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
        resetBtn.gameObject.SetActive(false);
        timerText.text = "Ready?";
    }
}
