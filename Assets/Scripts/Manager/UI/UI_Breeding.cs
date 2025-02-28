using System;
using System.Collections;
using System.Collections.Generic;
using Helper.Util;
using TMPro;
using UnityEngine;

public class UI_BreedingManager : MonoBehaviour
{ 
    static public UI_BreedingManager Instance;
    [SerializeField] private UI_CreatureSprite creatureSpriteLeft;
    [SerializeField] private UI_CreatureSprite creatureSpriteRight;
    [SerializeField] private UI_CreatureSprite creatureSpriteMiddle;
    [SerializeField] private UI_CreatureDetailsText detailsLeft;
    [SerializeField] private UI_CreatureDetailsText detailsRight;
    [SerializeField] private TextMeshProUGUI costText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    //TODO: Observer Pattern
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
        
        costText.text = BreedingManager.Instance.BreedingPrice.ToNumberSuffix(false);
    }

    public void AddCreatureToPod(Creature creature)
    {
        InventoryManager.Instance.AddToBreed(creature);
    }

    public void RemoveCreatureFromPod(bool isLeft)
    {
        if (BreedingManager.Instance.RemoveToBreed(isLeft))
        {
            SoundManager.Instance.PlaySFX("Click");
        }
        else
        {
            SoundManager.Instance.PlaySFX("Error");
        }
        UI_InventoryManager.Instance.RefreshInventory();
    }
    
    public void FuseCreature()
    {
        if (BreedingManager.Instance.Result != null)
        {
            SoundManager.Instance.PlaySFX("Error");
            return;
        }
        if (BreedingManager.Instance.Breed())
        {
            SoundManager.Instance.PlaySFX("Transaction");
            UI_ToggleManager.Instance.SwitchState("Fusion");
        }
        else
        {
            SoundManager.Instance.PlaySFX("Error");
        }
    }
    
}
