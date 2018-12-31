using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public static SoundSystem ss;
    // All audios
    public AudioSource audioLevel;
    public AudioSource audioCoin;
    public AudioSource audioJump;
    public AudioSource audioPause;
    public AudioSource audioLifeUp;
    public AudioSource audioBlockBreak;
    public AudioSource audioPowerUp;
    public AudioSource audioDeath;
    public AudioSource audioFireball;
    public AudioSource audioMushroom;
    public AudioSource audioGoomba;
    public AudioSource audioBump;
    public AudioSource audioGameover;
    public AudioSource audioWorldclear;
    public AudioSource audioStageclear;
    public AudioSource audioKick;

    void Awake()
    {
        if(ss == null)
        {
            ss = this;
        } else if(ss != this)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        ss = null;
    }

    public void PlayMusic()
    {
        audioLevel.Play();
    }

    public void PlayGameover()
    {
        audioGameover.Play();
    }

    public void PlayKick()
    {
        audioKick.Play();
    }

    public void PlayWorldclear()
    {
        audioWorldclear.Play();
    }

    public void PlayStageclear()
    {
        audioStageclear.Play();
    }

    public void StopMusic()
    {
        audioLevel.Stop();
    }

    public void PlayCoin()
    {
        audioCoin.Play();
    }

    public void PlayJump()
    {
        audioJump.Play();
    }

    public void PlayPause()
    {
        audioPause.Play();
    }

    public void PlayLifeUp()
    {
        audioLifeUp.Play();
    }

    public void PlayBlockBreak()
    {
        audioBlockBreak.Play();
    }

    public void PlayFireball()
    {
        audioFireball.Play();
    }

    public void PlayPowerUp()
    {
        audioPowerUp.Play();
    }

    public void PlayDeath()
    {
        audioDeath.Play();
    }

    public void PlayMushroom()
    {
        audioMushroom.Play();
    }

    public void PlayGoomba()
    {
        audioGoomba.Play();
    }

    public void PlayBump()
    {
        audioBump.Play();
    }
}
