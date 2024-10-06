using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private UI_CreatureDetailsText detailsLeft;
    [SerializeField] private UI_CreatureDetailsText detailsRight;
    [SerializeField] private TextMeshProUGUI costText;

    private void FixedUpdate()
    {
        creatureSpriteLeft.Reset();
        creatureSpriteRight.Reset();
        creatureSpriteMiddle.Reset();
        detailsLeft.Reset();
        detailsRight.Reset();
        
        creatureSpriteLeft.SetupRepresentation(BreedingManager.Instance.CreaturePod1);
        creatureSpriteRight.SetupRepresentation(BreedingManager.Instance.CreaturePod2);
        creatureSpriteMiddle.SetupRepresentation(BreedingManager.Instance.Result);
        
        
        detailsLeft.SetupRepresentation(BreedingManager.Instance.CreaturePod1);
        detailsRight.SetupRepresentation(BreedingManager.Instance.CreaturePod2);
        
        costText.text = BreedingManager.Instance.BreedingPrice.ToString();
    }

    public void AddCreatureToPod(Creature creature)
    {
        Debug.Log("Adding creature to Pod");
        InventoryManager.Instance.AddToBreed(creature);
    }

    public void RemoveCreatureFromPod(bool isLeft)
    {
        BreedingManager.Instance.RemoveToBreed(isLeft);
        UI_InventoryManager.Instance.RefreshInventory();
    }
    
    public void FuseCreature()
    {
        BreedingManager.Instance.Breed();
        CollectCreature();
    }

    public void CollectCreature()
    {
        BreedingManager.Instance.Collect();
        UI_InventoryManager.Instance.RefreshInventory();
    }
    
}
