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

    private Creature selectedCreatureForBattle;
    private Creature creatureInspectorLeft;
    private Creature creatureInspectorRight;
    private Creature selectedCreatureForReConfigure;

    private Creature selectedCreatureForMaterial_1;
    private Creature selectedCreatureForMaterial_2;
    private Creature selectedCreatureForMaterial_3;
    private Creature selectedCreatureForMaterial_4;

    public Creature SelectedCreatureForBattle 
    { get => selectedCreatureForBattle; set => selectedCreatureForBattle = value; }
    
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
        }
    }

    private void Update()
    {
        Debug.Log("A :" + materialA);
        Debug.Log("B :" + materialB);
        Debug.Log("C :" + materialC);
        Debug.Log("D :" + materialD);
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
    
    public void AddToBreed(Creature creature)
    {
        if (BreedingManager.Instance.AddToBreed(creature))
        {
            RemoveCreature(creature);
        };
        
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
    
    public void ChoiceCreatureForBattle(Creature creatureToChose)
    {
        if (creatureToChose != null && inventoryCreatures.Contains(creatureToChose))
        {
            RemoveCreatureFormBattle();
            selectedCreatureForBattle = creatureToChose;
            RemoveCreature(creatureToChose);
            BattleManager.Instance.NextBattle();
        }
    }
    
    public void RetreatFormBattle(Creature creatureToChose)
    {
        if (creatureToChose != null && selectedCreatureForBattle == creatureToChose)
        {
            RemoveCreatureFormBattle();
            if (UI_BattleManager.Instance != null) {
                UI_BattleManager.Instance.SetNextBattleButtonActive(false);
            }
        }
    }
    
    private void RemoveCreatureFormBattle()
    {
        if (selectedCreatureForBattle != null)
        {
            BattleManager.Instance.StopBattle();
            AddCreature(selectedCreatureForBattle);
            selectedCreatureForBattle = null;
        }
    }


    public void SelectCreatureLeft(Creature creatureToSelect)
    {
        if (creatureToSelect == creatureInspectorRight)
        {
            creatureInspectorRight = null;
        }
        creatureInspectorLeft = creatureToSelect;
    }
    
    public void SelectCreatureRight(Creature creatureToSelect)
    {
        if (creatureToSelect == creatureInspectorLeft)
        {
            creatureInspectorLeft = null;
        }
        creatureInspectorRight = creatureToSelect;
    }

    public void AddSlot()
    {
        inventorySpace++;
    }

    public bool HasSpace()
    {
        if(inventoryCreatures.Count < inventorySpace)
        {
            return true;
        }
        else
        {
            return false;
        }
        //throw new System.NotImplementedException();
    }
}
