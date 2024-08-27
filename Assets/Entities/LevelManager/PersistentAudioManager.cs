using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentAudioManager : MonoBehaviour
{
    public static PersistentAudioManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Preserve the object across scenes
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void ChangeMusic(AudioClip newClip, bool loop = true)
    {
        PlayMusic(newClip, loop);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
            else
                audioSource.Play();
        }
    }
}
