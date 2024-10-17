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

    private SoundManager soundManager;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            soundManager = FindObjectOfType<SoundManager>();
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
            leftCreatureDetails.CompareColor(left, right);

        }
        if (right != null)
        {
            rightCreatureSprite.SetupRepresentation(right);
            rightCreatureDetails.SetupRepresentation(right);
            rightCreatureDetails.CompareColor(right, left);
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
    
    
    //TODO: check if all the logic is needed
    public void SellCreatureToShop(bool isLeft)
    {
        Creature left = InventoryManager.Instance.CreatureInspectorLeft;
        Creature right = InventoryManager.Instance.CreatureInspectorRight;

        if (isLeft)
        {
            if (left != null)
            {
                Creature test = UI_BattleManager.Instance.SelectedCreature;

                if (test == left)
                {
                    UI_BattleManager.Instance.SelectedCreature = null;
                }
                UI_BattleManager.Instance.Refresh();
                
                InventoryManager.Instance.RemoveCreature(left);
                InventoryManager.Instance.SelectCreatureLeft(null);
                StoreManager.Instance.SellOwnedCreature(left);
                soundManager.PlaySFX("Click");
            }
            else
            {
                soundManager.PlaySFX("Error");
            }
        } 
        else
        {
            if (right != null)
            {
                Creature test = UI_BattleManager.Instance.SelectedCreature;

                if (test == right)
                {
                    UI_BattleManager.Instance.SelectedCreature = null;
                }
                UI_BattleManager.Instance.Refresh();
                
                InventoryManager.Instance.RemoveCreature(right);
                InventoryManager.Instance.SelectCreatureRight(null);
                StoreManager.Instance.SellOwnedCreature(right);
                soundManager.PlaySFX("Click");
            }
            else
            {
                soundManager.PlaySFX("Error");
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

                Creature test = UI_BattleManager.Instance.SelectedCreature;

                if (test == left)
                {
                    UI_BattleManager.Instance.SelectedCreature = null;
                }
                
                UI_BattleManager.Instance.Refresh();
                
                left = null;
                soundManager.PlaySFX("Click");
            }
            else
            {
                soundManager.PlaySFX("Error");
            }
        } 
        else
        {
            if (right != null)
            {
                UI_BreedingManager.Instance.AddCreatureToPod(right);
                InventoryManager.Instance.SelectCreatureRight(null);
                
                Creature test = UI_BattleManager.Instance.SelectedCreature;

                if (test == right)
                {
                    UI_BattleManager.Instance.SelectedCreature = null;
                }
                
                UI_BattleManager.Instance.Refresh();
                
                right = null;
                soundManager.PlaySFX("Click");
            }
            else
            {
                soundManager.PlaySFX("Error");
            }
        }

        UI_InventoryManager.Instance.RefreshInventory();
        SetInspector();
    }


    
}
