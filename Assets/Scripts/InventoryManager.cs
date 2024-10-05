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
    private HashSet<Creature> _breedingCreatures;
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
            DontDestroyOnLoad(gameObject);
            inventoryCreatures = new List<Creature>();
            _breedingCreatures = new HashSet<Creature>();

            AddCreature(CreatureManager.Instance.CreateBasicCreature().GetComponent<Creature>());
            AddCreature(CreatureManager.Instance.CreateBasicCreature().GetComponent<Creature>());
            AddCreature(CreatureManager.Instance.CreateBasicCreature().GetComponent<Creature>());
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
        if (_breedingCreatures.Count > 2)
        {
            return;
        }
        
        _breedingCreatures.Add(creature);
        RemoveCreature(creature);
        
    }

    public void RemoveToBreed(Creature creature)
    {
        if (_breedingCreatures.Contains(creature))
        {
            _breedingCreatures.Remove(creature);
            AddCreature(creature);
            
        }
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
        throw new System.NotImplementedException();
    }
}
