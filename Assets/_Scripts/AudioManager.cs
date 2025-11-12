using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioClip flipSFX, matchSFX, mismatchSFX, gameoverSFX;
     AudioSource AudioSource_SoundEffect;

    void Start()
    {
        AudioSource_SoundEffect = GetComponent<AudioSource>();
    }
    // --- Public Functions for Playing Sounds ---
    public void PlayCardFlip()
    {
        // Plays the flip sound. If another sound is currently playing, 
        // this one will play simultaneously without interrupting it.
        AudioSource_SoundEffect.PlayOneShot(flipSFX);
    }

    public void PlayMatch()
    {
        // Plays the match sound effect
        AudioSource_SoundEffect.PlayOneShot(matchSFX);
    }

    public void PlayMismatch()
    {
        // Plays the mismatch sound effect
        AudioSource_SoundEffect.PlayOneShot(mismatchSFX);
    }

    public void PlayGameOver()
    {
        // Plays the mismatch sound effect
        AudioSource_SoundEffect.PlayOneShot(gameoverSFX);
    }
}
