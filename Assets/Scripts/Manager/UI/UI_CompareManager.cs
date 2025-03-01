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


    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
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
                SoundManager.Instance.PlaySFX("Transaction");
            }
            else
            {
                SoundManager.Instance.PlaySFX("Error");
                UI_ToastManager.Instance.CreateToast(NotificationType.Alert, "No Creature", "Drag a Creature into the inspector to sell it!");
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
                SoundManager.Instance.PlaySFX("Transaction");
            }
            else
            {
                SoundManager.Instance.PlaySFX("Error");
                UI_ToastManager.Instance.CreateToast(NotificationType.Alert, "No Creature", "Drag a Creature into the inspector to sell it!");
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
                UI_BreedingManager.Instance.AddToLeftPod(left);
                InventoryManager.Instance.SelectCreatureLeft(null);

                Creature test = UI_BattleManager.Instance.SelectedCreature;

                if (test == left)
                {
                    UI_BattleManager.Instance.SelectedCreature = null;
                }
                
                UI_BattleManager.Instance.RefreshCreatureDetails();
                
                left = null;
                SoundManager.Instance.PlaySFX("Click");
            }
            else
            {
                SoundManager.Instance.PlaySFX("Error");
                UI_ToastManager.Instance.CreateToast(NotificationType.Alert, "No Creature", "Drag a Creature into the inspector to combine it!");
            }
        } 
        else
        {
            if (right != null)
            {
                UI_BreedingManager.Instance.AddToRightPod(right);
                InventoryManager.Instance.SelectCreatureRight(null);
                
                Creature test = UI_BattleManager.Instance.SelectedCreature;

                if (test == right)
                {
                    UI_BattleManager.Instance.SelectedCreature = null;
                }
                
                UI_BattleManager.Instance.RefreshCreatureDetails();
                
                right = null;
                SoundManager.Instance.PlaySFX("Click");
            }
            else
            {
                SoundManager.Instance.PlaySFX("Error");
                UI_ToastManager.Instance.CreateToast(NotificationType.Alert, "No Creature", "Drag a Creature into the inspector to combine it!");
            }
        }

        UI_InventoryManager.Instance.RefreshInventory();
        SetInspector();
    }


}
