using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager Instance { get; private set; }

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

    public IEnumerator MaterialA(Creature creature)
    {
        float amountToGenerate = (float)creature.CreatureStats.Attack / 60f;
        float timeUntilNextGeneration = 60f / (amountToGenerate + (float)creature.CreatureStats.Speed);
        while (true)
        {
            yield return new WaitForSeconds(timeUntilNextGeneration);
            InventoryManager.Instance.AddMaterialToInventory(MaterialType.MaterialA, 1);
        }
    }

    public IEnumerator MaterialB(Creature creature)
    {
        float amountToGenerate = (float)creature.CreatureStats.Dexterity / 60f;
        float timeUntilNextGeneration = 60f / (amountToGenerate + (float)creature.CreatureStats.Speed);
        while (true)
        {
            yield return new WaitForSeconds(timeUntilNextGeneration);
            InventoryManager.Instance.AddMaterialToInventory(MaterialType.MaterialB, 1);
        }
    }

    public IEnumerator MaterialC(Creature creature)
    {
        float amountToGenerate = (float)creature.CreatureStats.Defense / 60f;
        float timeUntilNextGeneration = 60f / (amountToGenerate + (float)creature.CreatureStats.Speed);
        while (true)
        {
            yield return new WaitForSeconds(timeUntilNextGeneration);
            InventoryManager.Instance.AddMaterialToInventory(MaterialType.MaterialC, 1);
        }
    }

    public IEnumerator MaterialD(Creature creature)
    {
        float amountToGenerate = (float)creature.MaxHealth / 60f;
        float timeUntilNextGeneration = 60f / (amountToGenerate + (float)creature.CreatureStats.Speed);
        while (true)
        {
            yield return new WaitForSeconds(timeUntilNextGeneration);
            InventoryManager.Instance.AddMaterialToInventory(MaterialType.MaterialD, 1);
        }
    }
}
