using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_Achievement_UI : MonoBehaviour
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
    
    [Header("Task List")]
    [SerializeField] private Transform taskListParent;
    [SerializeField] private GameObject taskPrefab; 
    
    [Header("Icons")]
    [SerializeField] private Image unlockedIcon; 
    [SerializeField] private Sprite lockedIcon;
    
    [Header("Button Image for Expanding/Collapsing")]
    [SerializeField] private Image expandCollapseButtonImage;
    [SerializeField] private Sprite collapsedButtonSprite;
    [SerializeField] private Sprite expandedButtonSprite;

    
    [SerializeField] private int maxTitleLength = 20;
    [SerializeField] private int maxDescriptionLength = 50;
    private Achievement achievement;
    

    private bool isExpanded = false;
    
    private List<GameObject> instantiatedTasks;
    
    private void Awake()
    {
        SetCollapsedState();
        instantiatedTasks = new List<GameObject>();
    }

    public void SetupAchievement(Achievement achievement)
    {
        this.achievement = achievement;
        // Set achievements
        achievementNameText.text = achievement.achievementName;
        shortAchievementDescriptionText.text = achievement.description;
        fullAchievementDescriptionText.text = achievement.description;
        collectedStarsText.text = achievement.collectedValue.ToString();
        unlockValueStarsText.text = achievement.unlockValue.ToString();

        // Set achievement icon
        unlockedIcon.sprite = achievement.isAchieved ? achievement.unlockedSprite : lockedIcon;
        
        if (achievement.isAchieved)
        {
            dateAchievedText.text = achievement.dateAchieved.ToString("MMMM dd, yyyy");
        }
        else
        {
            dateAchievedText.text = "Not Achieved";
        }

        // Populate tasks list dynamically
        PopulateTaskList(achievement.tasks);

        // Set reward text
        rewardText.text = string.IsNullOrEmpty(achievement.reward) ? "No reward" : achievement.reward;

        SetCollapsedState();
    }
    
    private void PopulateTaskList(List<Task> tasks)
    {
        Debug.Log("Clearing tasks...");
        ClearTasks();
        
        foreach (Task task in tasks)
        {
            GameObject taskItem = Instantiate(taskPrefab, taskListParent);
            UI_TaskComponent taskComponent = taskItem.GetComponent<UI_TaskComponent>();
            instantiatedTasks.Add(taskItem);
            taskComponent.SetupTask(task);
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
        LayoutRebuilder.ForceRebuildLayoutImmediate(achievementHolder.GetComponent<RectTransform>());
    }
    
    private void SetCollapsedState()
    {
        if (achievement != null) {
        
            achievementNameText.text = TruncateText(achievement.achievementName, maxTitleLength); 
            shortAchievementDescriptionText.gameObject.SetActive(true); 
            fullAchievementDescriptionText.gameObject.SetActive(false); 
            taskListParent.gameObject.SetActive(false); 
            bonusText.gameObject.SetActive(false); 
            rewardText.gameObject.SetActive(false);
        }
    }
    
    private void SetExpandedState()
    {
        achievementNameText.text = achievement.achievementName;
        shortAchievementDescriptionText.gameObject.SetActive(false);
        fullAchievementDescriptionText.gameObject.SetActive(true);
        taskListParent.gameObject.SetActive(true); 
        bonusText.gameObject.SetActive(true);
        rewardText.gameObject.SetActive(true);
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
