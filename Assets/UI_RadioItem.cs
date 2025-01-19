using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Data;

public class UI_RadioItem : MonoBehaviour
{
    public static UI_RadioItem Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI titleViewedTxt;
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private TextMeshProUGUI timeTxt;

    [SerializeField] private Sprite playSprite;
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Image playBtnSprite;

    private string currentTrackMaxTime;

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
        if(RadioManager.Instance != null)
        {
            RadioManager.Instance.OnChangePlayedTrackValue += PlayedTrackChanged;
            RadioManager.Instance.OnChangeViewedTrackValue += ViewedTrackChanged;
            RadioManager.Instance.OnChangeCurrentPlayedTimeValue += ViewedTrackTimeChanged;
        }

        RadioManager.Instance.SetupInitialState();
    }

    private void OnDisable()
    {
        RadioManager.Instance.OnChangePlayedTrackValue -= PlayedTrackChanged;
        RadioManager.Instance.OnChangeViewedTrackValue -= ViewedTrackChanged;
        RadioManager.Instance.OnChangeCurrentPlayedTimeValue -= ViewedTrackTimeChanged;
    }

    private void ViewedTrackChanged(Track viewedTrack)
    {
        titleViewedTxt.text = "Currently viewed: " + viewedTrack.name;
        PlayButtonChange();
    }

    private void PlayedTrackChanged(Track playedTrack)
    {
        titleTxt.text = "Currently playing: " + playedTrack.name;
        currentTrackMaxTime = TranslateToMinutes(playedTrack.source.clip.length);
        PlayButtonChange();
    }

    private void ViewedTrackTimeChanged(float time)
    {
        timeTxt.text = TranslateToMinutes(time) + " - " + currentTrackMaxTime;
    }

    public void PlayButtonChange()
    {

        if (!RadioManager.Instance.IsTrackEqual() || !RadioManager.Instance.IsViewedTrackPlaying())
        {
            playBtnSprite.sprite = playSprite;
            return;
        }
        playBtnSprite.sprite = pauseSprite;
    }

    public string TranslateToMinutes(float time)
    {
        int timeInSecondsInt = (int)time; 
        int minutes = timeInSecondsInt / 60;
        int seconds = timeInSecondsInt % 60;
        return minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }
}
