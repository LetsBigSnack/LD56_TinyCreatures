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

    private SoundManager soundManager;


    private void Awake()
    {
        _defaultAdjustedText = adjustedCreatureText.text;
        soundManager = FindObjectOfType<SoundManager>();
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
        if (StoreManager.Instance.BuyNewSlot())
        {
            soundManager.PlaySFX("Transaction");
        }
        else
        {
            soundManager.PlaySFX("Error");
        }
        UI_InventoryManager.Instance.RefreshInventory();
    }

    public void BuyBasicCreature()
    {
        if (StoreManager.Instance.BuyBasicCreature())
        {
            soundManager.PlaySFX("Transaction");
        }
        else
        {
            soundManager.PlaySFX("Error");
            Debug.Log("No money");
        }
        UI_InventoryManager.Instance.RefreshInventory();
    }

    public void BuyAdjustedCreature()
    {
        if (StoreManager.Instance.BuyAdvancedCreature())
        {
            soundManager.PlaySFX("Transaction");
        }
        else
        {
            soundManager.PlaySFX("Error");
        }
        UI_InventoryManager.Instance.RefreshInventory();
    }
    
    
}
