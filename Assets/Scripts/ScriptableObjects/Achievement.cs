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
    // visible attributes
    public string achievementName;
    public string description;
    public Sprite lockedSprite;
    public Sprite unlockedSprite;
    public BigDecimal unlockValue;
    public DateTime dateAchieved;
    public string reward;

    // not visible attributes
    public bool isAchieved;
    public achievementType type;
}