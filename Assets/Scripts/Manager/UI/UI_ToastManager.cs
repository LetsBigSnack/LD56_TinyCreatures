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

    public void PushNextToast(bool isAchievement)
    {
        StartCoroutine(ForceWait(isAchievement));
    }

    public void PushNewToast(bool isAchievement)
    {
        Toast toast;

        if (isAchievement && achievementToasts.Count > 0)
        {
            toast = achievementToasts[0];
            achievementToasts.RemoveAt(0);
            CreateToast(toast.title, toast.description,toast.achievementJSON);
        } 
        else if(!isAchievement && notificationToasts.Count > 0)
        {
            toast = notificationToasts[0];
            notificationToasts.RemoveAt(0);
            CreateToast(toast.title, toast.description);
        }
        
    }
    
    public void CreateToast(string title = "", string description = "",AchievementJSON achievement = null)
    {
        if (currAchievementToast == null && achievement != null)
        {
            CreateAchievmentToast(achievement);
            return;
        }

        if(currNotificationToast == null && achievement == null)
        {
            CreateNotificationToast(title, description);
            return;
        }

        StashToast(achievement,title,description);
    }
    
    private void StashToast(AchievementJSON achievement = null, string title = "", string description = "")
    {
        Toast toast = new Toast(achievement, title, description);

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
    }

    private IEnumerator ForceWait(bool isAchievement)
    {
        yield return new WaitForSeconds(0.5f);
        PushNewToast(isAchievement);
    }

}
