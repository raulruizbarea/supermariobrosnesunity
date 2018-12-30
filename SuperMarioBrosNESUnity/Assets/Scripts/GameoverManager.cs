using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameoverManager : MonoBehaviour
{
    public Text coinsText;
    public Text pointsText;
    public Text gameoverText;

    void Start()
    {
        pointsText.text = TitleManager.points.ToString().PadLeft(6, '0');
        coinsText.text = TitleManager.coins.ToString().PadLeft(2, '0');

        if(TitleManager.lifes > 0)
        {
            gameoverText.text = "VICTORY";
            SoundSystem.ss.PlayWorldclear();
        }
        else
        {
            SoundSystem.ss.PlayGameover();
        }
        StartCoroutine(Redirect());
    }

    public IEnumerator Redirect()
    {
        yield return new WaitForSeconds(7f);
        SceneManager.LoadScene("title");
    }
}
