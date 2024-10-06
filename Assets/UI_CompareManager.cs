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
        leftCreatureDetails.Reset();
        rightCreatureDetails.Reset();
        
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

    public void SellCreatureToShop(bool isLeft)
    {
        Creature left = InventoryManager.Instance.CreatureInspectorLeft;
        Creature right = InventoryManager.Instance.CreatureInspectorRight;

        if (isLeft)
        {
            if (left != null)
            {
                StoreManager.Instance.SellOwnedCreature(left);
                InventoryManager.Instance.RemoveCreature(left);
                InventoryManager.Instance.SelectCreatureLeft(null);
            }
        } 
        else
        {
            if (right != null)
            {
                StoreManager.Instance.SellOwnedCreature(right);
                InventoryManager.Instance.RemoveCreature(right);
                InventoryManager.Instance.SelectCreatureRight(null);
            }
        }

        UI_InventoryManager.Instance.RefreshInventory();
        SetInspector();
    }
    
    public void AddToBread(bool isLeft)
    {
        Creature left = InventoryManager.Instance.CreatureInspectorLeft;
        Creature right = InventoryManager.Instance.CreatureInspectorRight;

        if (isLeft)
        {
            if (left != null)
            {
                UI_BreedingManager.Instance.AddCreatureToPod(left);
                InventoryManager.Instance.SelectCreatureLeft(null);
                left = null;
            }
        } 
        else
        {
            if (right != null)
            {
                UI_BreedingManager.Instance.AddCreatureToPod(right);
                InventoryManager.Instance.SelectCreatureRight(null);
                right = null;
            }
        }

        UI_InventoryManager.Instance.RefreshInventory();
        SetInspector();
    }


    
}
