using System;
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

    private MaterialStats materialStatsA;
    private MaterialStats materialStatsB;
    private MaterialStats materialStatsC;
    private MaterialStats materialStatsD;

    public static event Action<BigDecimal> OnChangesYieldAmountMat1;
    public static event Action<BigDecimal> OnChangesYieldAmountMat2;
    public static event Action<BigDecimal> OnChangesYieldAmountMat3;
    public static event Action<BigDecimal> OnChangesYieldAmountMat4;

    public static event Action<float> OnChangesYieldTimeMat1;
    public static event Action<float> OnChangesYieldTimeMat2;
    public static event Action<float> OnChangesYieldTimeMat3;
    public static event Action<float> OnChangesYieldTimeMat4;

    private IEnumerator materialA;
    private IEnumerator materialB;
    private IEnumerator materialC;
    private IEnumerator materialD;
    public MaterialStats MaterialStatsA
    {
        get { return materialStatsA; }
        set { materialStatsA = value; }
    }

    public MaterialStats MaterialStatsB
    {
        get { return materialStatsB; }
        set { materialStatsB = value; }
    }

    public MaterialStats MaterialStatsC
    {
        get { return materialStatsC; }
        set { materialStatsC = value; }
    }

    public MaterialStats MaterialStatsD
    {
        get { return materialStatsD; }
        set { materialStatsD = value; }
    }

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
                OnChangesYieldAmountMat1?.Invoke(0);
                materialStatsA.yieldTime = 0;
                break;

            case MaterialType.MaterialB:
                creature = InventoryManager.Instance.SelectedCreatureForMaterial_2;
                if (creature == null)
                {
                    return;
                }
                InventoryManager.Instance.RemoveCreatureFromMaterialSlot(materialType);
                StopCoroutine(materialB);
                materialStatsB.yieldTime = 0;
                OnChangesYieldAmountMat2?.Invoke(0);
                break;

            case MaterialType.MaterialC:
                creature = InventoryManager.Instance.SelectedCreatureForMaterial_3;
                if (creature == null)
                {
                    return;
                }
                InventoryManager.Instance.RemoveCreatureFromMaterialSlot(materialType);
                StopCoroutine(materialC);
                materialStatsC.yieldTime = 0;
                OnChangesYieldAmountMat3?.Invoke(0);
                break;

            case MaterialType.MaterialD:
                creature = InventoryManager.Instance.SelectedCreatureForMaterial_4;
                if (creature == null)
                {
                    return;
                }
                InventoryManager.Instance.RemoveCreatureFromMaterialSlot(materialType);
                StopCoroutine(materialD);
                materialStatsD.yieldTime = 0;
                OnChangesYieldAmountMat4?.Invoke(0);
                break;
        }
    }

    public void StopAllMaterialCoroutines()
    {
        if(materialA != null)
        {
            StopCoroutine(materialA);
            OnChangesYieldAmountMat1?.Invoke(0);
            materialA = null;
        }

        if (materialB != null)
        {
            StopCoroutine(materialB);
            OnChangesYieldAmountMat2?.Invoke(0);
            materialB = null;
        }

        if (materialC != null)
        {
            StopCoroutine(materialC);
            OnChangesYieldAmountMat3?.Invoke(0);
            materialC = null;
        }

        if (materialD != null)
        {
            StopCoroutine(materialD);
            OnChangesYieldAmountMat4?.Invoke(0);
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

    public BigDecimal CalculateYield(BigDecimal stat, BigDecimal speed)
    {
        BigDecimal scaledStat = stat.Power(statFactor);
        BigDecimal scaledSpeed = speed.Power(speedFactor);
        
        BigDecimal yield = (scaledStat + scaledSpeed) * (scaledStat * scaledSpeed) ;
        BigDecimal dampenedYield = yield.Power(dampeningFactor);
        
        return dampenedYield;
    }
    
    public IEnumerator MaterialA(Creature creature)
    {
        materialStatsA.yieldAmount = CalculateYield(creature.CreatureStats.Attack, creature.CreatureStats.Speed);
        OnChangesYieldAmountMat1?.Invoke(materialStatsA.yieldAmount);

        while (true)
        {
            materialStatsA.yieldTime += 0.1f;
            if (materialStatsA.yieldTime >= materialStatsA.yieldSpeed)
            {
                InventoryManager.Instance.AddMaterialToInventory(MaterialType.MaterialA, materialStatsA.yieldAmount);
                materialStatsA.yieldTime = 0;
            }
            OnChangesYieldTimeMat1?.Invoke(materialStatsA.yieldTime);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator MaterialB(Creature creature)
    {
        materialStatsB.yieldAmount = CalculateYield(creature.MaxHealth, creature.CreatureStats.Speed);
        OnChangesYieldAmountMat2?.Invoke(materialStatsB.yieldAmount);

        while (true)
        {
            materialStatsB.yieldTime += 0.1f;
            if (materialStatsB.yieldTime >= materialStatsB.yieldSpeed)
            {
                InventoryManager.Instance.AddMaterialToInventory(MaterialType.MaterialB, materialStatsB.yieldAmount);
                materialStatsB.yieldTime = 0;
            }
            OnChangesYieldTimeMat2?.Invoke(materialStatsB.yieldTime);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator MaterialC(Creature creature)
    {
        materialStatsC.yieldAmount = CalculateYield(creature.CreatureStats.Defense, creature.CreatureStats.Speed);
        OnChangesYieldAmountMat3?.Invoke(materialStatsC.yieldAmount);

        while (true)
        {
            materialStatsC.yieldTime += 0.1f;
            if (materialStatsC.yieldTime >= materialStatsC.yieldSpeed)
            {
                InventoryManager.Instance.AddMaterialToInventory(MaterialType.MaterialC, materialStatsC.yieldAmount);
                materialStatsC.yieldTime = 0;
            }
            OnChangesYieldTimeMat3?.Invoke(materialStatsC.yieldTime);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator MaterialD(Creature creature)
    {
        materialStatsD.yieldAmount = CalculateYield(creature.CreatureStats.Dexterity, creature.CreatureStats.Speed);
        OnChangesYieldAmountMat4?.Invoke(materialStatsD.yieldAmount);

        while (true)
        {
            materialStatsD.yieldTime += 0.1f;
            if (materialStatsD.yieldTime >= materialStatsD.yieldSpeed)
            {
                InventoryManager.Instance.AddMaterialToInventory(MaterialType.MaterialD, materialStatsD.yieldAmount);
                materialStatsD.yieldTime = 0;
            }
            OnChangesYieldTimeMat4?.Invoke(materialStatsD.yieldTime);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
