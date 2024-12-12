using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.SocialPlatforms.Impl;
using System.Globalization;


public enum OptionFilterAchieved
{
    All,
    Unlocked,
    Locked
}

public class UI_AchievementManager : MonoBehaviour
{
    public static UI_AchievementManager Instance { get; private set; }

    [SerializeField] List<Achievement> achievements;

    [SerializeField] TMP_Dropdown dropdownAchieved;
    [SerializeField] TMP_Dropdown dropdownAchievementType;
    
    [SerializeField] private GameObject achievementUIPrefab;
    [SerializeField] private Transform achievementListContainer;

    [SerializeField] private TMP_InputField searchBar;

    private OptionFilterAchieved selectedAchieved = OptionFilterAchieved.All;
    private AchievementType selectedAchievementType = 0;

    private List<GameObject> instantiatedAchievements;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            instantiatedAchievements = new List<GameObject>();
            selectedAchievementType -= 1;
            FillDropdowns();
            SortAchievements();
        }
    }

    public void FillDropdowns()
    {
        Array values_1 = Enum.GetValues(typeof(OptionFilterAchieved));
        foreach(OptionFilterAchieved val in  values_1) 
        {
            dropdownAchieved.options.Add(new TMP_Dropdown.OptionData(Enum.GetName(typeof(OptionFilterAchieved), val), null));
        }

        //Takes enum from Achievement scriptable obj
        Array values_2 = Enum.GetValues(typeof(AchievementType));
        foreach (AchievementType val in values_2)
        {
            dropdownAchievementType.options.Add(new TMP_Dropdown.OptionData(Enum.GetName(typeof(AchievementType), val), null));
        }
    }

    public void OnChange()
    {
        selectedAchieved = (OptionFilterAchieved)dropdownAchieved.value;
        selectedAchievementType = (AchievementType)dropdownAchievementType.value - 1;

        #if UNITY_EDITOR
        Debug.Log("Value for left dd = "+ selectedAchieved + " --- Value for right dd = " +  selectedAchievementType);
        #endif

        // Option to fix double spaces in searchbar
        //if(searchBar.text.Length >= 2)
        //{
        //    if (searchBar.text[searchBar.text.Length - 1] == ' ' && searchBar.text[searchBar.text.Length - 2] == ' ')
        //    {
        //        searchBar.text = searchBar.text.Substring(0, searchBar.text.Length - 2);
        //        return;
        //    }
        //}

        SortAchievements();
    }

    public void ShowAchievements(List<Achievement> sortedAchievements)
    {
        ClearAchievements();

        #if UNITY_EDITOR
        //Debug.Log("---- ---- ---- ----");
        //foreach (Achievement achievement in sortedAchievements)
        //{
        //    Debug.Log(achievement.name);
        //}
        //Debug.Log("---- ---- ---- ----");

        #endif
        foreach (Achievement achievement in sortedAchievements)
        {
            GameObject achievementUI = Instantiate(achievementUIPrefab, achievementListContainer);
            UI_AchievementComponent achievementUIComponent = achievementUI.GetComponent<UI_AchievementComponent>();
            instantiatedAchievements.Add(achievementUI);
            achievementUIComponent.SetupAchievement(achievement);
        }
            
    }
    
    public void SortAchievements()
    {
        List<Achievement> filtertAchievements = achievements;

        // Sieve filtering method
        // First sieve unlocked or locked

        switch (selectedAchieved)
        {
            case OptionFilterAchieved.Unlocked:
                filtertAchievements = filtertAchievements.Where(achievement => achievement.isAchieved).ToList();
                break;
            case OptionFilterAchieved.Locked:
                filtertAchievements = filtertAchievements.Where(achievement => !achievement.isAchieved).ToList();
                break;
        }

        // Second sieve achievement type

        switch (selectedAchievementType)
        {
            case AchievementType.Wins:
                filtertAchievements = filtertAchievements.Where(achievement => achievement.type == AchievementType.Wins).ToList();
                break;
            case AchievementType.Fusions:
                filtertAchievements = filtertAchievements.Where(achievement => achievement.type == AchievementType.Fusions).ToList();
                break;
            case AchievementType.Speed:
                filtertAchievements = filtertAchievements.Where(achievement => achievement.type == AchievementType.Speed).ToList();
                break;
            default:
                break;
        }

        // Third sieve for searchbar
        string searchText = searchBar.text;
        if(searchText.Length >= 0 && searchText != "  ")
        {
            List<Achievement> helperListNames = new List<Achievement>();
            List<Achievement> helperListDescriptions = new List<Achievement>();

            helperListNames = filtertAchievements.Where(achievement => achievement.achievementName.ToLower().Contains(searchText.ToLower())).ToList();
            helperListDescriptions = filtertAchievements.Where(achievement => achievement.description.ToLower().Contains(searchText.ToLower())).ToList();
            
            filtertAchievements = helperListNames.Concat(helperListDescriptions).Distinct().ToList();
        }


        filtertAchievements.OrderBy(achievement => achievement.name);

        ShowAchievements(filtertAchievements);
    }


    private void ClearAchievements()
    {
        foreach (GameObject achievementUI in instantiatedAchievements)
        {
            Destroy(achievementUI);
        }
        instantiatedAchievements.Clear();
    }
}
