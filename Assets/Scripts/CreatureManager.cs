using System;
using System.Collections;
using System.Collections.Generic;
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
    
    [Header("Prefabs")]
    [SerializeField] private GameObject creaturePrefab;

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
    
    public GameObject CreateBasicCreature()
    {
        
        GameObject newCreatureObj = Instantiate(creaturePrefab);
        newCreatureObj.transform.position = new Vector3(20000f, 20000f, 20000f);
        Creature newCreature = newCreatureObj.GetComponent<Creature>();

        
        int randomHealth = Mathf.RoundToInt((UnityEngine.Random.Range(-statRange, statRange) + statMin) + statMin * 1.2f);
        float randomSpeed = UnityEngine.Random.Range(-statRange, statRange) + statMin/2;
        float randomAttack = UnityEngine.Random.Range(-statRange, statRange) + statMin/2;
        float randomDefense = UnityEngine.Random.Range(-statRange, statRange) + statMin/2;
        float randomDexterity = UnityEngine.Random.Range(-statRange, statRange) + statMin/2;

        newCreature.SetStats(randomHealth, randomSpeed, randomAttack, randomDefense, randomDexterity);

        
        Color headColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];
        Color bodyColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];
        Color armsColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];
        Color legsColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];

        newCreature.SetColor(headColor, bodyColor, legsColor, armsColor);

        
        Sprite randomHead = creatureHeadSprites[UnityEngine.Random.Range(0, creatureHeadSprites.Length)];
        Sprite randomBody = creatureBodySprites[UnityEngine.Random.Range(0, creatureBodySprites.Length)];
        Sprite randomLegs = creatureLegsSprites[UnityEngine.Random.Range(0, creatureLegsSprites.Length)];
        Sprite randomArms = creatureArmsSprites[UnityEngine.Random.Range(0, creatureArmsSprites.Length)];

        newCreature.SetSprites(randomHead, randomBody, randomLegs, randomArms);
        
        newCreature.CreatureGeneration = 0;
        newCreature.CreatureName = newCreature.GenerateRandomName();
        Debug.Log(newCreatureObj.transform.localScale);
        
        return newCreatureObj;
    }

    public GameObject CreateAdjustedCreature(float definedStatRange, float definedStatMin)
    {
        
        GameObject newCreatureObj = Instantiate(creaturePrefab);
        newCreatureObj.transform.position = new Vector3(20000f, 20000f, 20000f);
        Creature newCreature = newCreatureObj.GetComponent<Creature>();

        
        int randomHealth = Mathf.RoundToInt((UnityEngine.Random.Range(-definedStatRange, definedStatRange) + definedStatMin) * 1.2f);
        float randomSpeed = UnityEngine.Random.Range(-definedStatRange, definedStatRange) + definedStatMin/2;
        float randomAttack = UnityEngine.Random.Range(-definedStatRange, definedStatRange) + definedStatMin/2;
        float randomDefense = UnityEngine.Random.Range(-definedStatRange, definedStatRange) + definedStatMin/2;
        float randomDexterity = UnityEngine.Random.Range(-definedStatRange, definedStatRange) + definedStatMin/2;

        newCreature.SetStats(randomHealth, randomSpeed, randomAttack, randomDefense, randomDexterity);

        
        Color headColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];
        Color bodyColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];
        Color armsColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];
        Color legsColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];
        
        newCreature.SetColor(headColor, bodyColor,legsColor, armsColor);
        
        
        Sprite randomHead = creatureHeadSprites[UnityEngine.Random.Range(0, creatureHeadSprites.Length)];
        Sprite randomBody = creatureBodySprites[UnityEngine.Random.Range(0, creatureBodySprites.Length)];
        Sprite randomLegs = creatureLegsSprites[UnityEngine.Random.Range(0, creatureLegsSprites.Length)];
        Sprite randomArms = creatureArmsSprites[UnityEngine.Random.Range(0, creatureArmsSprites.Length)];

        newCreature.SetSprites(randomHead, randomBody, randomLegs, randomArms);

        newCreature.CreatureGeneration = 0;
        newCreature.CreatureName = newCreature.GenerateRandomName();
        
        return newCreatureObj;
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