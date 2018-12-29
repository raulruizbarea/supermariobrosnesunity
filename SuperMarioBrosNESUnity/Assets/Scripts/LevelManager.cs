using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Text lifesText;
    public Text coinsText;
    public Text pointsText;

    void Start()
    {
        lifesText.text = TitleManager.lifes.ToString();
        pointsText.text = TitleManager.points.ToString().PadLeft(6, '0');
        coinsText.text = TitleManager.coins.ToString().PadLeft(2, '0');
        StartCoroutine(Redirect());
    }

    public IEnumerator Redirect()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("world1-1");
    }
}
