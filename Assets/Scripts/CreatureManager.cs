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
    [SerializeField] private float statRange = 5.5f;
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
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        //CreateBasicCreature();
    }


    
    public GameObject CreateBasicCreature()
    {
        
        GameObject newCreatureObj = Instantiate(creaturePrefab);
        Creature newCreature = newCreatureObj.GetComponent<Creature>();

        
        int randomHealth = Mathf.RoundToInt(UnityEngine.Random.Range(0, statRange) + statMin);
        float randomSpeed = UnityEngine.Random.Range(0, statRange) + statMin;
        float randomAttack = UnityEngine.Random.Range(0, statRange) + statMin;
        float randomDefense = UnityEngine.Random.Range(0, statRange) + statMin;
        float randomDexterity = UnityEngine.Random.Range(0, statRange) + statMin;

        newCreature.SetStats(randomHealth, randomSpeed, randomAttack, randomDefense, randomDexterity);

        
        Color headColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];
        Color bodyColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];
        Color armsColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];
        Color legsColor = creatureColors[UnityEngine.Random.Range(0, creatureColors.Length)];

        newCreature.SetColor(headColor, bodyColor, armsColor, legsColor);

        
        Sprite randomHead = creatureHeadSprites[UnityEngine.Random.Range(0, creatureHeadSprites.Length)];
        Sprite randomBody = creatureBodySprites[UnityEngine.Random.Range(0, creatureBodySprites.Length)];
        Sprite randomLegs = creatureLegsSprites[UnityEngine.Random.Range(0, creatureLegsSprites.Length)];
        Sprite randomArms = creatureArmsSprites[UnityEngine.Random.Range(0, creatureArmsSprites.Length)];

        newCreature.SetSprites(randomHead, randomBody, randomLegs, randomArms);

        return newCreatureObj;
    }

    public GameObject CreateAdjustedCreature(float definedStatRange, float definedStatMin)
    {
        
        GameObject newCreatureObj = Instantiate(creaturePrefab);
        Creature newCreature = newCreatureObj.GetComponent<Creature>();

        
        int randomHealth = Mathf.RoundToInt(UnityEngine.Random.Range(0, definedStatRange) * definedStatMin);
        float randomSpeed = UnityEngine.Random.Range(0, definedStatRange) * definedStatMin;
        float randomAttack = UnityEngine.Random.Range(0, definedStatRange) * definedStatMin;
        float randomDefense = UnityEngine.Random.Range(0, definedStatRange) * definedStatMin;
        float randomDexterity = UnityEngine.Random.Range(0, definedStatRange) * definedStatMin;

        newCreature.SetStats(randomHealth, randomSpeed, randomAttack, randomDefense, randomDexterity);

        
        Sprite randomHead = creatureHeadSprites[UnityEngine.Random.Range(0, creatureHeadSprites.Length)];
        Sprite randomBody = creatureBodySprites[UnityEngine.Random.Range(0, creatureBodySprites.Length)];
        Sprite randomLegs = creatureLegsSprites[UnityEngine.Random.Range(0, creatureLegsSprites.Length)];
        Sprite randomArms = creatureArmsSprites[UnityEngine.Random.Range(0, creatureArmsSprites.Length)];

        newCreature.SetSprites(randomHead, randomBody, randomLegs, randomArms);

        return newCreatureObj;
    }
}