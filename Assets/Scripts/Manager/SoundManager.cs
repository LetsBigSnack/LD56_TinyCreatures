using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public Sound[] sfxSounds;
    public AudioSource sfxSource;

    [SerializeField]
    private AudioMixer myMixer;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;

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

    private void Start()
    {
        if (PlayerPrefs.HasKey("music") && PlayerPrefs.HasKey("sfx"))
        {
            SetVolumesFromPrefs();
        }
        else
        {
            SetMusicVolume();
            SetSfxVolume();
        }  
    }

    private void SetVolumesFromPrefs()
    {
        float musicVolume = PlayerPrefs.GetFloat("music");
        float sfxVolume = PlayerPrefs.GetFloat("sfx");
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
        myMixer.SetFloat("music", Mathf.Log10(musicVolume) * 20);
        myMixer.SetFloat("sfx", Mathf.Log10(sfxVolume) * 20);

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
        if(musicSlider == null)
        {
            Debug.Log("No music slider");
        }
        else{
            float volume = musicSlider.value;
            myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("music", volume);
        }
    }

    public void SetSfxVolume()
    {
        if (sfxSlider == null)
        {
            Debug.Log("No music slider");
        }
        else
        {
            float volume = sfxSlider.value;
            myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("sfx", volume);
        }
    }
}
