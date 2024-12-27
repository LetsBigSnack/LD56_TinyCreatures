using System;
using System.Collections.Generic;
using Data;
using UnityEngine;



[CreateAssetMenu(fileName = "NewAchievement", menuName = "Game/Achievement")]
public class Achievement : ScriptableObject
{
    // ID for the achievement
    public string achievementID;
    
    // visible attributes
    public string achievementName;
    public string description;
    public Sprite unlockedSprite;
    public int collectedValue;
    public int unlockValue;
    public DateTime dateAchieved;
    public string reward;

    // not visible attributes
    public bool isAchieved;
    public AchievementType type;
    
    public List<Task> tasks;
}