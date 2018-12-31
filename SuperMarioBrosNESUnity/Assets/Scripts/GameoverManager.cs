using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameoverManager : MonoBehaviour
{
    // Show stats
    public Text coinsText;
    public Text pointsText;
    public Text gameoverText;

    void Start()
    {
        pointsText.text = TitleManager.points.ToString().PadLeft(6, '0');
        coinsText.text = TitleManager.coins.ToString().PadLeft(2, '0');

        // If we have more than 0 lifes is victory otherwise defeat
        if(TitleManager.lifes > 0)
        {
            gameoverText.text = "VICTORY";
            SoundSystem.ss.PlayWorldclear();
        }
        else
        {
            SoundSystem.ss.PlayGameover();
        }
        // Redirect to title
        StartCoroutine(Redirect());
    }

    public IEnumerator Redirect()
    {
        // Wait 7 seconds to redirect and listen the music and look stats
        yield return new WaitForSeconds(7f);
        SceneManager.LoadScene("title");
    }
}
