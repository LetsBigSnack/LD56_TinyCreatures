using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public List<Creature> inventoryCreatures { get; private set; }

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
        }
    }

    public void AddCreature(Creature newCreature)
    {
        if (newCreature != null && !inventoryCreatures.Contains(newCreature))
        {
            inventoryCreatures.Add(newCreature);
        }
    }

    public void RemoveCreature(Creature creatureToRemove)
    {
        if (creatureToRemove != null && inventoryCreatures.Contains(creatureToRemove))
        {
            inventoryCreatures.Remove(creatureToRemove);
        }
    }

}
