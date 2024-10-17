using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHelper : MonoBehaviour
{
    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void PlaySfxSound(string sound)
    {
        if (soundManager != null)
        {
            soundManager.PlaySFX(sound);
        }
    }
}
