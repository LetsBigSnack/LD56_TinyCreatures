using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.UIElements;

public class UI_ShopManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI slotPriceText;
    [SerializeField] private  UnityEngine.UI.Button adjustedButton;
    [SerializeField] private TextMeshProUGUI adjustedPriceText;
    [SerializeField] private TextMeshProUGUI adjustedCreatureText;

    private string _defaultAdjustedText;


    private void Awake()
    {
        _defaultAdjustedText = adjustedCreatureText.text;
    }

    private void FixedUpdate()
    {
        UpdatePrices();
        UpdateUnlockables();
    }

    private void UpdatePrices()
    {
        slotPriceText.text = "Buy\n"+StoreManager.Instance.CurrentSlotPrice.ToString()+",-";
        adjustedPriceText.text = "Buy\n"+StoreManager.Instance.AdvancedCreaturePrice.ToString()+",-";
    }

    private void UpdateUnlockables()
    {
        if (StoreManager.Instance.CanBuyAdvancedCreature())
        {
            adjustedButton.interactable = true;
            adjustedCreatureText.text = _defaultAdjustedText;
        }
        else
        {
            adjustedButton.interactable = false;
            adjustedCreatureText.text = "Not able to buy advanced creature. Win at least 20 Battles";
            adjustedPriceText.text = "";
        }
    }

    public void BuySlot()
    {
        StoreManager.Instance.BuyNewSlot();
        UI_InventoryManager.Instance.RefreshInventory();
    }

    public void BuyBasicCreature()
    {
        StoreManager.Instance.BuyBasicCreature();
        UI_InventoryManager.Instance.RefreshInventory();
    }

    public void BuyAdjustedCreature()
    {
        StoreManager.Instance.BuyAdvancedCreature();
        UI_InventoryManager.Instance.RefreshInventory();
    }
    
    
}
