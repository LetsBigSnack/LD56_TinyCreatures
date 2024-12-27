using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BreedingManager : MonoBehaviour
{
    // Singleton instance
    public static BreedingManager Instance { get; private set; }

    [Header("Breeding")] 
    [SerializeField] private GameObject breedingPrefab;
    private Creature creaturePod1;
    private Creature creaturePod2;
    private Creature result;
    private BigDecimal breedingPrice = 0;
    //TODO: need to think about a better way 
    [SerializeField] private float winFactor = 0.5f;

    public BigDecimal BreedingPrice
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
    BodyPart newHeadSprite = Random.value < randomChance ? CreatureManager.Instance.GetRandomBodyPart(BodyPartType.Head) : (Random.value > 0.5f ? parent1.Representation.BodyParts[BodyPartType.Head] : parent2.Representation.BodyParts[BodyPartType.Head]);
    BodyPart newBodySprite = Random.value < randomChance ? CreatureManager.Instance.GetRandomBodyPart(BodyPartType.Body) : (Random.value > 0.5f ? parent1.Representation.BodyParts[BodyPartType.Body] : parent2.Representation.BodyParts[BodyPartType.Body]);
    BodyPart newLegsSprite = Random.value < randomChance ? CreatureManager.Instance.GetRandomBodyPart(BodyPartType.Legs) : (Random.value > 0.5f ? parent1.Representation.BodyParts[BodyPartType.Legs]: parent2.Representation.BodyParts[BodyPartType.Legs]);
    BodyPart newArmsSprite = Random.value < randomChance ? CreatureManager.Instance.GetRandomBodyPart(BodyPartType.Arms) : (Random.value > 0.5f ? parent1.Representation.BodyParts[BodyPartType.Arms]: parent2.Representation.BodyParts[BodyPartType.Arms]);
    
    Dictionary<BodyPartType, BodyPart> bodyParts = new Dictionary<BodyPartType, BodyPart>();
    bodyParts.Add(BodyPartType.Head, newHeadSprite);
    bodyParts.Add(BodyPartType.Body, newBodySprite);
    bodyParts.Add(BodyPartType.Legs, newLegsSprite);
    bodyParts.Add(BodyPartType.Arms, newArmsSprite);
    
    
    CreatureRepresentation creatureRepresentation = new CreatureRepresentation(bodyParts, newHeadColor, newBodyColor, newLegsColor, newArmsColor);
    BigDecimal lastGeneration = BigDecimal.Max(parent1.CreatureGeneration, parent2.CreatureGeneration) + 1;
    BigDecimal totalWins = parent1.CreatureWins + parent2.CreatureWins;
    
    float totalHealthModifier = bodyParts.Select(c => c.Value).Sum(t => t.healthModifier);
    float totalSpeedModifier = bodyParts.Select(c => c.Value).Sum(t => t.speedModifier);
    float totalAttackModifier = bodyParts.Select(c => c.Value).Sum(t => t.attackModifier);
    float totalDexterityModifier = bodyParts.Select(c => c.Value).Sum(t => t.dexterityModifier);
    float totalDefenseModifier = bodyParts.Select(c => c.Value).Sum(t => t.defenseModifier);

    
    // Combine stats from both parents and apply mutation
    BigDecimal newHealth = (parent1.MaxHealth + parent2.MaxHealth) / 2f * MutationFactor(totalHealthModifier);
    BigDecimal newSpeed = ((parent1.CreatureStats.Speed + parent2.CreatureStats.Speed) / 2f) * MutationFactor(totalSpeedModifier);
    BigDecimal newAttack = ((parent1.CreatureStats.Attack + parent2.CreatureStats.Attack) / 2f) * MutationFactor(totalAttackModifier);
    BigDecimal newDefense = ((parent1.CreatureStats.Defense + parent2.CreatureStats.Defense) / 2f) * MutationFactor(totalDefenseModifier);
    BigDecimal newDexterity = ((parent1.CreatureStats.Dexterity + parent2.CreatureStats.Dexterity) / 2f) * MutationFactor(totalDexterityModifier);

    // Ensure minimum values for stats
    newHealth = BigDecimal.Max(1, newHealth);
    newSpeed = BigDecimal.Max(1f, newSpeed);
    newAttack = BigDecimal.Max(1f, newAttack);
    newDefense = BigDecimal.Max(1f, newDefense);
    newDexterity = BigDecimal.Max(1f, newDexterity);
    CreatureStats creatureStats = new CreatureStats(newSpeed, newAttack, newDefense, newDexterity);
    
    result = new Creature(lastGeneration.Round(0), newHealth.Round(0), creatureStats, creatureRepresentation);
    return true;
}



    private BigDecimal MutationFactor(BigDecimal totalModifier)
    {
        BigDecimal t = ((-mutationFactor / (1.75f)) / 100f) * (1 - totalModifier);
        BigDecimal t2 = (((mutationFactor)) / 100f) * (1 + totalModifier);
        
        t = t.Round(3);
        t2 = t2.Round(3);
        
        BigDecimal factor = 1f + BigDecimal.Random(t, t2);
        
        return factor;
    }

    public void UpdatePrice()
    {
        if (creaturePod1 == null || creaturePod2 == null)
        {
            breedingPrice = 0;
            return;
        }
        
        BigDecimal newPrice = ((creaturePod1.CreatureStats.PowerLevel + creaturePod2.CreatureStats.PowerLevel)/2)*2;
        breedingPrice = (newPrice.Round(0));
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
