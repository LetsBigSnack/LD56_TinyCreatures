using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UI_AchievementManager : MonoBehaviour
{
    public static UI_AchievementManager Instance { get; private set; }

    [SerializeField] List<Achievement> achievements;
    [SerializeField] TMP_Dropdown dropdown;

    string selecetedOption = "All";
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
        }
    }

    public void SetSelected()
    {
        selecetedOption = dropdown.options[dropdown.value].text;
        SortAchievements(selecetedOption);
        Debug.Log(selecetedOption);
    }

    public void ShowAchievements(List<Achievement> sortedAchievements)
    {
        Debug.Log("This is how many in list = " + sortedAchievements.Count);
        foreach (Achievement achievement in sortedAchievements)
        {
            Debug.Log(achievement.name);
        }
    }

    public void SortAchievements(string sortBy)
    {
        switch (sortBy)
        {
            case "All":
                filtertAchievements = achievements.Where(achievement => achievement.isAchieved || !achievement.isAchieved).OrderBy(achievement => achievement.name).ToList();
                break;
            case "Locked":
                filtertAchievements = achievements.Where(achievement => !achievement.isAchieved).OrderBy(achievement => achievement.name).ToList();
                break;
            case "Unlocked":
                filtertAchievements = achievements.Where(achievement => achievement.isAchieved).OrderBy(achievement => achievement.name).ToList();
                break;
        }
        Debug.Log("After the switch");
        ShowAchievements(filtertAchievements);
    }

}
