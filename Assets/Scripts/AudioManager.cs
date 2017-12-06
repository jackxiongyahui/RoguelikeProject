using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public float minPitch = 0.9f;
    public float maxPicth = 1.1f;

    public AudioSource efxSource;
    public AudioSource bgSource;

	public void RandomPlay(params AudioClip[] clips)
    {
        float pitch = Random.Range(minPitch, maxPicth);
        int index = Random.Range(0, clips.Length);
        AudioClip clip = clips[index];
        efxSource.clip = clip;
        efxSource.pitch = pitch;
        efxSource.Play();
    }

    public void StopBgMusic()
    {
        bgSource.Stop();
    }
}
