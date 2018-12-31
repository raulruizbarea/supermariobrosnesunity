using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Texts to show
    public Text lifesText;
    public Text timeText;
    public Text coinsText;
    public Text pointsText;
    public Text debugText;

    // Debug panel
    public GameObject debug;
    public GameObject debugPanel;
    public GameObject pause;
    bool isPause = false;
    bool isDebug = false;

    // Time fixed 400s
    float time = 400f;

    GameObject mario;

    void Start()
    {
        mario = GameObject.FindGameObjectWithTag("mario");
        pointsText.text = TitleManager.points.ToString().PadLeft(6, '0');
        coinsText.text = TitleManager.coins.ToString().PadLeft(2, '0');
        // Start with pause off
        pause.SetActive(isPause);
        debug.SetActive(isPause);
        debugPanel.SetActive(isPause);
        // Play level music
        SoundSystem.ss.PlayMusic();
    }

    void Update()
    {
        // Meanwhile the time is bigger than 0 if mario doesnt touch the flag continue decreasing time
        if (time > 0)
        {
            if(!mario.GetComponent<Movement>().isFlag) { 
                time -= Time.deltaTime;
                timeText.text = time.ToString("f0");
            }
        }
        else {
            // If the time finish mario dies
            mario.GetComponent<Movement>().marioDeath = true;
        }

        // Press escape to show pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundSystem.ss.PlayPause();
            Pause();
        }

        // If we are in pause and push Space change debug on off or off on and shows the panel
        if (Input.GetKeyDown(KeyCode.Space) && isPause)
        {
            isDebug = !isDebug;
            if(isDebug)
            {
                debug.GetComponent<Text>().text = "DEBUG ON";
            }
            else
            {
                debug.GetComponent<Text>().text = "DEBUG OFF";
            }
            debugPanel.SetActive(isDebug);
        }

        // If we are on pause time is 0 otherwise realtime
        if (isPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    // Update the debug text with the text provided
    public void UpdateDebug(string text)
    {
        debugText.text = text + "\n" + debugText.text;
    }

    // Update lifes plus one
    public void UpdateLifes()
    {
        TitleManager.lifes += 1;
    }

    // Update coins plus one if its 99 next one one life more and again 0
    public void UpdateCoins()
    {
        if (TitleManager.coins == 99)
        {
            TitleManager.coins = 0;
            UpdateLifes();

        }
        else {
            TitleManager.coins += 1;
        }

        coinsText.text = TitleManager.coins.ToString().PadLeft(2, '0');
    }

    // Update points with the points provided
    public void UpdatePoints(int pt)
    {
        TitleManager.points += pt;
        pointsText.text = TitleManager.points.ToString().PadLeft(6, '0');
    }

    // Show or hide pause and debug
    void Pause()
    {
        isPause = !isPause;
        pause.SetActive(isPause);
        debug.SetActive(isPause);
    }
}
