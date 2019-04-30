using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSounds : MonoBehaviour
{
    public AudioClip DeathSound1;
    public AudioClip DeathSound2;
    public AudioClip DeathSound3;
    public AudioClip DeathSound4;
    public AudioClip DeathSound5;

    private AudioClip[] sounds;

    private void Start()
    {
        sounds = new AudioClip[] { DeathSound1, DeathSound2, DeathSound3, DeathSound4, DeathSound5 };
    }

    public void Play(AudioSource source) {
        int random = Random.Range(0, 4);
        source.PlayOneShot(sounds[random]);
    }

}
