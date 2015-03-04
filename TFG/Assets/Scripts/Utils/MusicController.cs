using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour{

    public AudioClip[] sound;

    void Awake() 
    {
        if (gameObject.GetComponent<AudioSource>() == null)
            gameObject.AddComponent<AudioSource>();
    }

    public void Play(int track) 
    {
        if(track <= sound.Length -1)
            audio.PlayOneShot(sound[track]);
    }
}
