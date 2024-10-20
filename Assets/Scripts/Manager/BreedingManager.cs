using System;
using System.Collections;
using System.Collections.Generic;
using Data;
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
    //TODO: need to think about a better way 
    [SerializeField] private float winFactor = 0.5f;

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
        }
    }
    
    public bool AddToBreed(Creature creature)
    {
        if (creaturePod1 != null && creaturePod2 != null)
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
            if (creaturePod1 != null && InventoryManager.Instance.HasSpace())
            {
                InventoryManager.Instance.AddCreature(creaturePod1);
                breedingPrice = 0;
                creaturePod1 = null;
                return true;
            }
        }
        else
        {
            if (creaturePod2 != null && InventoryManager.Instance.HasSpace())
            {
                InventoryManager.Instance.AddCreature(creaturePod2);
                breedingPrice = 0;
                creaturePod2 = null;
                return true;
            }
        }
        
        return false;
    }
    
public bool Breed(bool pay = true, float randomChance = 0.05f) // randomChance parameter added
{
    if(creaturePod1 == null || creaturePod2 == null)
    {
        return false;
    }

    Creature parent1 = creaturePod1;
    Creature parent2 = creaturePod2;

    if ((parent1 == null || parent2 == null ))
    {
        Debug.LogError("One or both parent creatures are missing.");
        return false;
    }

    if (pay == true)
    {
        if (StoreManager.Instance.PlayerMoney < BreedingPrice)
        {
            return false;
        }
    }
    
    StoreManager.Instance.SpendMoney(BreedingPrice);


    int totalWins = parent1.CreatureWins + parent2.CreatureWins;
    
    // Combine stats from both parents and apply mutation
    int newHealth = Mathf.RoundToInt((parent1.MaxHealth + parent2.MaxHealth) / 2f * MutationFactor(totalWins));
    float newSpeed = ((parent1.CreatureStats.Speed + parent2.CreatureStats.Speed) / 2f) * MutationFactor(totalWins);
    float newAttack = ((parent1.CreatureStats.Attack + parent2.CreatureStats.Attack) / 2f) * MutationFactor(totalWins);
    float newDefense = ((parent1.CreatureStats.Defense + parent2.CreatureStats.Defense) / 2f) * MutationFactor(totalWins);
    float newDexterity = ((parent1.CreatureStats.Dexterity + parent2.CreatureStats.Dexterity) / 2f) * MutationFactor(totalWins);

    // Ensure minimum values for stats
    newHealth = Mathf.Max(1, newHealth);
    newSpeed = Mathf.Max(1f, newSpeed);
    newAttack = Mathf.Max(1f, newAttack);
    newDefense = Mathf.Max(1f, newDefense);
    newDexterity = Mathf.Max(1f, newDexterity);
    

    // Create a "color pod" from all body parts of both parents
    List<Color> colorPod = new List<Color>
    {
        parent1.Representation.HeadColor,
        parent1.Representation.BodyColor,
        parent1.Representation.LegsColor,
        parent1.Representation.ArmsColor,
        parent2.Representation.HeadColor,
        parent2.Representation.BodyColor,
        parent2.Representation.LegsColor,
        parent2.Representation.ArmsColor,

    };

    // Randomly assign colors from the pod to the new creature's body parts
    Color newHeadColor = Random.value < randomChance ? CreatureManager.Instance.GetRandomColor() : colorPod[Random.Range(0, colorPod.Count)];
    Color newBodyColor = Random.value < randomChance ? CreatureManager.Instance.GetRandomColor() : colorPod[Random.Range(0, colorPod.Count)];
    Color newArmsColor = Random.value < randomChance ? CreatureManager.Instance.GetRandomColor()  : colorPod[Random.Range(0, colorPod.Count)];
    Color newLegsColor = Random.value < randomChance ? CreatureManager.Instance.GetRandomColor()  : colorPod[Random.Range(0, colorPod.Count)];
    
    // Randomly assign sprites from the parents or use random body parts based on the randomChance
    Sprite newHeadSprite = Random.value < randomChance ? CreatureManager.Instance.GetRandomBodyPart("Head") : (Random.value > 0.5f ? parent1.Representation.HeadSprite : parent2.Representation.HeadSprite);
    Sprite newBodySprite = Random.value < randomChance ? CreatureManager.Instance.GetRandomBodyPart("Body") : (Random.value > 0.5f ? parent1.Representation.BodySprite : parent2.Representation.BodySprite);
    Sprite newLegsSprite = Random.value < randomChance ? CreatureManager.Instance.GetRandomBodyPart("Legs") : (Random.value > 0.5f ? parent1.Representation.LegsSprite: parent2.Representation.LegsSprite);
    Sprite newArmsSprite = Random.value < randomChance ? CreatureManager.Instance.GetRandomBodyPart("Arms") : (Random.value > 0.5f ? parent1.Representation.ArmsSprite: parent2.Representation.ArmsSprite);

    CreatureRepresentation creatureRepresentation = new CreatureRepresentation(newHeadSprite, newBodySprite, newLegsSprite, newArmsSprite, newHeadColor, newBodyColor, newLegsColor, newArmsColor);
    int lastGeneration = Mathf.Max(parent1.CreatureGeneration, parent2.CreatureGeneration) + 1;
    CreatureStats creatureStats = new CreatureStats(newSpeed, newAttack, newDefense, newDexterity);
    
    
    result = new Creature(lastGeneration, newHealth, creatureStats, creatureRepresentation);
    return true;
}



    private float MutationFactor(int totalWins)
    {
        //TODO: think about creatureWins
        float factor = 1f + Random.Range((-mutationFactor/(1.5f)) / 100f, (mutationFactor + (totalWins * winFactor)) / 100f);
        
        Debug.Log("Factor: " + factor);
        
        return factor;
    }

    public void UpdatePrice()
    {
        if (creaturePod1 == null || creaturePod2 == null)
        {
            Debug.Log("Test");
            return;
        }
        breedingPrice = Mathf.RoundToInt((creaturePod1.CreatureStats.PowerLevel + creaturePod2.CreatureStats.PowerLevel)/2)*2;
    }

    public bool Collect()
    {
        
        if (result != null && InventoryManager.Instance.HasSpace())
        {
            InventoryManager.Instance.AddCreature(result);
            result = null;
            return true;
        }

        return false;
    }
}
