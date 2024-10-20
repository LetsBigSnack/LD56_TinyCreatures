using UnityEngine;

public enum BodyPartType
{
    Head,
    Body,
    Arms,
    Legs
}


[CreateAssetMenu(fileName = "NewBodyPart", menuName = "Game/BodyPart")]
public class BodyPart : ScriptableObject
{
    public Sprite bodyPartSprite;
    
    // Stat modifiers
    public float healthModifier;
    public float speedModifier;
    public float attackModifier;
    public float defenseModifier;
    public float dexterityModifier;
    
}