using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory")]
    [SerializeField] private List<Creature> inventoryCreatures;
    [SerializeField] private int inventorySpace = 8;

    private BigDecimal materialA = 0;
    private BigDecimal materialB = 0;
    private BigDecimal materialC = 0;
    private BigDecimal materialD = 0;

    public static event Action<BigDecimal> OnChangesMaterialA;
    public static event Action<BigDecimal> OnChangesMaterialB;
    public static event Action<BigDecimal> OnChangesMaterialC;
    public static event Action<BigDecimal> OnChangesMaterialD;

    //private Creature selectedCreatureForBattle;
    private Dictionary<CreatureBattleSlot, Creature> creatureBattleSlots;

    public static event Action<CreatureBattleSlot> OnCreatureChanged;

    private Creature creatureInspectorLeft;
    private Creature creatureInspectorRight;

    public static event Action<Creature> OnChangesCreatureInspectorLeft;
    public static event Action<Creature> OnChangesCreatureInspectorRight;

    private Creature selectedCreatureForReConfigure;

    private Creature selectedCreatureForMaterial_1;
    private Creature selectedCreatureForMaterial_2;
    private Creature selectedCreatureForMaterial_3;
    private Creature selectedCreatureForMaterial_4;

    //public Creature SelectedCreatureForBattle { get => selectedCreatureForBattle; set => selectedCreatureForBattle = value; }
    public  Dictionary<CreatureBattleSlot, Creature> CreatureBattleSlots
    {
        get => creatureBattleSlots;
        set
        {
            Debug.Log("Changing creature battle slots");
            creatureBattleSlots = value;
        }
    }
    public Creature CreatureInspectorLeft 
    { get => creatureInspectorLeft; set => creatureInspectorLeft = value; }

    public Creature CreatureInspectorRight 
    { get => creatureInspectorRight; set => creatureInspectorRight = value; }

    public Creature SelectedCreatureForReConfigure
    { get => selectedCreatureForReConfigure; set => selectedCreatureForReConfigure = value; }

    public Creature SelectedCreatureForMaterial_1
    { get => selectedCreatureForMaterial_1; set => selectedCreatureForMaterial_1 = value; }
    public Creature SelectedCreatureForMaterial_2
    { get => selectedCreatureForMaterial_2; set => selectedCreatureForMaterial_2 = value; }
    public Creature SelectedCreatureForMaterial_3
    { get => selectedCreatureForMaterial_3; set => selectedCreatureForMaterial_3 = value; }
    public Creature SelectedCreatureForMaterial_4
    { get => selectedCreatureForMaterial_4; set => selectedCreatureForMaterial_4 = value; }

    public BigDecimal MaterialA
    { get => materialA; set => materialA = value; }
    public BigDecimal MaterialB
    { get => materialB; set => materialB = value; }
    public BigDecimal MaterialC
    { get => materialC; set => materialC = value; }
    public BigDecimal MaterialD
    { get => materialD; set => materialD = value; }

    public List<Creature> InventoryCreatures
    {
        get { return inventoryCreatures; }
        set => inventoryCreatures = value;
    }

    public int InventorySpace
    {
        get => inventorySpace;
        set => inventorySpace = value;
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
            creatureBattleSlots = new Dictionary<CreatureBattleSlot, Creature>
            {
                { CreatureBattleSlot.Attack, null },
                { CreatureBattleSlot.Defense, null },
                { CreatureBattleSlot.Heal, null }
            };
        }
    }

    public Creature IsCreatureSlotEmpty(CreatureBattleSlot type)
    {
        if (type == CreatureBattleSlot.Enemy) return null;
        return creatureBattleSlots[type];
    }

    public bool AddCreature(Creature newCreature)
    {
        if (newCreature != null && !inventoryCreatures.Contains(newCreature))
        {
            if (inventoryCreatures.Count < inventorySpace)
            {
                inventoryCreatures.Add(newCreature);
                CreatureManager.Instance.CheckCollectedParts(newCreature);
                UI_InventoryManager.Instance.RefreshInventory();
                return true;
            }
        }
        return false;
    }

    public void AddMaterialToInventory(MaterialType material, BigDecimal amount)
    {
        switch (material)
        {
            case MaterialType.MaterialA:
                materialA += amount;
                OnChangesMaterialA?.Invoke(materialA);
                break;

            case MaterialType.MaterialB:
                materialB += amount;
                OnChangesMaterialB?.Invoke(materialB);
                break;

            case MaterialType.MaterialC:
                materialC += amount;
                OnChangesMaterialC?.Invoke(materialC);
                break;

            case MaterialType.MaterialD:
                materialD += amount;
                OnChangesMaterialD?.Invoke(materialD);
                break;
        }
    }

    public bool ReduceMaterialToInventory(MaterialType material, BigDecimal amount)
    {
        switch (material)
        {
            case MaterialType.MaterialA:
                if((materialA - amount) < 0)
                {
                    return false;
                }
                materialA -= amount;
                OnChangesMaterialA?.Invoke(materialA);
                break;

            case MaterialType.MaterialB:
                if ((materialB - amount) < 0)
                {
                    return false;
                }
                materialB -= amount;
                OnChangesMaterialB?.Invoke(materialB);
                break;

            case MaterialType.MaterialC:
                if ((materialC - amount) < 0)
                {
                    return false;
                }
                materialC -= amount;
                OnChangesMaterialC?.Invoke(materialC);
                break;

            case MaterialType.MaterialD:
                if ((materialD - amount) < 0)
                {
                    return false;
                }
                materialD -= amount;
                OnChangesMaterialD?.Invoke(materialD);
                break;
        }

        return true;
    }

    public bool AddCreatureToMaterialSlot(Creature creature, MaterialType selectedMaterial)
    {
        if (creature != null && inventoryCreatures.Contains(creature))
        {
            switch (selectedMaterial)
            {
                case MaterialType.MaterialA:
                    RemoveCreatureFromMaterialSlot(selectedMaterial);
                    selectedCreatureForMaterial_1 = creature;
                    break;

                case MaterialType.MaterialB:
                    RemoveCreatureFromMaterialSlot(selectedMaterial);
                    selectedCreatureForMaterial_2 = creature;
                    break;

                case MaterialType.MaterialC:
                    RemoveCreatureFromMaterialSlot(selectedMaterial);
                    selectedCreatureForMaterial_3 = creature;
                    break;

                case MaterialType.MaterialD:
                    RemoveCreatureFromMaterialSlot(selectedMaterial);
                    selectedCreatureForMaterial_4 = creature;
                    break;
            }
            inventoryCreatures.Remove(creature);
            UI_InventoryManager.Instance.RefreshInventory();
            return true;
        }
        return false;
    }

    public void RemoveCreatureFromMaterialSlot(MaterialType materialType)
    {
        switch (materialType)
        {
            case MaterialType.MaterialA:
                RemoveCreatureFromMaterialSlot(selectedCreatureForMaterial_1, materialType);
                break;

            case MaterialType.MaterialB:
                RemoveCreatureFromMaterialSlot(selectedCreatureForMaterial_2, materialType);
                break;

            case MaterialType.MaterialC:
                RemoveCreatureFromMaterialSlot(selectedCreatureForMaterial_3, materialType);
                break;

            case MaterialType.MaterialD:
                RemoveCreatureFromMaterialSlot(selectedCreatureForMaterial_4, materialType);
                break;
        }
    }

    public void RemoveCreatureFromMaterialSlot(Creature creature, MaterialType materialType)
    {
        if (creature == null)
        {
            return;
        }

        switch (materialType)
        {
            case MaterialType.MaterialA:
                if(selectedCreatureForMaterial_1 != creature)
                {
                    return;
                }
                selectedCreatureForMaterial_1 = null;
                break;

            case MaterialType.MaterialB:
                if (selectedCreatureForMaterial_2 != creature)
                {
                    return;
                }
                selectedCreatureForMaterial_2 = null;
                break;

            case MaterialType.MaterialC:
                if (selectedCreatureForMaterial_3 != creature)
                {
                    return;
                }
                selectedCreatureForMaterial_3 = null;
                break;

            case MaterialType.MaterialD:
                if (selectedCreatureForMaterial_4 != creature)
                {
                    return;
                }
                selectedCreatureForMaterial_4 = null;
                break;
        }
        AddCreature(creature);
    }

    public bool RemoveCreature(Creature creatureToRemove)
    {
        if (creatureToRemove != null && inventoryCreatures.Contains(creatureToRemove))
        {
            inventoryCreatures.Remove(creatureToRemove);
            UI_InventoryManager.Instance.RefreshInventory();
            return true;
        }
        return false;
    }

    public bool AddToBreedLeft(Creature creature)
    {
        if(BreedingManager.Instance.AddToBreedLeft(creature)){
            RemoveCreature(creature);
            RemoveFromCompareManager(creature);
            return true;
        };

        AddCreature(BreedingManager.Instance.CreaturePod1);
        BreedingManager.Instance.RemoveFromBreed(true);
        BreedingManager.Instance.AddToBreedLeft(creature);
        RemoveCreature(creature);
        RemoveFromCompareManager(creature);
        return true;
    }

    public bool AddToBreedRight(Creature creature)
    {
        if (BreedingManager.Instance.AddToBreedRight(creature))
        {
            RemoveCreature(creature);
            RemoveFromCompareManager(creature);
            return true;
        };

        AddCreature(BreedingManager.Instance.CreaturePod2);
        BreedingManager.Instance.RemoveFromBreed(false);
        BreedingManager.Instance.AddToBreedRight(creature);
        RemoveCreature(creature);
        RemoveFromCompareManager(creature);
        return true;
    }

    private void RemoveFromCompareManager(Creature creature)
    {
        if (creatureInspectorLeft == creature)
        {
            creatureInspectorLeft = null;
            OnChangesCreatureInspectorLeft?.Invoke(creatureInspectorLeft);
            UI_CompareManager.Instance.SetInspector();
            return;
        }

        if(creatureInspectorRight == creature)
        {
            creatureInspectorRight = null;
            OnChangesCreatureInspectorRight?.Invoke(creatureInspectorRight);
            UI_CompareManager.Instance.SetInspector();
            return;
        }
    }

    public void AddToReconfigure(Creature creature)
    {
        if (UI_BattleManager.Instance.SelectedCreature != creature && selectedCreatureForReConfigure == null)
        {
            if (ReconfigureManager.Instance.AddToReconfigure(creature))
            {
                RemoveCreature(creature);
                selectedCreatureForReConfigure = creature;
            }
        }
    }

    public void RemoveFromReconfigure(Creature creature)
    {
        if(selectedCreatureForReConfigure != null)
        {
            AddCreature(creature);
            selectedCreatureForReConfigure = null;
        }
    }
    
    //previously ChoiceCreatureForBattle
    public void SelectCreatureForBattle(Creature creature, CreatureBattleSlot battleSlot)
    {
        if (creature != null && inventoryCreatures.Contains(creature))
        {
            
            if (creature == creatureInspectorLeft)
            {
                creatureInspectorLeft = null;
                OnChangesCreatureInspectorLeft?.Invoke(creatureInspectorLeft);
            }
            
            if (creature == creatureInspectorRight)
            {
                creatureInspectorRight = null;
                OnChangesCreatureInspectorRight?.Invoke(creatureInspectorRight);
            }
            
            UI_CompareManager.Instance?.SetInspector();
            
            creatureBattleSlots[battleSlot] = creature;
            OnCreatureChanged?.Invoke(battleSlot);
            
            UI_BattleManager.Instance.UpdateBattleCreatureRepresentation(battleSlot, creature);

            RemoveCreature(creature);
            
        }
    }
    
    public bool RetreatFormBattle(Creature creature, CreatureBattleSlot battleSlot)
    {
        if (creature == null || creatureBattleSlots[battleSlot] != creature || !HasSpace())
        {
            return false;
        }
       
        creatureBattleSlots[battleSlot] = null;
        OnCreatureChanged?.Invoke(battleSlot);
        AddCreature(creature);
        return true;
    }
    
    public void SelectCreatureLeft(Creature creatureToSelect)
    {
        if (creatureToSelect == creatureInspectorRight)
        {
            creatureInspectorRight = null;
            OnChangesCreatureInspectorRight?.Invoke(creatureInspectorRight);
        }
        creatureInspectorLeft = creatureToSelect;
        OnChangesCreatureInspectorLeft?.Invoke(creatureInspectorLeft);
    }
    
    public void SelectCreatureRight(Creature creatureToSelect)
    {
        if (creatureToSelect == creatureInspectorLeft)
        {
            creatureInspectorLeft = null;
            OnChangesCreatureInspectorLeft?.Invoke(creatureInspectorLeft);
        }
        creatureInspectorRight = creatureToSelect;
        OnChangesCreatureInspectorRight?.Invoke(creatureInspectorRight);
    }

    public void AddSlot()
    {
        inventorySpace++;
    }

    public bool HasSpace(int amount = 1)
    {
        if(inventoryCreatures.Count + amount <= inventorySpace)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
