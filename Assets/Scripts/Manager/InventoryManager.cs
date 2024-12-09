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
