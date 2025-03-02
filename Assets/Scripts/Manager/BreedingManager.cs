using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class BreedingManager : MonoBehaviour
{
    // Singleton instance
    public static BreedingManager Instance { get; private set; }

    [Header("Breeding")]
    [SerializeField] private GameObject breedingPrefab;
    private Creature creaturePod1;
    private Creature creaturePod2;

    public Action<Creature> OnCreatureChangePod1;
    public Action<Creature> OnCreatureChangePod2;

    private Creature result;
    public Action<Creature> OnCreatureChangeResult;

    private BigDecimal breedingPrice = 0;

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
            OnCreatureChangePod2.Invoke(creaturePod2);
        }
        else
        {
            creaturePod1 = creature;
            OnCreatureChangePod1.Invoke(creaturePod1);
        }

        UpdatePrice();
        return true;
    }

    public bool AddToBreedLeft(Creature creature)
    {
        if (creaturePod1 != null)
        {
            return false;
        }

        creaturePod1 = creature;
        OnCreatureChangePod1.Invoke(creaturePod1);
        UpdatePrice();
        return true;
    }

    public bool AddToBreedRight(Creature creature)
    {
        if (creaturePod2 != null)
        {
            return false;
        }

        creaturePod2 = creature;
        OnCreatureChangePod2.Invoke(creaturePod2);
        UpdatePrice();
        return true;
    }

    public void SellResult()
    {
        StoreManager.Instance.SellOwnedCreature(result);
        result = null;
        OnCreatureChangeResult.Invoke(result);
    }

    public bool RemoveFromBreed(bool isLeft)
    {

        if (isLeft)
        {
            if (creaturePod1 != null && InventoryManager.Instance.HasSpace())
            {
                InventoryManager.Instance.AddCreature(creaturePod1);
                breedingPrice = 0;
                creaturePod1 = null;
                OnCreatureChangePod1.Invoke(creaturePod1);
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
                OnCreatureChangePod2.Invoke(creaturePod2);
                return true;
            }
        }

        return false;
    }

    public bool Breed(bool pay = true, float randomChance = 0.05f) // randomChance parameter added
    {
        if (creaturePod1 == null || creaturePod2 == null)
        {
            return false;
        }

        Creature parent1 = creaturePod1;
        Creature parent2 = creaturePod2;

        if ((parent1 == null || parent2 == null))
        {
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
        List<BaseColor> colorPodBase = new List<BaseColor>
    {
        parent1.Representation.BaseColor,
        parent2.Representation.BaseColor,

    };

        List<AddOnColor> colorPodAddOn = new List<AddOnColor>
    {
        parent1.Representation.AddOnColor,
        parent2.Representation.AddOnColor,
    };

        // Randomly assign colors from the pod to the new creature's body parts
        BaseColor newBaseColor = Random.value < randomChance ? ColorManager.Instance.RandomBaseColor() : colorPodBase[Random.Range(0, colorPodBase.Count)];
        AddOnColor newAddOnColor = Random.value < randomChance ? ColorManager.Instance.RandomAddOnColor() : colorPodAddOn[Random.Range(0, colorPodAddOn.Count)];

        // Randomly assign sprites from the parents or use random body parts based on the randomChance
        BodyPart newHeadSprite = Random.value < randomChance ? CreatureManager.Instance.GetRandomBodyPart(BodyPartType.Head) : (Random.value > 0.5f ? parent1.Representation.BodyParts[BodyPartType.Head] : parent2.Representation.BodyParts[BodyPartType.Head]);
        BodyPart newBodySprite = Random.value < randomChance ? CreatureManager.Instance.GetRandomBodyPart(BodyPartType.Body) : (Random.value > 0.5f ? parent1.Representation.BodyParts[BodyPartType.Body] : parent2.Representation.BodyParts[BodyPartType.Body]);
        BodyPart newLegsSprite = Random.value < randomChance ? CreatureManager.Instance.GetRandomBodyPart(BodyPartType.Legs) : (Random.value > 0.5f ? parent1.Representation.BodyParts[BodyPartType.Legs] : parent2.Representation.BodyParts[BodyPartType.Legs]);
        BodyPart newArmsSprite = Random.value < randomChance ? CreatureManager.Instance.GetRandomBodyPart(BodyPartType.Arms) : (Random.value > 0.5f ? parent1.Representation.BodyParts[BodyPartType.Arms] : parent2.Representation.BodyParts[BodyPartType.Arms]);
        BodyPart newTopHeadSprite = Random.value < randomChance ? CreatureManager.Instance.GetRandomBodyPart(BodyPartType.TopHead) : (Random.value > 0.5f ? parent1.Representation.BodyParts[BodyPartType.TopHead] : parent2.Representation.BodyParts[BodyPartType.TopHead]);
        BodyPart newBackSprite = Random.value < randomChance ? CreatureManager.Instance.GetRandomBodyPart(BodyPartType.Back) : (Random.value > 0.5f ? parent1.Representation.BodyParts[BodyPartType.Back] : parent2.Representation.BodyParts[BodyPartType.Back]);
        BodyPart newTailSprite = Random.value < randomChance ? CreatureManager.Instance.GetRandomBodyPart(BodyPartType.Tail) : (Random.value > 0.5f ? parent1.Representation.BodyParts[BodyPartType.Tail] : parent2.Representation.BodyParts[BodyPartType.Tail]);

        Dictionary<BodyPartType, BodyPart> bodyParts = new Dictionary<BodyPartType, BodyPart>();
        bodyParts.Add(BodyPartType.Head, newHeadSprite);
        bodyParts.Add(BodyPartType.Body, newBodySprite);
        bodyParts.Add(BodyPartType.Legs, newLegsSprite);
        bodyParts.Add(BodyPartType.Arms, newArmsSprite);
        bodyParts.Add(BodyPartType.TopHead, newTopHeadSprite);
        bodyParts.Add(BodyPartType.Back, newBackSprite);
        bodyParts.Add(BodyPartType.Tail, newTailSprite);

        CreatureRepresentation creatureRepresentation = new CreatureRepresentation(bodyParts, newBaseColor, newAddOnColor);
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
        OnCreatureChangeResult.Invoke(result);
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

        BigDecimal newPrice = ((creaturePod1.CreatureStats.PowerLevel + creaturePod2.CreatureStats.PowerLevel) / 2) * 2;
        breedingPrice = (newPrice.Round(0));
    }

    public bool Collect()
    {

        if (result != null && InventoryManager.Instance.HasSpace())
        {
            InventoryManager.Instance.AddCreature(result);
            result = null;
            OnCreatureChangeResult.Invoke(result);
            return true;
        }

        return false;
    }
}
