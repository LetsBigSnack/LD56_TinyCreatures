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

    public void DropdownChange()
    {
        selectedAchieved = (OptionFilterAchieved)dropdownAchieved.value;
        selectedAchievementType = (AchievementType)dropdownAchievementType.value - 1;

        #if UNITY_EDITOR
        Debug.Log("Value for left dd = "+ selectedAchieved + " --- Value for right dd = " +  selectedAchievementType);
        #endif

        SortAchievements();
    }

    public void ShowAchievements(List<Achievement> sortedAchievements)
    {
        #if UNITY_EDITOR
        ClearAchievements();
        Debug.Log("---- ---- ---- ----");
        foreach (Achievement achievement in sortedAchievements)
        {
            Debug.Log(achievement.name);
        }
        Debug.Log("---- ---- ---- ----");

        #endif
        foreach (Achievement achievement in sortedAchievements)
        {
            GameObject achievementUI = Instantiate(achievementUIPrefab, achievementListContainer);
            UI_Achievement_UI achievementUIComponent = achievementUI.GetComponent<UI_Achievement_UI>();
            instantiatedAchievements.Add(achievementUI);
            achievementUIComponent.PopulateAchievements(achievement);
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
        // TODO: make searchbar filter
       
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
