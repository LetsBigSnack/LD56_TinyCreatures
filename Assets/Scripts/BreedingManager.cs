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

    Creature parent1 = creaturePod1.GetComponent<Creature>();
    Creature parent2 = creaturePod2.GetComponent<Creature>();

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
    
    GameObject newCreatureObj = Instantiate(breedingPrefab);
    Creature newCreature = newCreatureObj.GetComponent<Creature>();

    // Combine stats from both parents and apply mutation
    int newHealth = Mathf.RoundToInt((parent1.MaxHealth + parent2.MaxHealth) / 2f * MutationFactor());
    float newSpeed = ((parent1.Speed + parent2.Speed) / 2f) * MutationFactor();
    float newAttack = ((parent1.Attack + parent2.Attack) / 2f) * MutationFactor();
    float newDefense = ((parent1.Defense + parent2.Defense) / 2f) * MutationFactor();
    float newDexterity = ((parent1.Dexterity + parent2.Dexterity) / 2f) * MutationFactor();

    // Ensure minimum values for stats
    newHealth = Mathf.Max(1, newHealth);
    newSpeed = Mathf.Max(1f, newSpeed);
    newAttack = Mathf.Max(1f, newAttack);
    newDefense = Mathf.Max(1f, newDefense);
    newDexterity = Mathf.Max(1f, newDexterity);

    newCreature.SetStats(newHealth, newSpeed, newAttack, newDefense, newDexterity);

    // Create a "color pod" from all body parts of both parents
    List<Color> colorPod = new List<Color>
    {
        parent1.HeadRenderer.color,
        parent1.BodyRenderer.color,
        parent1.ArmsRenderer.color,
        parent1.LegsRenderer.color,
        parent2.HeadRenderer.color,
        parent2.BodyRenderer.color,
        parent2.ArmsRenderer.color,
        parent2.LegsRenderer.color
    };

    // Randomly assign colors from the pod to the new creature's body parts
    Color newHeadColor = Random.value < randomChance ? CreatureManager.Instance.GetRandomColor() : colorPod[Random.Range(0, colorPod.Count)];
    Color newBodyColor = Random.value < randomChance ? CreatureManager.Instance.GetRandomColor() : colorPod[Random.Range(0, colorPod.Count)];
    Color newArmsColor = Random.value < randomChance ? CreatureManager.Instance.GetRandomColor()  : colorPod[Random.Range(0, colorPod.Count)];
    Color newLegsColor = Random.value < randomChance ? CreatureManager.Instance.GetRandomColor()  : colorPod[Random.Range(0, colorPod.Count)];

    newCreature.SetColor(newHeadColor, newBodyColor, newLegsColor, newArmsColor);

    // Randomly assign sprites from the parents or use random body parts based on the randomChance
    Sprite newHeadSprite = Random.value < randomChance ? CreatureManager.Instance.GetRandomBodyPart("Head") : (Random.value > 0.5f ? parent1.HeadRenderer.sprite : parent2.HeadRenderer.sprite);
    Sprite newBodySprite = Random.value < randomChance ? CreatureManager.Instance.GetRandomBodyPart("Body") : (Random.value > 0.5f ? parent1.BodyRenderer.sprite : parent2.BodyRenderer.sprite);
    Sprite newLegsSprite = Random.value < randomChance ? CreatureManager.Instance.GetRandomBodyPart("Legs") : (Random.value > 0.5f ? parent1.LegsRenderer.sprite : parent2.LegsRenderer.sprite);
    Sprite newArmsSprite = Random.value < randomChance ? CreatureManager.Instance.GetRandomBodyPart("Arms") : (Random.value > 0.5f ? parent1.ArmsRenderer.sprite : parent2.ArmsRenderer.sprite);

    newCreature.SetSprites(newHeadSprite, newBodySprite, newLegsSprite, newArmsSprite);
    
    // Assign generation based on the higher of the two parents' generations
    int lastGeneration = Mathf.Max(parent1.CreatureGeneration, parent2.CreatureGeneration) + 1;
    newCreature.CreatureGeneration = lastGeneration;
    newCreature.CreatureName = newCreature.GenerateRandomName();
    result = newCreature;
    return true;
}



    private float MutationFactor()
    {
        float factor = 1f + Random.Range((-mutationFactor/(1.5f)) / 100f, mutationFactor / 100f);
        
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
