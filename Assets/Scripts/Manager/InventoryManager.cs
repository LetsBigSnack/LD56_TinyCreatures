using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    
    [Header("Inventory")]
    [SerializeField] private List<Creature> inventoryCreatures;
    [SerializeField] private int inventorySpace = 8;
    [SerializeField] private Creature selectedCreatureForBattle;

    [SerializeField] private Creature creatureInspectorLeft;
    [SerializeField] private Creature creatureInspectorRight;
    
    
    public Creature SelectedCreatureForBattle 
    { get => selectedCreatureForBattle; set => selectedCreatureForBattle = value; }
    
    
    public Creature CreatureInspectorLeft 
    { get => creatureInspectorLeft; set => creatureInspectorLeft = value; }

    public Creature CreatureInspectorRight 
    { get => creatureInspectorRight; set => creatureInspectorRight = value; }

    
    public List<Creature> InventoryCreatures
    {
        get { return inventoryCreatures; }
    }
    
    public int InventorySpace {get => inventorySpace;}
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            inventoryCreatures = new List<Creature>();
   
            AddCreature(CreatureManager.Instance.CreateBasicCreature());
            AddCreature(CreatureManager.Instance.CreateBasicCreature());
            AddCreature(CreatureManager.Instance.CreateBasicCreature());
        }
    }

    public bool AddCreature(Creature newCreature)
    {
        if (newCreature != null && !inventoryCreatures.Contains(newCreature))
        {
            if (inventoryCreatures.Count < inventorySpace)
            {
                inventoryCreatures.Add(newCreature);
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
    
    public void ChoiceCreatureForBattle(Creature creatureToChose)
    {
        if (creatureToChose != null && inventoryCreatures.Contains(creatureToChose))
        {
            RemoveCreatureFormBattle();
            selectedCreatureForBattle = creatureToChose;
            RemoveCreature(creatureToChose);
            BattleManager.Instance.StartBattle();
        }
    }
    
    public void RetreatFormBattle(Creature creatureToChose)
    {
        if (creatureToChose != null && selectedCreatureForBattle == creatureToChose)
        {
            RemoveCreatureFormBattle();
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
