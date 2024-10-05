using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public Sound[] musicSounds, sfxSounds;
    public AudioSource[] musicSource;
    public AudioSource sfxSource;

    public float customLoopTime = 5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        for (int i = 0; i < musicSource.Length; i++)
        {
            musicSource[i].clip = musicSounds[i].clip;
            musicSource[i].Play();
            customLoopTime = customLoopTime + i;
            if(i != 1) {
                StartCoroutine(HandleCustomLoop(musicSource[i], customLoopTime));
            }
        }
    }

    private IEnumerator HandleCustomLoop(AudioSource source, float loopTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(source.clip.length);
            yield return new WaitForSeconds(loopTime);
            source.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfxSounds, s => s.name == name);

        if (sound == null)
        {
            Debug.Log("Sound not found: " + name);
        }
        else
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }


}
