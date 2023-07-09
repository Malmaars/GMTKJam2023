using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;
    
    public AudioSource lelePlayer;
    public AudioSource drumsPlayer;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void OnRageMeterChange(float newValue)
    {
        drumsPlayer.volume = newValue;
    }
}
