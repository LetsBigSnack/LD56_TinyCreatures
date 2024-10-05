using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CompareManager : MonoBehaviour
{
    public static UI_CompareManager Instance;
    
    [SerializeField] private UI_CreatureSprite leftCreatureSprite;
    [SerializeField] private UI_CreatureSprite rightCreatureSprite;
    [SerializeField] private UI_CreatureDetailsText leftCreatureDetails;
    [SerializeField] private UI_CreatureDetailsText rightCreatureDetails;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetInspector()
    {
        leftCreatureSprite.Reset();
        rightCreatureSprite.Reset();
        
        Creature left = InventoryManager.Instance.CreatureInspectorLeft;
        Creature right = InventoryManager.Instance.CreatureInspectorRight;
        
        if (left != null)
        {
            leftCreatureSprite.SetupRepresentation(left);
            leftCreatureDetails.SetupRepresentation(left);
        }
        if (right != null)
        {
            rightCreatureSprite.SetupRepresentation(right);
            rightCreatureDetails.SetupRepresentation(right);
        }
        //Left / Button handle 
    }

    public void SetToBreedPod()
    {
        //add creature to the breedPod
    }

    public void SetAsActiveFightingCreature() {
        //add creature to the current fight
    }

    public void SellCreatureToShop()
    {
        //sell current picked creature
    }


    
}
