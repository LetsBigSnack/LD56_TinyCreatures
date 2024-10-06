using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BreedingManager : MonoBehaviour
{
    
    static public UI_BreedingManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    


    [SerializeField] private UI_CreatureSprite creatureSpriteLeft;
    [SerializeField] private UI_CreatureSprite creatureSpriteRight;
    [SerializeField] private UI_CreatureSprite creatureSpriteMiddle;


    private void FixedUpdate()
    {
        creatureSpriteLeft.Reset();
        creatureSpriteRight.Reset();
        creatureSpriteMiddle.Reset();
        
        creatureSpriteLeft.SetupRepresentation(BreedingManager.Instance.CreaturePod1);
        creatureSpriteRight.SetupRepresentation(BreedingManager.Instance.CreaturePod2);
        creatureSpriteMiddle.SetupRepresentation(BreedingManager.Instance.Result);
    }

    public void AddCreatureToPod(Creature creature)
    {
        Debug.Log("Adding creature to Pod");
        InventoryManager.Instance.AddToBreed(creature);
    }

    public void FuseCreature()
    {
        BreedingManager.Instance.Breed();
    }
    
}
