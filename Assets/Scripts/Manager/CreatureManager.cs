using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    
    public static CreatureManager Instance { get; private set; }

    [Header("Sprite Settings")]
    [SerializeField] private Sprite[] creatureHeadSprites;
    [SerializeField] private Sprite[] creatureBodySprites;
    [SerializeField] private Sprite[] creatureLegsSprites;
    [SerializeField] private Sprite[] creatureArmsSprites;
    [SerializeField] private Color[] creatureColors;
    
    [Header("Stat Settings")]
    [Range(0, 100.0f)]
    [SerializeField] private float statRange = 3.5f;
    [Range(0, 100.0f)]
    [SerializeField] private float statMin = 10f;
    
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
    
    //TODO: clean up variable names
    public Creature CreateBasicCreature()
    {
        
        int randomHealth = Mathf.RoundToInt((UnityEngine.Random.Range(-statRange, statRange) + statMin) + statMin * 1.2f);
        CreatureStats creatureStats = CreateCreatureStats(statRange, statMin);
        CreatureRepresentation creatureRepresentation = GetRandomCreatureRepresentation();
        
        Creature createdCreature = new Creature(0,randomHealth, creatureStats, creatureRepresentation);
        
        return createdCreature;
    }

    private CreatureRepresentation GetRandomCreatureRepresentation()
    {
        Color headColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];
        Color bodyColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];
        Color armsColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];
        Color legsColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];
        
        Sprite randomHead = creatureHeadSprites[UnityEngine.Random.Range(0, creatureHeadSprites.Length)];
        Sprite randomBody = creatureBodySprites[UnityEngine.Random.Range(0, creatureBodySprites.Length)];
        Sprite randomLegs = creatureLegsSprites[UnityEngine.Random.Range(0, creatureLegsSprites.Length)];
        Sprite randomArms = creatureArmsSprites[UnityEngine.Random.Range(0, creatureArmsSprites.Length)];

        CreatureRepresentation creatureRepresentation = new CreatureRepresentation(randomHead, randomBody, randomLegs,
            randomArms, headColor, bodyColor, legsColor, armsColor);

        return creatureRepresentation;
    }

    public CreatureStats CreateCreatureStats(float definedStatRange, float definedStatMin)
    {
        float randomSpeed = UnityEngine.Random.Range(-definedStatRange, definedStatRange) + definedStatMin/2;
        float randomAttack = UnityEngine.Random.Range(-definedStatRange, definedStatRange) + definedStatMin/2;
        float randomDefense = UnityEngine.Random.Range(-definedStatRange, definedStatRange) + definedStatMin/2;
        float randomDexterity = UnityEngine.Random.Range(-definedStatRange, definedStatRange) + definedStatMin/2;

        CreatureStats creatureStats = new CreatureStats(randomSpeed, randomAttack, randomDefense, randomDexterity);
        return creatureStats;
    }

    public Creature CreateAdjustedCreature(float definedStatRange, float definedStatMin)
    {
        int randomHealth = Mathf.RoundToInt((UnityEngine.Random.Range(-definedStatRange, definedStatRange) + definedStatMin) + definedStatMin * 1.2f);
        CreatureStats creatureStats = CreateCreatureStats(definedStatRange, definedStatMin);
        CreatureRepresentation creatureRepresentation = GetRandomCreatureRepresentation();
        Creature createdCreature = new Creature(0,randomHealth, creatureStats, creatureRepresentation);
        
        return createdCreature;
    }

    public Sprite GetRandomBodyPart(string name)
    {
        switch (name)
        {
            case "Head":
                return creatureHeadSprites[UnityEngine.Random.Range(0, creatureHeadSprites.Length)];
                break;
            case "Body":
                return creatureBodySprites[UnityEngine.Random.Range(0, creatureHeadSprites.Length)];
                break;
            case "Legs":
                return creatureLegsSprites[UnityEngine.Random.Range(0, creatureHeadSprites.Length)];
                break;
            case "Arms":
                return creatureArmsSprites[UnityEngine.Random.Range(0, creatureHeadSprites.Length)];
                break;
        }

        return null;
    }
    
    public Color GetRandomColor()
    {
        Color headColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];
        return headColor;
    }
}