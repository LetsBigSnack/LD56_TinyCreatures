using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using Manager;

public class UI_ToastManager : MonoBehaviour
{
    public static UI_ToastManager Instance;
    [SerializeField] GameObject toastPrefab;

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

    public void CreateToast(AchievementJSON achievement)
    {
        GameObject newToast = Instantiate(toastPrefab, gameObject.transform);

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
}
