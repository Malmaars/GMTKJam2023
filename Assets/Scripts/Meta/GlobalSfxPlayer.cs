using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SFX
{
    Shovel,
    Crow,
    FlyingBush,
    Splash,
    LightGrunt,
    HeavyGrunt,
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

    public void Play(SFX sfx, float volume = 1.0f)
    {
        SoundEffect sound = soundEffects.FirstOrDefault(s => s.sfx == sfx);
        if (sound != null)
        {
            audioSource.volume = volume;
            // audioSource.clip = ;
            audioSource.PlayOneShot(sound.clips[Random.Range(0, sound.clips.Length)]);
        }
    }
}

[Serializable]
public class SoundEffect
{
    public SFX sfx;
    public AudioClip[] clips;
}