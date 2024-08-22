using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public AudioSource hitSound;

    public AudioSource crowdSound;
    
    public AudioSource goalSound;
    public AudioSource goalSavedSound;
    public AudioSource goalKeeperSound;
    public AudioSource goalSavedKeeperSound;

    public AudioSource ambienceSound;
    public AudioSource goalPlayerSound;
    public AudioSource goalPlayerSavedSound;
    public AudioSource Key;
    public AudioSource Coin;
    public AudioSource boo;
    
    public AudioSource puckHitSound;

    public AudioSource buttonClickSound;
    public void PlayCrownSound()
    {
        ambienceSound.Pause();
        crowdSound.Play();
    }
    
    public void PlayHitSound()
    {
        hitSound.Play();
    }
    public void KeyHitSound()
    {
        Key.Play();
    }
    public void CoinHitSound()
    {
        Coin.Play();
    }

    public void BooSound()
    {
        boo.Play();
    }

    public void PlayButtonClickSound()
    {
        buttonClickSound.Play();
    }

    public void PlayGoalSound()
    {
        goalSound.Play();
    }

    public void PlayGoalSavedSound()
    {
        goalSavedSound.Play();
    }
    public void PlayPuckHitSoundSound()
    {
        puckHitSound.Play();
    }

    public void PlayGoalKeeperSound(float delay)
    {
        goalKeeperSound.PlayDelayed(delay);
    } 
    
    public void PlayGoalSavedKeeperSound(float delay)
    {
        goalSavedKeeperSound.PlayDelayed(delay);
    }
    public void PlayGoalPlayerSound(float delay)
    {
        goalPlayerSound.PlayDelayed(delay);
    }
    public void PlayGoalSavedPlayerSound(float delay)
    {
        goalPlayerSavedSound.PlayDelayed(delay);
    }
}
