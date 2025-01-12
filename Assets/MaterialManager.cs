using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager Instance { get; private set; }
    
    
    [SerializeField] 
    [Range(0,1f)]
    private float statFactor = 0.9f;
    
    [SerializeField] 
    [Range(0,1f)]
    private float speedFactor = 0.6f;
    
    [SerializeField] 
    [Range(0,1f)]
    private float dampeningFactor = 0.6f;

    
    
    //add to SaveState also change via SaveState
    [SerializeField] private float yieldSpeedMat1 = 60f;
    [SerializeField] private float yieldSpeedMat2 = 60f;
    [SerializeField] private float yieldSpeedMat3 = 60f;
    [SerializeField] private float yieldSpeedMat4 = 60f;
    
    
    private IEnumerator materialA;
    private IEnumerator materialB;
    private IEnumerator materialC;
    private IEnumerator materialD;

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

    public void StartFarming(MaterialType materialType)
    {
        Creature creature = null;
        switch (materialType)
        {
            case MaterialType.MaterialA:
                creature = InventoryManager.Instance.SelectedCreatureForMaterial_1;
                if (creature == null)
                {
                    return;
                }
                InventoryManager.Instance.AddCreatureToMaterialSlot(creature, materialType);
                materialA = MaterialA(creature);
                StartCoroutine(materialA);
                break;

            case MaterialType.MaterialB:
                creature = InventoryManager.Instance.SelectedCreatureForMaterial_2;
                if (creature == null)
                {
                    return;
                }
                InventoryManager.Instance.AddCreatureToMaterialSlot(creature, materialType);
                materialB = MaterialB(creature);
                StartCoroutine(materialB);
                break;

            case MaterialType.MaterialC:
                creature = InventoryManager.Instance.SelectedCreatureForMaterial_3;
                if (creature == null)
                {
                    return;
                }
                InventoryManager.Instance.AddCreatureToMaterialSlot(creature, materialType);
                materialC = MaterialC(creature);
                StartCoroutine(materialC);
                break;

            case MaterialType.MaterialD:
                creature = InventoryManager.Instance.SelectedCreatureForMaterial_4;
                if (creature == null)
                {
                    return;
                }
                InventoryManager.Instance.AddCreatureToMaterialSlot(creature, materialType);
                materialD = MaterialD(creature);
                StartCoroutine(materialD);
                break;
        }
    }

    public void StopFarming(MaterialType materialType)
    {
        Creature creature = null;
        switch (materialType)
        {
            case MaterialType.MaterialA:
                creature = InventoryManager.Instance.SelectedCreatureForMaterial_1;
                if (creature == null)
                {
                    return;
                }
                InventoryManager.Instance.RemoveCreatureFromMaterialSlot(materialType);
                StopCoroutine(materialA);
                break;

            case MaterialType.MaterialB:
                creature = InventoryManager.Instance.SelectedCreatureForMaterial_2;
                if (creature == null)
                {
                    return;
                }
                InventoryManager.Instance.RemoveCreatureFromMaterialSlot(materialType);
                StopCoroutine(materialB);
                break;

            case MaterialType.MaterialC:
                creature = InventoryManager.Instance.SelectedCreatureForMaterial_3;
                if (creature == null)
                {
                    return;
                }
                InventoryManager.Instance.RemoveCreatureFromMaterialSlot(materialType);
                StopCoroutine(materialC);
                break;

            case MaterialType.MaterialD:
                creature = InventoryManager.Instance.SelectedCreatureForMaterial_4;
                if (creature == null)
                {
                    return;
                }
                InventoryManager.Instance.RemoveCreatureFromMaterialSlot(materialType);
                StopCoroutine(materialD);
                break;
        }
    }

    public void StopAllMaterialCoroutines()
    {
        if(materialA != null)
        {
            StopCoroutine(materialA);
            materialA = null;
        }

        if (materialB != null)
        {
            StopCoroutine(materialB);
            materialB = null;
        }

        if (materialC != null)
        {
            StopCoroutine(materialC);
            materialC = null;
        }

        if (materialD != null)
        {
            StopCoroutine(materialD);
            materialD = null;
        }
    }

    public void StartAllMaterialCoroutines()
    {
        StartFarming(MaterialType.MaterialA);
        StartFarming(MaterialType.MaterialB);
        StartFarming(MaterialType.MaterialC);
        StartFarming(MaterialType.MaterialD);
    }

    
    private BigDecimal CalculateYield(BigDecimal stat, BigDecimal speed)
    {
        BigDecimal scaledStat = stat.Power(statFactor);
        BigDecimal scaledSpeed = speed.Power(speedFactor);
        
        BigDecimal yield = (scaledStat + scaledSpeed) * (scaledStat * scaledSpeed) ;
        BigDecimal dampenedYield = yield.Power(dampeningFactor);
        
        return dampenedYield;
    }
    
    
    public IEnumerator MaterialA(Creature creature)
    {
        BigDecimal amountToGenerate = CalculateYield(creature.CreatureStats.Attack, creature.CreatureStats.Speed);
        
        while (true)
        {
            yield return new WaitForSeconds(yieldSpeedMat1);
            InventoryManager.Instance.AddMaterialToInventory(MaterialType.MaterialA, amountToGenerate);
        }
    }

    public IEnumerator MaterialB(Creature creature)
    {
        BigDecimal amountToGenerate = CalculateYield(creature.MaxHealth, creature.CreatureStats.Speed);
        
        while (true)
        {
            yield return new WaitForSeconds(yieldSpeedMat2);
            InventoryManager.Instance.AddMaterialToInventory(MaterialType.MaterialB, amountToGenerate);
        }
    }

    public IEnumerator MaterialC(Creature creature)
    {
        BigDecimal amountToGenerate = CalculateYield(creature.CreatureStats.Defense, creature.CreatureStats.Speed);

        while (true)
        {
            yield return new WaitForSeconds(yieldSpeedMat3);
            InventoryManager.Instance.AddMaterialToInventory(MaterialType.MaterialC, amountToGenerate);
        }
    }

    public IEnumerator MaterialD(Creature creature)
    {
        BigDecimal amountToGenerate = CalculateYield(creature.CreatureStats.Dexterity, creature.CreatureStats.Speed);

        while (true)
        {
            yield return new WaitForSeconds(yieldSpeedMat4);
            InventoryManager.Instance.AddMaterialToInventory(MaterialType.MaterialD, amountToGenerate);
        }
    }
}
