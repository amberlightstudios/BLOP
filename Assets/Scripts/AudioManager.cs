using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    // This plays the audio with its name being "name"
    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.clip.name == name);
        s.source.Play();
    }
}
