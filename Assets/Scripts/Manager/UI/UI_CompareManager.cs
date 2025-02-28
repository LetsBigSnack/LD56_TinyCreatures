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
    [SerializeField] private UICreatureButton leftCreatureButton;
    [SerializeField] private UI_CreatureSprite rightCreatureSprite;
    [SerializeField] private UICreatureButton rightCreatureButton;
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

    private void OnEnable()
    {
        SetInspector();
    }

    public void SetInspector()
    {
        leftCreatureButton.Creature = null;
        rightCreatureButton.Creature = null;
        leftCreatureSprite.Reset();
        rightCreatureSprite.Reset();
        leftCreatureDetails.Reset();
        rightCreatureDetails.Reset();
        
        Creature left = InventoryManager.Instance.CreatureInspectorLeft;
        Creature right = InventoryManager.Instance.CreatureInspectorRight;
        
        if (left != null)
        {
            leftCreatureButton.Creature = left;
            leftCreatureSprite.SetupRepresentation(left);
            leftCreatureDetails.SetupRepresentation(left);
            leftCreatureDetails.CompareColor(left, right);

        }
        if (right != null)
        {
            leftCreatureButton.Creature = right;
            rightCreatureSprite.SetupRepresentation(right);
            rightCreatureDetails.SetupRepresentation(right);
            rightCreatureDetails.CompareColor(right, left);
        }
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
                UI_BattleManager.Instance.RefreshCreatureDetails();
                
                InventoryManager.Instance.RemoveCreature(left);
                InventoryManager.Instance.SelectCreatureLeft(null);
                StoreManager.Instance.SellOwnedCreature(left);
                soundManager.PlaySFX("Transaction");
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
                UI_BattleManager.Instance.RefreshCreatureDetails();
                
                InventoryManager.Instance.RemoveCreature(right);
                InventoryManager.Instance.SelectCreatureRight(null);
                StoreManager.Instance.SellOwnedCreature(right);
                soundManager.PlaySFX("Transaction");
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
                
                UI_BattleManager.Instance.RefreshCreatureDetails();
                
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
                
                UI_BattleManager.Instance.RefreshCreatureDetails();
                
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
