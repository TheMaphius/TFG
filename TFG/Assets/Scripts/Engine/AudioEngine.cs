using UnityEngine;
using System.Collections;

public class AudioEngine : MonoBehaviour {

    public AudioClip[] audio_background;
    public float volume;
    public int priority;

    private IEnumerator coroutine;

    void Start()
    {
        audio.loop = true;
        audio.volume = volume;
        audio.priority = priority;
    }

    public void MainAudio()
    {
        StartCoroutine(coroutine = Play(audio_background[0]));
    }

    public void BackgroundAudio()
    {
        StartCoroutine(coroutine = Play(audio_background[1]));
    }

    public void BossAudio()
    {
        StartCoroutine(coroutine = Play(audio_background[2]));
    }

    public void GameOverAudio()
    {
        StartCoroutine(coroutine = Play(audio_background[3]));
    }


    public void OptionsAudio()
    {
        StartCoroutine(coroutine = Play(audio_background[4]));
    }

    private void CheckAudio()
    {
        bool isMute = (PlayerPrefs.GetInt("sound") == 0) ? true : false;

        AudioSource[] audio = FindObjectsOfType<AudioSource>();

        for (int i = 0; i < audio.Length; i++)
            audio[i].mute = isMute;
    }

    private IEnumerator Play(AudioClip clip)
    {
        while(true)
        {
            audio.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }
    }

    public void Stop()
    {
        audio.Stop();
        if(coroutine != null)
            StopCoroutine(coroutine);
    }

    void OnEnable()
    {
        CheckAudio();
    }
}
