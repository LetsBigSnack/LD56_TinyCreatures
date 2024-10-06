using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BreedingManager : MonoBehaviour
{
    // Singleton instance
    public static BreedingManager Instance { get; private set; }

    [Header("Breeding")] 
    [SerializeField] private GameObject breedingPrefab;
    [SerializeField] private Creature creaturePod1;
    [SerializeField] private Creature creaturePod2;
    [SerializeField] private Creature result;
    [SerializeField] private int breedingPrice = 0;

    public int BreedingPrice
    {
        get => breedingPrice;
    }
    
    private HashSet<Creature> _breedingCreatures;
    
    public Creature CreaturePod1 { get => creaturePod1; set => creaturePod1 = value; }
    public Creature CreaturePod2 { get => creaturePod2; set => creaturePod2 = value; }
    
    public Creature Result { get => result; set => result = value; }
    
    [Range(0f, 100f)]
    [SerializeField] private float mutationFactor = 20.0f;

    private void Awake()
    {
        // Ensure that there is only one instance of BreedingManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure this instance persists across scenes
        }
    }
    
    public bool AddToBreed(Creature creature)
    {
        if (creaturePod1 && creaturePod2)
        {
            return false;
        }

        
        
        if (creaturePod1 != null)
        {
            creaturePod2 = creature;
            
        }
        else
        {
            creaturePod1 = creature;
   
        }
        
        UpdatePrice();
        return true;
    }

    public bool RemoveToBreed(bool isLeft)
    {

        if (isLeft)
        {
            if (creaturePod1 != null)
            {
                InventoryManager.Instance.AddCreature(creaturePod1);
                breedingPrice = 0;
                creaturePod1 = null;
            }
        }
        else
        {
            if (creaturePod2 != null)
            {
                InventoryManager.Instance.AddCreature(creaturePod2);
                breedingPrice = 0;
                creaturePod2 = null;
            }
        }
        
        return false;
    }
    
   public void Breed()
    {
        Creature parent1 = creaturePod1.GetComponent<Creature>();
        Creature parent2 = creaturePod2.GetComponent<Creature>();

        if ((parent1 == null || parent2 == null ) && StoreManager.Instance.PlayerMoney < BreedingPrice)
        {
            Debug.LogError("One or both parent creatures are missing.");
            return;
        }
        
        StoreManager.Instance.SpendMoney(BreedingPrice);
        
        GameObject newCreatureObj = Instantiate(breedingPrefab);
        Creature newCreature = newCreatureObj.GetComponent<Creature>();

        
        int newHealth = Mathf.RoundToInt((parent1.MaxHealth + parent2.MaxHealth) / 2f * MutationFactor());
        float newSpeed = ((parent1.Speed + parent2.Speed) / 2f) * MutationFactor();
        float newAttack = ((parent1.Attack + parent2.Attack) / 2f) * MutationFactor();
        float newDefense = ((parent1.Defense + parent2.Defense) / 2f) * MutationFactor();
        float newDexterity = ((parent1.Dexterity + parent2.Dexterity) / 2f) * MutationFactor();

        
        newHealth = Mathf.Max(1, newHealth);
        newSpeed = Mathf.Max(1f, newSpeed);
        newAttack = Mathf.Max(1f, newAttack);
        newDefense = Mathf.Max(1f, newDefense);
        newDexterity = Mathf.Max(1f, newDexterity);

        
        newCreature.SetStats(newHealth, newSpeed, newAttack, newDefense, newDexterity);

        
        Color newHeadColor = Random.value > 0.5f ? parent1.HeadRenderer.color : parent2.HeadRenderer.color;
        Color newBodyColor = Random.value > 0.5f ? parent1.BodyRenderer.color : parent2.BodyRenderer.color;
        Color newArmsColor = Random.value > 0.5f ? parent1.LegsRenderer.color : parent2.LegsRenderer.color;
        Color newLegsColor = Random.value > 0.5f ? parent1.ArmsRenderer.color : parent2.ArmsRenderer.color;

        
        newCreature.SetColor(newHeadColor, newBodyColor, newLegsColor, newArmsColor);

        
        Sprite newHeadSprite = Random.value > 0.5f ? parent1.HeadRenderer.sprite : parent2.HeadRenderer.sprite;
        Sprite newBodySprite = Random.value > 0.5f ? parent1.BodyRenderer.sprite : parent2.BodyRenderer.sprite;
        Sprite newLegsSprite = Random.value > 0.5f ? parent1.LegsRenderer.sprite : parent2.LegsRenderer.sprite;
        Sprite newArmsSprite = Random.value > 0.5f ? parent1.ArmsRenderer.sprite : parent2.ArmsRenderer.sprite;

        
        newCreature.SetSprites(newHeadSprite, newBodySprite, newLegsSprite, newArmsSprite);
        
        
        int lastGeneration = parent1.CreatureGeneration > parent2.CreatureGeneration ? parent1.CreatureGeneration : parent2.CreatureGeneration;
        lastGeneration++;

        newCreature.CreatureGeneration = lastGeneration;
        
        result = newCreature;
    }

    private float MutationFactor()
    {
        float factor = 1f + Random.Range((-mutationFactor/2) / 100f, mutationFactor / 100f);
        
        Debug.Log("Factor: " + factor);
        
        return factor;
    }

    public void UpdatePrice()
    {
        if (!creaturePod1 || !creaturePod2)
        {
            return;
        }
        breedingPrice = Mathf.RoundToInt((creaturePod1.PowerLevel + creaturePod2.PowerLevel)/2)*2;
    }

    public void Collect()
    {
        if (result != null)
        {
            InventoryManager.Instance.AddCreature(result);
            result = null;
        }
    }
}
