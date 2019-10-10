using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource effects;

    public AudioSource music;

    public static AudioManager instance;

    public float lowPitchRange;
    public float highPitchRange;
    private float defaultPitch = 0;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else if (instance != this)
        {
            Destroy(gameObject);
        }


        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Play the passed clip with the audio manager
    /// </summary>
    /// <param name="clip"></param>
    public void ChangeSong(AudioClip clip)
    {
        music.clip = clip;
        music.Play();
    }

    /// <summary>
    /// Play the passed clip with the audio manager, you can set the volume between 0f and 1f
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="volume"></param>
    public void ChangeSong(AudioClip clip, float volume)
    {
        music.clip = clip;
        music.volume = volume;
        music.Play();
    }

    /// <summary>
    /// Play a sound effect once.
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySfx(AudioClip clip)
    {
        effects.pitch = defaultPitch;
        effects.clip = clip;
        effects.Play();
    }

    /// <summary>
    /// Play a sound effect once, you can set the volume between 0f and 1f
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="volume"></param>
    public void PlaySfx(AudioClip clip, float volume)
    {
        effects.pitch = defaultPitch;
        effects.clip = clip;
        effects.volume = volume;
        effects.Play();
    }

    /// <summary>
    /// Play a sound effect with a random pitch
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySfxWithRandomPitch(AudioClip clip)
    {
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        effects.pitch = randomPitch;
        effects.clip = clip;
        effects.Play();

    }
}
