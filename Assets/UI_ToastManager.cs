using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using Manager;
using UnityEngine.SocialPlatforms.Impl;

public class Toast
{
    public AchievementJSON achievementJSON = null;
    public string title = "";
    public string description = "";

    public Toast(AchievementJSON achievement, string title, string description)
    {
        this.achievementJSON = achievement;
        this.title = title;
        this.description = description;
    }
}


public class UI_ToastManager : MonoBehaviour
{
    public static UI_ToastManager Instance;
    [SerializeField] GameObject achievementToastPrefab;
    [SerializeField] GameObject notificationToastPrefab;

    private GameObject currToast;
    [SerializeField] private List<Toast> toasts = new List<Toast>();

    public GameObject CurrToast {
        get { return currToast; }
        set { currToast = value; }
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

    public void PushNextToast()
    {
        if(toasts.Count > 0)
        {
            Toast toast = toasts[0];
            toasts.RemoveAt(0);
            CreateToast(toast.achievementJSON, toast.title, toast.description);
        }
    }

    public void CreateToast(string title = "", string description = "")
    {
        CreateToast(null, title, description);
    }
    public void CreateToast(AchievementJSON achievement = null, string title = "", string description = "")
    {
        if (currToast == null)
        {
            if (achievement != null)
            {
                CreateAchievmentToast(achievement);
                return;
            }
            CreateNotificationToast(title, description);
            return;
        }
        
        Toast toast = new Toast(achievement, title, description);
        toasts.Add(toast);
    }

    private void CreateAchievmentToast(AchievementJSON achievement)
    {
        GameObject newToast = Instantiate(achievementToastPrefab, gameObject.transform);
        currToast = newToast;

        UI_ToastItem toastItem = newToast.GetComponent<UI_ToastItem>();

        toastItem.Image.sprite = AchievementManager.Instance.GetReferancedImage(achievement.sprite);
        toastItem.TitelText.text = achievement.name;

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
        currToast = newToast;

        UI_ToastItem toastItem = newToast.GetComponent<UI_ToastItem>();

        toastItem.TitelText.text = title;

        toastItem.RewardText.text = description;
    }

}
