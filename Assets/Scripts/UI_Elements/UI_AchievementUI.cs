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
    
    //[Header("Buttons")]
    //[SerializeField] private Button expandButton; 
   // [SerializeField] private Button collapseButton; 
    
    private bool isExpanded = false;
    
    private void Awake()
    {
        collapsedHolder = transform.Find("Collapsed").gameObject;
        expandedHolder = transform.Find("Expanded").gameObject;

        collapsedHolder.SetActive(true);
        expandedHolder.SetActive(false); 
        
    }
    
    //SetupRepresentation
    public void PopulateAchievements(Achievement achievement)
    {
        //UpdateAchievementIcon(achievement);
        
            expandedAchievementNameText.text = achievement.achievementName;
            expandedAchievementDescriptionText.text = achievement.description;
            expandedCollectedText.text = achievement.collectedValue.ToString();
            expandedUnlockValueText.text = achievement.unlockValue.ToString();
            expandedRewardText.text = string.IsNullOrEmpty(achievement.reward) ? "No reward" : achievement.reward;
            
            collapsedAchievementNameText.text = TruncateText(achievement.achievementName, maxTitleLength);
            collapsedAchievementDescriptionText.text = TruncateText(achievement.description, maxDescriptionLength);
            collapsedCollectedText.text = achievement.isAchieved ? achievement.collectedValue.ToString() : "0";
            collapsedUnlockValueText.text = achievement.unlockValue.ToString();
        
    }

    public void SetupRepresentation()
    {
        // Fill header
        // Fill short description
        // Star List
        // Date Achieved - > only visible when achieved
        
        // Fill  full description
        // Fill list of thing to  do
          // List -> Populating  the space with items [array with strings]
          // instantiate at the transform of the parent (like in content box)
          // Flexible size
        //  Fill bonus
        
    }

    public void ToggleExpandCollapse()
    {
        // !is Expanded  = isExpanded
        
        // if  is Expanded false
          // Disable Full Description Parent
          // Enable Short Description
          
          
        // if  is Expanded true
          // Enable Full Description
          // Disable short description
        
        
        // Refresh
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
    
    //TODO: Check with Text Mesh
    private string TruncateText(string text, int maxLength)
    {
        if (!isExpanded && text.Length > maxLength)
        {
            return text.Substring(0, maxLength) + "...";
        }
        return text;
    }
    
    public void Expand()
    {
        
        if (!isExpanded)
        {
            collapsedHolder.SetActive(false);
            expandedHolder.SetActive(true);
            isExpanded = true;
            
            Debug.Log("Expanded successfully");
        }
    }

    public void Collapse()
    {
        if (isExpanded)
        {
            collapsedHolder.SetActive(true);
            expandedHolder.SetActive(false);
            isExpanded = false;
        }
    }
    
}
