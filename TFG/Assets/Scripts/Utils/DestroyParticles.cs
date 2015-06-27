/***************************************************************************/
/**** AUTOR:    Joshua García Palacios                                 *****/
/**** PROGRAMA:  TFG                                                   *****/
/**** VERSIO:    1.0                                                   *****/
/***************************************************************************/

using UnityEngine;
using System.Collections;

public class DestroyParticles : MonoBehaviour {

    [Tooltip("Speed duration particle.")]
    public float speed = 1.0f;
    [Tooltip("Lifetime particle.")]
    public float destroyTime = 3.0f;
    [Tooltip("Audio effect of particle.")]
    public AudioClip audioEffect;

    private ParticleSystem particle;

    void Awake() 
    {
        if(audioEffect != null)
            gameObject.AddComponent<AudioSource>();

        particle = gameObject.GetComponent<ParticleSystem>();
    }

	void Start () 
    {
        if(particle != null)
            particleSystem.playbackSpeed = speed;
        
        if (audioEffect != null)
            audio.PlayOneShot(audioEffect);
        
        if(destroyTime > 0)
            Destroy(gameObject, destroyTime);
	}

    public void Play()
    {
        gameObject.particleEmitter.enabled = true;
    }
    
    public void Stop()
    {
        gameObject.particleEmitter.enabled = false;
    }

	
}
