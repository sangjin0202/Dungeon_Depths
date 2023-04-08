using System.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource = null;
    public Slider volumeSlider;
    public AudioMixer mixer;

    public void SetVolume()
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(volumeSlider.value));
    }
    public bool IsOnPlay
    {
        get => audioSource.isPlaying;
    }
    public bool IsLoop
    {
        set => audioSource.loop = value;
        get => audioSource.loop;
    }

    public void ForcePlay(AudioClip audio)
    {
        if (IsOnPlay)
            audioSource.Stop();
        audioSource.clip = audio;
        audioSource.Play();
    }

    public void Play()
    {
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    private void OnEnable()
    {
        if (!audioSource)
            audioSource = GetComponent<AudioSource>();
    }
}