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
    
    [SerializeField] private GameObject uiBuckBackPrefab;
    [SerializeField] private List<GameObject> buyBacks;
    [SerializeField] private Transform buyBacksParent;

    
    private string _defaultAdjustedText;

    private SoundManager soundManager;


    private void Awake()
    {
        _defaultAdjustedText = adjustedCreatureText.text;
        soundManager = FindObjectOfType<SoundManager>();
        buyBacks = new List<GameObject>();
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
            adjustedCreatureText.text = "Win 10 Battles to unlock \n " + BattleManager.Instance.PlayerWins + "/10";
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

    private void OnEnable()
    {
        RefreshInventory();
    }

    public void RefreshInventory()
    {
        
        foreach (GameObject uiCrt in buyBacks)
        {
            Destroy(uiCrt);
        }
        
        buyBacks = new List<GameObject>();
        if (StoreManager.Instance == null || StoreManager.Instance.SoldCreatures == null)
        {
            return;
        }
        foreach (Creature creature in StoreManager.Instance.SoldCreatures)
        {
            GameObject uiCreature = Instantiate(uiBuckBackPrefab, buyBacksParent.position, Quaternion.identity);
            uiCreature.transform.SetParent(buyBacksParent, false);
            BuyBackCreature uiCreatureButton = uiCreature.GetComponentInChildren<BuyBackCreature>();
            uiCreatureButton.Creature = creature;
            uiCreatureButton.Refresh();
            buyBacks.Add(uiCreature);
        }
    }
    
    
}
