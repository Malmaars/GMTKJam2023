using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SFX
{
    Bleep,
    Bloop,
}

public class GlobalSfxPlayer : MonoBehaviour
{
    public static GlobalSfxPlayer instance;
    
    public SoundEffect[] soundEffects;
    private AudioSource audioSource;
    
    private void Awake()
    {
        if (instance == null)
        {
            audioSource = GetComponent<AudioSource>();
            
            instance = this;
        }
        else Destroy(gameObject);
    }

    public void Play(SFX sfx)
    {
        SoundEffect sound = soundEffects.FirstOrDefault(s => s.sfx == sfx);
        if (sound != null)
        {
            audioSource.clip = sound.clip;
            audioSource.Play();
        }
    }
}

[Serializable]
public class SoundEffect
{
    public SFX sfx;
    public AudioClip clip;
}