using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    

    public AudioSource mainAudioChannel;
    public AudioSource backgroundMusicChannel;
    public AudioClip jumpSound;
    public AudioClip scoreSound;
    public AudioClip loseSound;
    public AudioClip ButtonSound;
}
