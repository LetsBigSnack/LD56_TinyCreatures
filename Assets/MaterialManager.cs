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
    
    //add to SaveState also change via SaveState
    [SerializeField] private float yieldSpeedMat1 = 60f;
    [SerializeField] private float yieldSpeedMat2 = 60f;
    [SerializeField] private float yieldSpeedMat3 = 60f;
    [SerializeField] private float yieldSpeedMat4 = 60f;

    public static event Action<int> OnChangesYieldTimeMat1;
    public static event Action<int> OnChangesYieldTimeMat2;
    public static event Action<int> OnChangesYieldTimeMat3;
    public static event Action<int> OnChangesYieldTimeMat4;

    [SerializeField] private int curYieldTimeMat1;
    [SerializeField] private int curYieldTimeMat2;
    [SerializeField] private int curYieldTimeMat3;
    [SerializeField] private int curYieldTimeMat4;

    [SerializeField] private BigDecimal curYieldAmountMat1;
    [SerializeField] private BigDecimal curYieldAmountMat2;
    [SerializeField] private BigDecimal curYieldAmountMat3;
    [SerializeField] private BigDecimal curYieldAmountMat4;

    public static event Action<BigDecimal> OnChangesYieldAmountMat1;
    public static event Action<BigDecimal> OnChangesYieldAmountMat2;
    public static event Action<BigDecimal> OnChangesYieldAmountMat3;
    public static event Action<BigDecimal> OnChangesYieldAmountMat4;

    private IEnumerator materialA;
    private IEnumerator materialB;
    private IEnumerator materialC;
    private IEnumerator materialD;

    public float YieldSpeedMat1
    {
        get { return yieldSpeedMat1; }
        set { yieldSpeedMat1 = value; }
    }

    public float YieldSpeedMat2
    {
        get { return yieldSpeedMat2; }
        set { yieldSpeedMat2 = value; }
    }

    public float YieldSpeedMat3
    {
        get { return yieldSpeedMat3; }
        set { yieldSpeedMat3 = value; }
    }

    public float YieldSpeedMat4
    {
        get { return yieldSpeedMat4; }
        set { yieldSpeedMat4 = value; }
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
                curYieldTimeMat1 = 0;
                OnChangesYieldTimeMat1?.Invoke(curYieldTimeMat1);
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
                curYieldTimeMat2 = 0;
                OnChangesYieldTimeMat2?.Invoke(curYieldTimeMat2);
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
                curYieldTimeMat3 = 0;
                OnChangesYieldTimeMat3?.Invoke(curYieldTimeMat3);
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
                curYieldTimeMat4 = 0;
                OnChangesYieldTimeMat4?.Invoke(curYieldTimeMat4);
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
                curYieldTimeMat1 = 0;
                curYieldAmountMat1 = 0;
                OnChangesYieldAmountMat1?.Invoke(curYieldAmountMat1);
                OnChangesYieldTimeMat1?.Invoke(curYieldTimeMat1);
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
                curYieldTimeMat2 = 0;
                OnChangesYieldTimeMat2?.Invoke(curYieldTimeMat2);
                curYieldAmountMat2 = 0;
                OnChangesYieldAmountMat2?.Invoke(curYieldAmountMat2);
                break;

            case MaterialType.MaterialC:
                creature = InventoryManager.Instance.SelectedCreatureForMaterial_3;
                if (creature == null)
                {
                    return;
                }
                InventoryManager.Instance.RemoveCreatureFromMaterialSlot(materialType);
                StopCoroutine(materialC);
                curYieldTimeMat3 = 0;
                OnChangesYieldTimeMat3?.Invoke(curYieldTimeMat3);
                curYieldAmountMat3 = 0;
                OnChangesYieldAmountMat3?.Invoke(curYieldAmountMat3);
                break;

            case MaterialType.MaterialD:
                creature = InventoryManager.Instance.SelectedCreatureForMaterial_4;
                if (creature == null)
                {
                    return;
                }
                InventoryManager.Instance.RemoveCreatureFromMaterialSlot(materialType);
                curYieldTimeMat4 = 0;
                OnChangesYieldTimeMat4?.Invoke(curYieldTimeMat4);
                curYieldAmountMat4 = 0;
                OnChangesYieldAmountMat4?.Invoke(curYieldAmountMat4);
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
        curYieldAmountMat1 = CalculateYield(creature.CreatureStats.Attack, creature.CreatureStats.Speed);
        OnChangesYieldAmountMat1?.Invoke(curYieldAmountMat1);
        
        while (true)
        {
            if (curYieldTimeMat1 >= yieldSpeedMat1)
            {
                InventoryManager.Instance.AddMaterialToInventory(MaterialType.MaterialA, curYieldAmountMat1);
                curYieldTimeMat1 = 0;
            } 
            else
            {
                curYieldTimeMat1++;
            }
            OnChangesYieldTimeMat1?.Invoke(curYieldTimeMat1);
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator MaterialB(Creature creature)
    {
        curYieldAmountMat2 = CalculateYield(creature.MaxHealth, creature.CreatureStats.Speed);
        OnChangesYieldAmountMat2?.Invoke(curYieldAmountMat2);

        while (true)
        {
            if (curYieldTimeMat2 >= yieldSpeedMat2)
            {
                InventoryManager.Instance.AddMaterialToInventory(MaterialType.MaterialB, curYieldAmountMat2);
                curYieldTimeMat2 = 0;
            }
            else
            {
                curYieldTimeMat2++;
            }
            OnChangesYieldTimeMat2?.Invoke(curYieldTimeMat2);
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator MaterialC(Creature creature)
    {
        curYieldAmountMat3 = CalculateYield(creature.CreatureStats.Defense, creature.CreatureStats.Speed);
        OnChangesYieldAmountMat3?.Invoke(curYieldAmountMat3);

        while (true)
        {
            if (curYieldTimeMat3 >= yieldSpeedMat3)
            {
                InventoryManager.Instance.AddMaterialToInventory(MaterialType.MaterialC, curYieldAmountMat3);
                curYieldTimeMat3 = 0;
            }
            else
            {
                curYieldTimeMat3++;
            }
            OnChangesYieldTimeMat3?.Invoke(curYieldTimeMat3);
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator MaterialD(Creature creature)
    {
        curYieldAmountMat4 = CalculateYield(creature.CreatureStats.Dexterity, creature.CreatureStats.Speed);
        OnChangesYieldAmountMat4?.Invoke(curYieldAmountMat4);

        while (true)
        {
            if (curYieldTimeMat4 >= yieldSpeedMat4)
            {
                InventoryManager.Instance.AddMaterialToInventory(MaterialType.MaterialD, curYieldAmountMat4);
                curYieldTimeMat4 = 0;
            }
            else
            {
                curYieldTimeMat4++;
            }
            OnChangesYieldTimeMat4?.Invoke(curYieldTimeMat4);
            yield return new WaitForSeconds(1f);
        }
    }
}
