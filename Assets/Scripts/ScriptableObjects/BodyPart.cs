using System;
using Newtonsoft.Json;
using UnityEngine;

public enum BodyPartType
{
    Head,
    Body,
    Arms,
    Legs
}


[CreateAssetMenu(fileName = "NewBodyPart", menuName = "Game/BodyPart")]
[Serializable]
[JsonConverter(typeof(BodyPartHandler))]
public class BodyPart : ScriptableObject
{
    public Sprite bodyPartSprite;
    
    // Stat modifiers
    public float healthModifier;
    public float speedModifier;
    public float attackModifier;
    public float defenseModifier;
    public float dexterityModifier;

    // Creature Dex Entry
    public string bodyPartName;
    public bool unlocked = true;
}