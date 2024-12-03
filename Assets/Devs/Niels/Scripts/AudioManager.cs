using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// Manages all audio playback in the game using a singleton pattern
public class AudioManager : MonoBehaviour
{
    /// Singleton instance of the AudioManager
    public static AudioManager instance;

    /// Audio mixer group for controlling audio output
    public AudioMixerGroup mixerGroup;

    /// Array of Sound objects containing all available audio clips
    public Sound[] sounds;

    /// Initializes the AudioManager and sets up audio sources for all sounds

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = mixerGroup;
        }
    }

    /// Plays the specified sound with randomized volume and pitch variations
    /// <param name="sound">Name of the sound to play</param>
    public void Play(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

    /// Stops the specified sound from playing
    /// <param name="sound">Name of the sound to stop</param>
    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return;
        }

        s.source.Stop();
    }

    /// Stops all currently playing sounds

    public void StopAllSounds()
    {
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
    }

    /// Checks if a specific sound is currently playing
    /// <param name="sound">Name of the sound to check</param>
    /// <returns>True if the sound is playing, false otherwise</returns>
    public bool IsPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return false;
        }

        return s.source.isPlaying;
    }
}