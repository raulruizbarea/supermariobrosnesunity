using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{
    // The flag to move
    public Transform flag;
    // To start moving the flag
    bool isDown = false;

    // Play the animation of going down, stop music and play stage clear
    public void AnimateFlag()
    {
        isDown = true;
        SoundSystem.ss.StopMusic();
        SoundSystem.ss.PlayStageclear();
    }

    void Update()
    {
        // If the flag has to move then go to the bottom position, checked in the Editor
        if (isDown) { 
            flag.localPosition = Vector3.MoveTowards(flag.localPosition, new Vector3(-0.43f, 0.8f ,0), 5 * Time.deltaTime);
        }
    }
}
