using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text lifesText;
    public Text timeText;
    public Text coinsText;
    public Text pointsText;

    public GameObject pause;
    bool isPause = false;

    float time = 400f;

    GameObject mario;

    void Start()
    {
        mario = GameObject.FindGameObjectWithTag("mario");
        pointsText.text = TitleManager.points.ToString().PadLeft(6, '0');
        coinsText.text = TitleManager.coins.ToString().PadLeft(2, '0');
        pause.SetActive(isPause);
        SoundSystem.ss.PlayMusic();
    }


    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            timeText.text = time.ToString("f0");
        }

        if (time <= 0f)
        {
            mario.GetComponent<Movement>().marioDeath = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundSystem.ss.PlayPause();
            Pause();
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
    }
}
