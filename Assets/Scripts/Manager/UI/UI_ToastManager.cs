using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using System.Linq;
using Manager;

public class UI_ToastManager : MonoBehaviour
{
    public static UI_ToastManager Instance;
    
    [SerializeField] GameObject achievementToastPrefab;
    [SerializeField] GameObject notificationToastPrefab;
    [SerializeField] private List<Toast> achievementToasts = new List<Toast>();
    [SerializeField] private List<Toast> notificationToasts = new List<Toast>();

    private GameObject currAchievementToast;
    private GameObject currNotificationToast;

    public GameObject CurrAchievementToast {
        get { return currAchievementToast; }
        set { currAchievementToast = value; }
    }

    public GameObject CurrNotificationToast
    {
        get { return currNotificationToast; }
        set { currNotificationToast = value; }
    }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void PushNextToast(NotificationType type)
    {
        StartCoroutine(ForceWait(type));
    }

    public void PushNewToast(NotificationType type)
    {
        Toast toast;

        switch (type)
        {
            case NotificationType.Achievement:
                if (!NextAchievementAvailable()) return;
                toast = achievementToasts[0];
                achievementToasts.RemoveAt(0);
                CreateToast(NotificationType.Achievement, toast.title, toast.description, toast.achievementJSON);
                break;
            case NotificationType.Notification:
                if (!NextNotificationAvailable()) return;
                toast = notificationToasts[0];
                notificationToasts.RemoveAt(0);
                CreateToast(NotificationType.Notification, toast.title, toast.description);
                break;
            case NotificationType.Alert:
                if (!NextNotificationAvailable()) return;
                toast = notificationToasts[0];
                notificationToasts.RemoveAt(0);
                CreateToast(NotificationType.Alert, toast.title, toast.description);
                break;
        }        
    }

    public bool NextAchievementAvailable()
    {
        return achievementToasts.Count > 0;
    }

    public bool NextNotificationAvailable()
    {
        return notificationToasts.Count > 0;
    }

    public bool CurrentAchievementEmpty()
    {
        return currAchievementToast == null;
    }

    public bool CurrentNotificationEmpty()
    {
        return currNotificationToast == null;
    }
    
    public void CreateToast(NotificationType type, string title = "", string description = "",AchievementJSON achievement = null)
    {
        switch (type)
        {
            case NotificationType.Achievement:
                if (CurrentAchievementEmpty())
                {
                    CreateAchievmentToast(achievement);
                    return;
                }
                break;
            case NotificationType.Notification:
                if (CurrentNotificationEmpty())
                {
                    CreateNotificationToast(title, description);
                    return;
                }
                break;
            case NotificationType.Alert:
                if (CurrentNotificationEmpty())
                {
                    CreateErrorToast(title, description);
                    return;
                }
                break;
        }
        StashToast(type,achievement,title,description);
    }
    
    private void StashToast(NotificationType type, AchievementJSON achievement = null, string title = "", string description = "")
    {
        Toast toast = new Toast(type, achievement, title, description);

        if (toast.achievementJSON != null)
        {
            achievementToasts.Add(toast);
            return;
        }
        notificationToasts.Add(toast);
    }

    private void CreateAchievmentToast(AchievementJSON achievement)
    {
        GameObject newToast = Instantiate(achievementToastPrefab, gameObject.transform);
        currAchievementToast = newToast;

        UI_ToastItem toastItem = newToast.GetComponent<UI_ToastItem>();

        toastItem.Image.sprite = AchievementManager.Instance.GetReferancedImage(achievement.sprite);
        toastItem.TitelText.text = achievement.name;
        toastItem.Type = NotificationType.Achievement;

        string rewardText;

        if (achievement.rewards.Count > 0)
        {
            List<Reward> rewards = achievement.rewards;
            List<string> rewardDescriptions = rewards.Select(x => x.description).ToList();
            string finalRewardText = string.Join("\n", rewardDescriptions);
            rewardText = finalRewardText;
            toastItem.RewardText.text = rewardText;
        }
    }

    private void CreateNotificationToast(string title, string description)
    {
        GameObject newToast = Instantiate(notificationToastPrefab, gameObject.transform);
        currNotificationToast = newToast;

        UI_ToastItem toastItem = newToast.GetComponent<UI_ToastItem>();

        toastItem.TitelText.text = title;
        toastItem.RewardText.text = description;
        toastItem.Type = NotificationType.Notification;
    }

    private void CreateErrorToast(string title, string description)
    {
        GameObject newToast = Instantiate(notificationToastPrefab, gameObject.transform);
        currNotificationToast = newToast;

        UI_ToastItem toastItem = newToast.GetComponent<UI_ToastItem>();

        toastItem.TitelText.text = title;
        toastItem.RewardText.text = description;
        toastItem.Type = NotificationType.Alert;
    }

    private IEnumerator ForceWait(NotificationType type)
    {
        yield return new WaitForSeconds(0.5f);
        PushNewToast(type);
    }

}
