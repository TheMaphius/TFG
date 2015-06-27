/***************************************************************************/
/**** AUTOR:    Joshua García Palacios                                 *****/
/**** PROGRAMA:  TFG                                                   *****/
/**** VERSIO:    1.0                                                   *****/
/***************************************************************************/

using UnityEngine;
using System.Collections;

public class MusicAnimation : MonoBehaviour{

    public AudioClip[] sound;
    public bool mute;
    public bool repeat;
    //public bool isMenu;     // <== This var is for the zombies of menu so they can't execute the audio.
    
    private Animator anim;
    private AudioSource audio;
    private IEnumerator coroutine;

    private float delay;
    private string state;

    private bool play = false;
    private bool loop = true;

    void Awake() 
    {

        if (gameObject.GetComponent<AudioSource>() == null)
            gameObject.AddComponent<AudioSource>();

        anim = gameObject.GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        
    }

    void Start()
    {
        bool isMute = (PlayerPrefs.GetInt("sound") == 0) ? true : false;

        audio.playOnAwake = false;
        audio.loop = repeat;
        audio.mute = (mute == true) ? true : isMute;
        if(mute == true)
            audio.volume = 0;
    }

    public void Play(int track) 
    {
        // That condition prevents the state 'Any state' send various call to the actual state.
        if (track >= 0 && track <= sound.Length - 1)
                audio.PlayOneShot(sound[track]);
        
    }


    public void Play2(int track)
    {

        // That condition prevents the state 'Any state' send various call to the actual state.
        if (!play)
        {
            play = true;
            float delay = sound[track].length;

            if (track >= 0 && track <= sound.Length - 1)
                audio.PlayOneShot(sound[track]);

            // Waste time until track finish to play.
            StartCoroutine(Delay(delay));
        }
    }


   
    public void PlayLoop(int track)
    {
        // That condition prevents the state 'Any state' send various call to the actual state.
        if (coroutine != null)
            StopCoroutine(coroutine);

        loop = true;
        StartCoroutine(coroutine = Loop(track));
    }


    private IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        play = false;
    }

    private IEnumerator Loop(int track){
        
        bool currentState = true;

        while (loop && currentState)
        {
            audio.PlayOneShot(sound[track]);
            yield return new WaitForSeconds(delay);
            currentState = anim.GetCurrentAnimatorStateInfo(0).IsName(this.state);
        }

    }

    public void Stop()
    {
        audio.Stop();
    }

    private void StopLoop()
    {
        loop = false;

        if (coroutine != null)
            StopCoroutine(coroutine);

    }

    public void setDelay(float delay)
    {
        this.delay = delay;
    }

    public void setState(string state)
    {
        this.state = state;
    }

}
