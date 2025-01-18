using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RadioItem : MonoBehaviour
{
    public static UI_RadioItem Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI titleViewedTxt;
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private TextMeshProUGUI timeTxt;

    [SerializeField] private Sprite playSprite;
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Image playBtnSprite;

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
        if(UI_RadioManager.Instance != null)
        {
            UI_RadioManager.Instance.OnChangePlayedTrackValue += PlayedTrackChanged;
            UI_RadioManager.Instance.OnChangeViewedTrackValue += ViewedTrackChanged;
        }
    }

    private void OnDisable()
    {
        UI_RadioManager.Instance.OnChangePlayedTrackValue -= PlayedTrackChanged;
        UI_RadioManager.Instance.OnChangeViewedTrackValue -= ViewedTrackChanged;
    }

    private void ViewedTrackChanged(Track viewedTrack)
    {
        titleViewedTxt.text = viewedTrack.name;
        PlayButtonChange();
    }

    private void PlayedTrackChanged(Track playedTrack)
    {
        titleTxt.text = playedTrack.name;
        timeTxt.text = playedTrack.source.time + " - " + playedTrack.source.clip.length.ToString();
        PlayButtonChange();
    }

    public void PlayButtonChange()
    {
        Debug.Log("IsTrackEqual: " + UI_RadioManager.Instance.IsTrackEqual());
        Debug.Log("IsTrackPlaying: " + UI_RadioManager.Instance.IsTrackPlaying());

        if (!UI_RadioManager.Instance.IsTrackEqual() || !UI_RadioManager.Instance.IsTrackPlaying())
        {
            playBtnSprite.sprite = playSprite;
            return;
        }
        playBtnSprite.sprite = pauseSprite;
    }

}
