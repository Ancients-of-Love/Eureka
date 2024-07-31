using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : Singleton<Audio>
{
    public AudioSource audioSource;

    public bool ChangeAudio = false;

    public AudioClip[] audioClips;
    public AudioClip CurrentlyPlaying;

    public void PlayIntenseMusic()
    {
        if (CurrentlyPlaying == audioClips[1])
        {
            return;
        }
        audioSource.clip = audioClips[1];
        CurrentlyPlaying = audioClips[1];
        audioSource.Play();
    }

    public void PlayBaseMusic()
    {
        if (CurrentlyPlaying == audioClips[0])
        {
            return;
        }
        audioSource.clip = audioClips[0];
        CurrentlyPlaying = audioClips[0];
        audioSource.Play();
    }

    public void PlayDamagingMusic()
    {
        if (CurrentlyPlaying == audioClips[2])
        {
            return;
        }
        audioSource.clip = audioClips[2];
        CurrentlyPlaying = audioClips[2];
        audioSource.Play();
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}