using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    
    public static CreatureManager Instance { get; private set; }

    [Header("Sprite Settings")]
    
    [SerializeField] private BodyPartSet[] bodyPartSets;

    [SerializeField] private Color[] creatureColors;
    
    [Header("Stat Settings")]
    [Range(0, 100.0f)]
    [SerializeField] private float statRange = 3.5f;
    [Range(0, 100.0f)]
    [SerializeField] private float statMin = 10f;
    
    private List<BodyPart> _unlockedHeads = new List<BodyPart>();
    private List<BodyPart> _unlockedBodies = new List<BodyPart>();
    private List<BodyPart> _unlockedArms = new List<BodyPart>();
    private List<BodyPart> _unlockedLegs = new List<BodyPart>();
    
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
        RefreshUnlockedParts();
        
       
        CreatureRepresentation creatureRepresentation = GetRandomCreatureRepresentation();
        
        float totalHealthModifier = creatureRepresentation.BodyParts.Select(c => c.Value).Sum(t => t.healthModifier);
        int randomHealth = Mathf.RoundToInt((UnityEngine.Random.Range(-statRange * (1 - totalHealthModifier), statRange * (1 + totalHealthModifier)) + statMin + statMin) * 1.2f);

        CreatureStats creatureStats = CreateCreatureStats(statRange, statMin, creatureRepresentation.BodyParts);
        
        Creature createdCreature = new Creature(0,randomHealth, creatureStats, creatureRepresentation);
        
        return createdCreature;
    }

    private CreatureRepresentation GetRandomCreatureRepresentation()
    {
        Color headColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];
        Color bodyColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];
        Color armsColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];
        Color legsColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];

        
        BodyPart randomHead = _unlockedHeads[UnityEngine.Random.Range(0, _unlockedHeads.Count)];
        BodyPart randomBody = _unlockedBodies[UnityEngine.Random.Range(0, _unlockedBodies.Count)];
        BodyPart randomArms = _unlockedArms[UnityEngine.Random.Range(0, _unlockedArms.Count)];
        BodyPart randomLegs = _unlockedLegs[UnityEngine.Random.Range(0, _unlockedLegs.Count)];
        
        Dictionary<BodyPartType, BodyPart> bodyParts = new Dictionary<BodyPartType, BodyPart>();
        bodyParts.Add(BodyPartType.Head, randomHead);
        bodyParts.Add(BodyPartType.Body, randomBody);
        bodyParts.Add(BodyPartType.Arms, randomArms);
        bodyParts.Add(BodyPartType.Legs, randomLegs);
        
        
        CreatureRepresentation creatureRepresentation = new CreatureRepresentation(bodyParts, headColor, bodyColor, legsColor, armsColor);

        return creatureRepresentation;
    }

    public CreatureStats CreateCreatureStats(float definedStatRange, float definedStatMin,
        Dictionary<BodyPartType, BodyPart> bodyParts)
    {
        
        float totalSpeedModifier = bodyParts.Select(c => c.Value).Sum(t => t.speedModifier);
        float totalAttackModifier = bodyParts.Select(c => c.Value).Sum(t => t.attackModifier);
        float totalDexterityModifier = bodyParts.Select(c => c.Value).Sum(t => t.dexterityModifier);
        float totalDefenseModifier = bodyParts.Select(c => c.Value).Sum(t => t.defenseModifier);

        
        float randomSpeed = UnityEngine.Random.Range(-definedStatRange * (1 - totalSpeedModifier), definedStatRange * (1 + totalSpeedModifier)) + definedStatMin / 2;
        float randomAttack = UnityEngine.Random.Range(-definedStatRange * (1 - totalAttackModifier), definedStatRange * (1 + totalAttackModifier)) + definedStatMin / 2;
        float randomDefense = UnityEngine.Random.Range(-definedStatRange * (1 - totalDefenseModifier), definedStatRange * (1 + totalDefenseModifier)) + definedStatMin / 2;
        float randomDexterity = UnityEngine.Random.Range(-definedStatRange * (1 - totalDexterityModifier), definedStatRange * (1 + totalDexterityModifier)) + definedStatMin / 2;

        
        CreatureStats creatureStats = new CreatureStats(randomSpeed, randomAttack, randomDefense, randomDexterity);
        return creatureStats;
    }

    public Creature CreateAdjustedCreature(float definedStatRange, float definedStatMin)
    {
        RefreshUnlockedParts();
        CreatureRepresentation creatureRepresentation = GetRandomCreatureRepresentation();
        
        float totalHealthModifier = creatureRepresentation.BodyParts.Select(c => c.Value).Sum(t => t.healthModifier);
        int randomHealth = Mathf.RoundToInt((UnityEngine.Random.Range(-definedStatRange * (1 - totalHealthModifier), definedStatRange * (1 + totalHealthModifier)) + definedStatMin + definedStatMin) * 1.2f);

        CreatureStats creatureStats = CreateCreatureStats(definedStatRange, definedStatMin,creatureRepresentation.BodyParts);
        
        Creature createdCreature = new Creature(0,randomHealth, creatureStats, creatureRepresentation);
        
        return createdCreature;
    }

    public BodyPart GetRandomBodyPart(BodyPartType bodyPartType)
    {
        RefreshUnlockedParts();
        
        switch (bodyPartType)
        {
            case BodyPartType.Head:
                return _unlockedHeads[UnityEngine.Random.Range(0, _unlockedHeads.Count)];
                break;
            case BodyPartType.Body:
                return _unlockedBodies[UnityEngine.Random.Range(0, _unlockedBodies.Count)];
                break;
            case BodyPartType.Legs:
                return _unlockedLegs[UnityEngine.Random.Range(0, _unlockedLegs.Count)];
                break;
            case BodyPartType.Arms:
                return _unlockedArms[UnityEngine.Random.Range(0, _unlockedArms.Count)];
                break;
        }

        return null;
    }

    public void RefreshUnlockedParts()
    {
                
        _unlockedHeads = bodyPartSets
            .Where(set => set.unlocked) 
            .SelectMany(set => set.bodyPartEntries) 
            .Where(entry => entry.bodyPartType == BodyPartType.Head)
            .Select(partEntry => partEntry.bodyPart)
            .ToList();
        
        _unlockedBodies = bodyPartSets
            .Where(set => set.unlocked) 
            .SelectMany(set => set.bodyPartEntries) 
            .Where(entry => entry.bodyPartType == BodyPartType.Body)
            .Select(partEntry => partEntry.bodyPart)
            .ToList();
        
        _unlockedArms = bodyPartSets
            .Where(set => set.unlocked) 
            .SelectMany(set => set.bodyPartEntries) 
            .Where(entry => entry.bodyPartType == BodyPartType.Arms)
            .Select(partEntry => partEntry.bodyPart)
            .ToList();
        
        _unlockedLegs = bodyPartSets
            .Where(set => set.unlocked) 
            .SelectMany(set => set.bodyPartEntries) 
            .Where(entry => entry.bodyPartType == BodyPartType.Legs)
            .Select(partEntry => partEntry.bodyPart)
            .ToList();
    }
    
    public Color GetRandomColor()
    {
        Color headColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];
        return headColor;
    }
}