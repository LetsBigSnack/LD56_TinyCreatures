using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager Instance { get; private set; }

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
        System.Func<Creature, IEnumerator> farmingFn = null;
        switch (materialType)
        {
            case MaterialType.MaterialA:
                creature = InventoryManager.Instance.SelectedCreatureForMaterial_1;
                farmingFn = MaterialA;
                break;

            case MaterialType.MaterialB:
                creature = InventoryManager.Instance.SelectedCreatureForMaterial_1;
                farmingFn = MaterialB;
                break;

            case MaterialType.MaterialC:
                creature = InventoryManager.Instance.SelectedCreatureForMaterial_1;
                farmingFn = MaterialC;
                break;

            case MaterialType.MaterialD:
                creature = InventoryManager.Instance.SelectedCreatureForMaterial_1;
                farmingFn = MaterialD;
                break;
        }

        if(creature == null || farmingFn == null)
        {
            return;
        }

        StartCoroutine(farmingFn(creature));
    }

    public IEnumerator MaterialA(Creature creature)
    {
        yield return new WaitForSeconds(1);
    }

    public IEnumerator MaterialB(Creature creature)
    {
        yield return new WaitForSeconds(1);
    }

    public IEnumerator MaterialC(Creature creature)
    {
        yield return new WaitForSeconds(1);
    }

    public IEnumerator MaterialD(Creature creature)
    {
        yield return new WaitForSeconds(1);
    }
}
