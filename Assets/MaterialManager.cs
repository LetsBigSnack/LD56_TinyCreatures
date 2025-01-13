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
                StopCoroutine(UI_MaterialManager.Instance.SliderA);
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
                StopCoroutine(UI_MaterialManager.Instance.SliderB);
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
                StopCoroutine(UI_MaterialManager.Instance.SliderC);
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
                StopCoroutine(UI_MaterialManager.Instance.SliderD);
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
            StopCoroutine(UI_MaterialManager.Instance.SliderA);
            OnChangesYieldAmountMat1?.Invoke(0);
            materialA = null;
        }

        if (materialB != null)
        {
            StopCoroutine(materialB);
            StopCoroutine(UI_MaterialManager.Instance.SliderB);
            OnChangesYieldAmountMat2?.Invoke(0);
            materialB = null;
        }

        if (materialC != null)
        {
            StopCoroutine(materialC);
            StopCoroutine(UI_MaterialManager.Instance.SliderC);
            OnChangesYieldAmountMat3?.Invoke(0);
            materialC = null;
        }

        if (materialD != null)
        {
            StopCoroutine(materialD);
            StopCoroutine(UI_MaterialManager.Instance.SliderD);
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
        BigDecimal amount = CalculateYield(creature.CreatureStats.Attack, creature.CreatureStats.Speed);
        OnChangesYieldAmountMat1?.Invoke(amount);
        UI_MaterialManager.Instance.SliderA = StartCoroutine(UI_MaterialManager.Instance.SliderMatA());

        while (true)
        {
            if (materialStatsA.yieldTime >= materialStatsA.yieldSpeed)
            {
                InventoryManager.Instance.AddMaterialToInventory(MaterialType.MaterialA, amount);
                materialStatsA.yieldTime = 0;
            } 
            else
            {
                materialStatsA.yieldTime++;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator MaterialB(Creature creature)
    {
        Debug.Log("I started this!");
        BigDecimal amount = CalculateYield(creature.MaxHealth, creature.CreatureStats.Speed);
        OnChangesYieldAmountMat2?.Invoke(amount);
        UI_MaterialManager.Instance.SliderB = StartCoroutine(UI_MaterialManager.Instance.SliderMatB());

        while (true)
        {
            if (materialStatsB.yieldTime >= materialStatsB.yieldSpeed)
            {
                InventoryManager.Instance.AddMaterialToInventory(MaterialType.MaterialB, amount);
                materialStatsB.yieldTime = 0;
            }
            else
            {
                materialStatsB.yieldTime++;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator MaterialC(Creature creature)
    {
        BigDecimal amount = CalculateYield(creature.CreatureStats.Defense, creature.CreatureStats.Speed);
        OnChangesYieldAmountMat3?.Invoke(amount);
        UI_MaterialManager.Instance.SliderC = StartCoroutine(UI_MaterialManager.Instance.SliderMatC());

        while (true)
        {
            if (materialStatsC.yieldTime >= materialStatsC.yieldSpeed)
            {
                InventoryManager.Instance.AddMaterialToInventory(MaterialType.MaterialC, amount);
                materialStatsC.yieldTime = 0;
            }
            else
            {
                materialStatsC.yieldTime++;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator MaterialD(Creature creature)
    {
        BigDecimal amount = CalculateYield(creature.CreatureStats.Dexterity, creature.CreatureStats.Speed);
        OnChangesYieldAmountMat4?.Invoke(amount);
        UI_MaterialManager.Instance.SliderD = StartCoroutine(UI_MaterialManager.Instance.SliderMatD());

        while (true)
        {
            if (materialStatsD.yieldTime >= materialStatsD.yieldSpeed)
            {
                InventoryManager.Instance.AddMaterialToInventory(MaterialType.MaterialD, amount);
                materialStatsD.yieldTime = 0;
            }
            else
            {
                materialStatsD.yieldTime++;
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
