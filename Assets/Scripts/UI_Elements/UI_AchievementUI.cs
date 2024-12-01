using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Achievement_UI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject collapsedHolder; 
    [SerializeField] private GameObject expandedHolder; 

    [Header("References for UI")]
    [Header("Text References for Collapsed State")]
    [SerializeField] private TMP_Text collapsedAchievementNameText;
    [SerializeField] private TMP_Text collapsedAchievementDescriptionText;
    [SerializeField] private TMP_Text collapsedCollectedText; 
    [SerializeField] private TMP_Text collapsedUnlockValueText; 
    
    [Header("Text References for Expanded State")]
    [SerializeField] private TMP_Text expandedAchievementNameText; 
    [SerializeField] private TMP_Text expandedAchievementDescriptionText;
    [SerializeField] private TMP_Text expandedCollectedText; 
    [SerializeField] private TMP_Text expandedUnlockValueText;
    [SerializeField] private TMP_Text expandedRewardText; 
    
    [Header("Icons")]
    [SerializeField] private Image collapsedIcon;
    [SerializeField] private Image expandedIcon; 

    [SerializeField] private Sprite lockedIcon;
    
    [SerializeField] private int maxTitleLength = 15;
    [SerializeField] private int maxDescriptionLength = 50;
    
    [Header("Buttons")]
    [SerializeField] private Button expandButton; 
    [SerializeField] private Button collapseButton; 
    
    private bool isExpanded = false;
    
    private void Awake()
    {
        collapsedHolder = transform.Find("Collapsed").gameObject;
        expandedHolder = transform.Find("Expanded").gameObject;

        collapsedHolder.SetActive(true);
        expandedHolder.SetActive(false); 
        
        // Assign button listeners
        if (expandButton != null)
        {
            expandButton.onClick.AddListener(Expand);
        }

        if (collapseButton != null)
        {
            collapseButton.onClick.AddListener(Collapse);
        }
        else
        {
            Debug.LogWarning("Collapse Button not assigned in the inspector.");
        }
        
        collapsedHolder.SetActive(true);
        expandedHolder.SetActive(false);
        
    }
    
    public void PopulateAchievements(Achievement achievement)
    {
        UpdateAchievementIcon(achievement);

        // Update collapsed and expanded UI components
        if (isExpanded)
        {
            expandedAchievementNameText.text = achievement.achievementName;
            expandedAchievementDescriptionText.text = achievement.description;
            expandedCollectedText.text = achievement.collectedValue.ToString();
            expandedUnlockValueText.text = achievement.unlockValue.ToString();
            expandedRewardText.text = string.IsNullOrEmpty(achievement.reward) ? "No reward" : achievement.reward;
        }
        else
        {
            collapsedAchievementNameText.text = TruncateText(achievement.achievementName, maxTitleLength);
            collapsedAchievementDescriptionText.text = TruncateText(achievement.description, maxDescriptionLength);
            collapsedCollectedText.text = achievement.isAchieved ? achievement.collectedValue.ToString() : "0";
            collapsedUnlockValueText.text = achievement.unlockValue.ToString();
        }
    }
    
    
    public void ToggleExpandCollapseAchievement()
    {
        if (isExpanded)
        {
            Collapse();
        }
        else
        {
            Expand();
        }
    }
    
    private void UpdateAchievementIcon(Achievement achievement)
    {
        // Use the unlocked icon if the achievement is achieved, otherwise use the locked icon
        Sprite iconToUse = achievement.isAchieved ? achievement.unlockedSprite : lockedIcon;

        // Update the icon for both collapsed and expanded views
        collapsedIcon.sprite = iconToUse;
        expandedIcon.sprite = iconToUse;
    }
    
    private string TruncateText(string text, int maxLength)
    {
        // Only truncate if it's collapsed and the text is longer than maxLength
        if (!isExpanded && text.Length > maxLength)
        {
            return text.Substring(0, maxLength) + "...";
        }
        return text;
    }
    
    private void Expand()
    {
        // Only switch to expanded if it's not already expanded
        if (!isExpanded)
        {
            collapsedHolder.SetActive(false);
            expandedHolder.SetActive(true);
            isExpanded = true;
            
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
        }
    }

    private void Collapse()
    {
        if (isExpanded)
        {
            collapsedHolder.SetActive(true);
            expandedHolder.SetActive(false);
            isExpanded = false;
            
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
        }
    }
    
}
