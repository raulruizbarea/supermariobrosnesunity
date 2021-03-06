﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public static int points = 0;
    public static int coins = 0;
    public static int lifes = 3;

    public GameObject axeGame;
    public GameObject axeExit;

    bool isGame;

    void Start()
    {
        // Default values for lifes coins and points
        lifes = 3;
        coins = 0;
        points = 0;
        // Default value play game
        isGame = true;
        axeGame.SetActive(isGame);
        axeExit.SetActive(!isGame);
    }

    void Update()
    {
        // Move between options play or exit
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            isGame = !isGame;
            print(isGame);

            axeGame.SetActive(isGame);
            axeExit.SetActive(!isGame);
        }
        // If return or space go to option selected
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            if(isGame) { 
                SceneManager.LoadScene("level");
            }
            else
            {
                Application.Quit();
            }
        }
    }
}
