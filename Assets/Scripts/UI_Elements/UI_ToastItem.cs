using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum NotificationType
{
    Achievement,
    Notification,
    Alert
}

public class UI_ToastItem : MonoBehaviour
{
    [SerializeField] private NotificationType type;

    [SerializeField] private Image image;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI titelText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private Color notificationColor;
    [SerializeField] private Color alertColor;

    public NotificationType Type
    {
        get { return type; }
        set { type = value; }
    }

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
        SetupRepresentation();
    }

    private void SetupRepresentation()
    {
        switch (type)
        {
            case NotificationType.Achievement:
                UI_ToastManager.Instance.CurrAchievementToast = gameObject;
                backgroundImage.color = notificationColor;
                break;
            case NotificationType.Notification:
                UI_ToastManager.Instance.CurrNotificationToast = gameObject;
                backgroundImage.color = notificationColor;
                break;
            case NotificationType.Alert:
                UI_ToastManager.Instance.CurrNotificationToast = gameObject;
                backgroundImage.color = alertColor;
                break;

        }
    }

    public void EndToast()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (type == NotificationType.Achievement)
        {
            UI_ToastManager.Instance.CurrAchievementToast = null;
        }
        else
        {
            UI_ToastManager.Instance.CurrNotificationToast = null;
        }
        UI_ToastManager.Instance.PushNextToast(type);
    }
}
