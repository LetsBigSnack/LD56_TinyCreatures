using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ToastItem : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI titelText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private bool isAchievement;

    public Image Image
    {
        get { return image; }
        set { image = value; }
    }

    public TextMeshProUGUI TitelText
    {
        get { return titelText; }
        set { titelText = value; }
    }

    public TextMeshProUGUI RewardText
    {
        get { return rewardText; }
        set { rewardText = value; }
    }

    void Start()
    {
        if (isAchievement)
        {
            UI_ToastManager.Instance.CurrAchievementToast = gameObject;
        } 
        else
        {
            UI_ToastManager.Instance.CurrNotificationToast = gameObject;
        }  
    }

    public void EndToast()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (isAchievement)
        {
            UI_ToastManager.Instance.CurrAchievementToast = null;
        }
        else
        {
            UI_ToastManager.Instance.CurrNotificationToast = null;
        }
        UI_ToastManager.Instance.PushNextToast(isAchievement);
    }
}
