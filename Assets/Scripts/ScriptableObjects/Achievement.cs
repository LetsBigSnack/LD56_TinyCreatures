using System;
using Data;
using UnityEngine;


public enum achievementType
{
    Wins,
    Fusions,
    Speed
}

[CreateAssetMenu(fileName = "NewAchievement", menuName = "Game/Achievement")]
public class Achievement : ScriptableObject
{
    //TODO add ID
    // visible attributes
    public string achievementName;
    public string description;
    public Sprite unlockedSprite;
    public BigDecimal unlockValue;
    public DateTime dateAchieved;
    public string reward;

    // not visible attributes
    public bool isAchieved;
    public achievementType type;
}