using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Data;
using System.Linq;

public class UI_AchievementComponent : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject achievementHolder; 

    [Header("References for UI")]
    [Header("Text References")]
    [SerializeField] private TMP_Text achievementNameText;
    [SerializeField] private TMP_Text shortAchievementDescriptionText;
    [SerializeField] private TMP_Text fullAchievementDescriptionText;
    [SerializeField] private TMP_Text collectedStarsText; 
    [SerializeField] private TMP_Text unlockValueStarsText; 
    [SerializeField] private TMP_Text bonusText;
    [SerializeField] private TMP_Text rewardText; 
    [SerializeField] private TMP_Text dateAchievedText;
    [SerializeField] private GameObject expandedText;
    
    [Header("Task List")]
    [SerializeField] private Transform taskListParent;
    [SerializeField] private GameObject taskPrefab; 
    
    [Header("Icons")]
    [SerializeField] private Image unlockedIcon; 
    [SerializeField] private Sprite lockedIcon;
    
    [Header("Outline Images")]
    [SerializeField] private Image collapsedOutline; 
    [SerializeField] private Sprite expandedOutline;
    
    [Header("Button Image for Expanding/Collapsing")]
    [SerializeField] private Image expandCollapseButtonImage;
    [SerializeField] private Sprite collapsedButtonSprite;
    [SerializeField] private Sprite expandedButtonSprite;

    
    [SerializeField] private int maxTitleLength = 20;
    [SerializeField] private int maxDescriptionLength = 50;
    private AchievementJSON achievement;
    

    private bool isExpanded = false;
    
    private List<GameObject> instantiatedTasks;
    
    private void Awake()
    {
        SetCollapsedState();
        instantiatedTasks = new List<GameObject>();
    }

    public void SetupAchievement(AchievementJSON achievement)
    {
        this.achievement = achievement;
        // Set achievements
        achievementNameText.text = achievement.name;
        shortAchievementDescriptionText.text = achievement.description;
        fullAchievementDescriptionText.text = achievement.description;
        collectedStarsText.text = achievement.requirements.Where(x => x.completed).Count().ToString();
        unlockValueStarsText.text = achievement.requirements.Count().ToString();

        //Set achievement icon
        if (UI_AchievementManager.Instance.GetReferancedImage(achievement.sprite) !=  null)
        {
            unlockedIcon.sprite = achievement.unlocked ? UI_AchievementManager.Instance.GetReferancedImage(achievement.sprite) : lockedIcon;
        }
        else
        {
            throw new System.Exception("Something went wrong when loading the image! for the achievement: " + achievement.name);
        }
        
        if (achievement.unlocked)
        {
            dateAchievedText.text = achievement.date;
        }
        else
        {
            dateAchievedText.text = "Not Achieved";
        }

        // Populate tasks list dynamically
        PopulateTaskList(achievement.requirements);

        if(achievement.rewards.Count > 0) {
            // Set reward text
            List<Reward> rewards = achievement.rewards;
            List<string> rewardDescriptions = rewards.Select(x => x.description).ToList();
            string finalRewardText = string.Join("\n", rewardDescriptions);
            rewardText.text = finalRewardText;
        }

        SetCollapsedState();
    }

    private void PopulateTaskList(List<AchievementRequirement> requirements)
    {
        //Debug.Log("Clearing tasks...");
        ClearTasks();
        
        foreach (AchievementRequirement requirement in requirements)
        {
            GameObject taskItem = Instantiate(taskPrefab, taskListParent);
            UI_TaskComponent taskComponent = taskItem.GetComponent<UI_TaskComponent>();
            instantiatedTasks.Add(taskItem);
            taskComponent.SetupRequirement(requirement);
        }
    }
    
    public void ToggleExpandCollapse()
    {
        isExpanded = !isExpanded;

        if (isExpanded)
        {
            SetExpandedState();
        }
        else
        {
            SetCollapsedState();
        }
        UpdateButtonSprite();
        
        // I have to use it because otherwise size of holder not getting resized when collapsing unless scroll ;/
        //LayoutRebuilder.ForceRebuildLayoutImmediate(achievementHolder.GetComponent<RectTransform>());
    }
    
    private void SetCollapsedState()
    {
        if (achievement != null) {
        
            achievementNameText.text = TruncateText(achievement.name, maxTitleLength); 
            shortAchievementDescriptionText.gameObject.SetActive(true); 
            taskListParent.gameObject.SetActive(false); 
            expandedText.gameObject.SetActive(false);
        }
    }
    
    private void SetExpandedState()
    {
        achievementNameText.text = achievement.name;
        shortAchievementDescriptionText.gameObject.SetActive(false);
        taskListParent.gameObject.SetActive(true); 
        expandedText.gameObject.SetActive(true);
    }
    
    private void UpdateButtonSprite()
    {
        if (expandCollapseButtonImage != null)
        {
            expandCollapseButtonImage.sprite = isExpanded ? expandedButtonSprite : collapsedButtonSprite;
        }
    }
    
    //TODO: Check with Text Mesh
    private string TruncateText(string text, int maxLength)
    {
        if (!isExpanded && text.Length > maxLength)
        {
            return text.Substring(0, maxLength) + "...";
        }
        return text;
    }
    
    private void ClearTasks()
    {
        foreach (GameObject taskItem in instantiatedTasks)
        {
            Destroy(taskItem);
        }
        instantiatedTasks.Clear();
    }
    
}
