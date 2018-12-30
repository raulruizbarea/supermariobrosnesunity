using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text lifesText;
    public Text timeText;
    public Text coinsText;
    public Text pointsText;
    public Text debugText;

    public GameObject debug;
    public GameObject debugPanel;
    public GameObject pause;
    bool isPause = false;
    bool isDebug = false;

    float time = 400f;

    GameObject mario;

    void Start()
    {
        mario = GameObject.FindGameObjectWithTag("mario");
        pointsText.text = TitleManager.points.ToString().PadLeft(6, '0');
        coinsText.text = TitleManager.coins.ToString().PadLeft(2, '0');
        pause.SetActive(isPause);
        debug.SetActive(isPause);
        debugPanel.SetActive(isPause);
        SoundSystem.ss.PlayMusic();
    }

    void Update()
    {
        if (time > 0)
        {
            if(!mario.GetComponent<Movement>().isFlag) { 
                time -= Time.deltaTime;
                timeText.text = time.ToString("f0");
            }
        }
        else {
            mario.GetComponent<Movement>().marioDeath = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundSystem.ss.PlayPause();
            Pause();
        }

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

        if (isPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void UpdateDebug(string text)
    {
        debugText.text = text + "\n" + debugText.text;
    }

    public void UpdateLifes()
    {
        TitleManager.lifes += 1;
        lifesText.text = TitleManager.lifes.ToString();
    }

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

    public void UpdatePoints(int pt)
    {
        TitleManager.points += pt;
        pointsText.text = TitleManager.points.ToString().PadLeft(6, '0');
    }

    void Pause()
    {
        isPause = !isPause;
        pause.SetActive(isPause);
        debug.SetActive(isPause);
    }
}
