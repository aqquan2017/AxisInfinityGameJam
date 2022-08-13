using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public static TestManager Instance;
    
    private AudioSource _audioSource;
    public AudioClip _moveSound;
    public AudioClip _winSound;
    public AudioClip _openTransitionSound;
    public AudioClip _closeTransitionSound;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    public void PlaySound(AudioClip audioClip) => _audioSource.PlayOneShot(audioClip);
}
