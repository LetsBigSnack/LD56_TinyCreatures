using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public Sound[] musicSounds, sfxSounds;
    public AudioSource[] musicSource;
    public AudioSource sfxSource;

    public float customLoopTime = 5f;

    [SerializeField]
    private AudioMixer myMixer;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private bool muffledMusic = false;

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
        SetMusicVolume();
        SetSfxVolume();
    }

    public void PlayBackgroundMusic()
    {
        for (int i = 0; i < musicSource.Length; i++)
        {
            musicSource[i].clip = musicSounds[i].clip;
            musicSource[i].Play();
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

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume)*20);
    }

    public void SetSfxVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
    }

    public void changeBackground()
    {
        if (muffledMusic)
        {
            myMixer.SetFloat("lowpass", 5000f);
            myMixer.SetFloat("distortion", 0.25f);
        }
        if (!muffledMusic)
        {
            myMixer.SetFloat("lowpass", 700f);
            myMixer.SetFloat("distortion", 0f);
        }

        muffledMusic = !muffledMusic;
    }


}
