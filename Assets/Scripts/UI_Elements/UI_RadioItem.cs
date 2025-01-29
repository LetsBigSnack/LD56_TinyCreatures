using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Data;

public class UI_RadioItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static UI_RadioItem Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI titleViewedTxt;
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private TextMeshProUGUI timeTxt;

    [SerializeField] private Sprite playSprite;
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Image playBtnSprite;

    [SerializeField] private RectTransform titleBarRectTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;

    private bool isDragging = false;

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

            canvas = GetComponentInParent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(titleBarRectTransform, eventData.position, eventData.pressEventCamera))
        {
            isDragging = true;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            (transform as RectTransform).anchoredPosition += eventData.delta / canvas.scaleFactor;

            ClampToScreen();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            isDragging = false;
            canvasGroup.blocksRaycasts = true;
        }
    }

    private void ClampToScreen()
    {
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;

        Vector2 radioToolSize = (transform as RectTransform).sizeDelta;

        Vector2 clampedPosition = (transform as RectTransform).anchoredPosition;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -canvasSize.x / 2 + radioToolSize.x / 2, canvasSize.x / 2 - radioToolSize.x / 2);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -canvasSize.y / 2 + radioToolSize.y / 2, canvasSize.y / 2 - radioToolSize.y / 2);

        (transform as RectTransform).anchoredPosition = clampedPosition;
    }
}
