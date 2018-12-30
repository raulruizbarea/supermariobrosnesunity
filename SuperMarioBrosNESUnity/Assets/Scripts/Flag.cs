using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{
    public Transform flag;
    bool isDown = false;

    public void AnimateFlag()
    {
        isDown = true;
        SoundSystem.ss.StopMusic();
        SoundSystem.ss.PlayStageclear();
    }
    // Update is called once per frame
    void Update()
    {
        if (isDown) { 
            flag.localPosition = Vector3.MoveTowards(flag.localPosition, new Vector3(-0.43f, 0.8f ,0), 5 * Time.deltaTime);
        }
    }
}
