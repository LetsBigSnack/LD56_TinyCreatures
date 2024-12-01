using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.SocialPlatforms.Impl;


public enum dropdown_1
{
    All,
    Unlocked,
    Locked
}

public class UI_AchievementManager : MonoBehaviour
{
    public static UI_AchievementManager Instance { get; private set; }

    [SerializeField] List<Achievement> achievements;
    [SerializeField] TMP_Dropdown dropdown1;
    [SerializeField] TMP_Dropdown dropdown2;
    
    [SerializeField] private GameObject achievementUIPrefab;
    [SerializeField] private Transform achievementListContainer;

    dropdown_1 dropdown1Selected = dropdown_1.All;
    achievementType dropdown2Selected = 0;
    List<Achievement> filtertAchievements;


    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {

            Instance = this;
            fillDropdowns();
            filtertAchievements = achievements;
            SortAchievements(dropdown1Selected, dropdown2Selected -1);
        }
    }

    public void fillDropdowns()
    {
        dropdown1.options.Clear();
        dropdown2.options.Clear();
        
        Array values_1 = Enum.GetValues(typeof(dropdown_1));
        foreach(dropdown_1 val in  values_1) 
        {
            dropdown1.options.Add(new TMP_Dropdown.OptionData(Enum.GetName(typeof(dropdown_1), val), null));
        }

        //Takes enum from Achievement scriptable obj
        Array values_2 = Enum.GetValues(typeof(achievementType));
        foreach (achievementType val in values_2)
        {
            dropdown2.options.Add(new TMP_Dropdown.OptionData(Enum.GetName(typeof(achievementType), val), null));
        }
    }

    public void dropdownChange()
    {
        dropdown1Selected = (dropdown_1)dropdown1.value;
        dropdown2Selected = (achievementType)dropdown2.value - 1;
        Debug.Log("Value for left dd = "+ dropdown1Selected + " --- Value for right dd = " +  dropdown2Selected);
        SortAchievements(dropdown1Selected, dropdown2Selected);
    }

    public void ShowAchievements(List<Achievement> sortedAchievements)
    {
        // Debug.Log("---- ---- ---- ----");
        // foreach (Achievement achievement in sortedAchievements)
        // {
        //     Debug.Log(achievement.name);
        // }
        // Debug.Log("---- ---- ---- ----");
        
        foreach (Achievement achievement in sortedAchievements)
        {
            GameObject achievementUI = Instantiate(achievementUIPrefab, achievementListContainer);
            UI_Achievement_UI achievementUIComponent = achievementUI.GetComponent<UI_Achievement_UI>();
            achievementUIComponent.PopulateAchievements(achievement);
        }
            
    }
    
    

    public void SortAchievements(dropdown_1 sortBy, achievementType sortBy2)
    {
        if(sortBy2 < 0)
        {
            switch (sortBy)
            {
                case dropdown_1.All:
                    filtertAchievements = achievements.Where(achievement => achievement.isAchieved || !achievement.isAchieved).OrderBy(achievement => achievement.name).ToList();
                    break;
                case dropdown_1.Unlocked:
                    filtertAchievements = achievements.Where(achievement => achievement.isAchieved).OrderBy(achievement => achievement.name).ToList();
                    break;
                case dropdown_1.Locked:
                    filtertAchievements = achievements.Where(achievement => !achievement.isAchieved).OrderBy(achievement => achievement.name).ToList();
                    break;
            }
            ShowAchievements(filtertAchievements);
            return;
        }
        switch (sortBy)
        {
            case dropdown_1.All:
                filtertAchievements = achievements.Where(achievement => achievement.isAchieved || !achievement.isAchieved).Where(achievement => achievement.type == sortBy2).OrderBy(achievement => achievement.name).ToList();
                break;
            case dropdown_1.Unlocked:
                filtertAchievements = achievements.Where(achievement => achievement.isAchieved).Where(achievement => achievement.type == sortBy2).OrderBy(achievement => achievement.name).ToList();
                break;
            case dropdown_1.Locked:
                filtertAchievements = achievements.Where(achievement => !achievement.isAchieved).Where(achievement => achievement.type == sortBy2).OrderBy(achievement => achievement.name).ToList();
                break;
        }
        ShowAchievements(filtertAchievements);
    }

    public void SortDd2Achievements(achievementType sortBy, List<Achievement> sortByDd1)
    {
        if(sortBy < 0)
        {
            filtertAchievements = sortByDd1.Where(achievement => achievement.isAchieved || !achievement.isAchieved).OrderBy(achievement => achievement.name).ToList();
            ShowAchievements(filtertAchievements);
            return;
        }

        filtertAchievements = sortByDd1.Where(achievement => achievement.type == sortBy).ToList();
        ShowAchievements(filtertAchievements);
    }

}
