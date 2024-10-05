using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance { get; private set; }

    [Header("Prices")] 
    [SerializeField] private int currentSlotPrice;
    [SerializeField] private int pricesPerSlot = 10;
    [SerializeField] private int boughtSlots = 0;
    [SerializeField] private int basicCreaturePrice = 10;
    //TODO: add threshold to buy shir
    [SerializeField] private int winThreshold = 20;
    [SerializeField] private int advancedCreaturePrice = 10;
    [SerializeField] private int pricePerPowerLevel = 5;
    [SerializeField] private int playerMoney = 50;
    
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            currentSlotPrice = pricesPerSlot;
            DontDestroyOnLoad(gameObject);
        }
    }



    public void UpdatePrices()
    {
        advancedCreaturePrice = BattleManager.Instance.GetPredictedPowerLevel()  * pricePerPowerLevel;
        currentSlotPrice = pricesPerSlot * pricesPerSlot;
    }

    public bool BuyNewSlot()
    {
        if (playerMoney < currentSlotPrice)
        {
            return false;
        }

        InventoryManager.Instance.AddSlot();
        SpendMoney(currentSlotPrice);
        return true;
    }
    
    public bool BuyBasicCreature()
    {
        if (playerMoney < basicCreaturePrice)
        {
            return false;
        }
        InventoryManager.Instance.AddCreature(CreatureManager.Instance.CreateBasicCreature().GetComponent<Creature>());
        SpendMoney(basicCreaturePrice);
        return true;

    }
    
    public bool BuyAdvancedCreature()
    {
        if (playerMoney < advancedCreaturePrice)
        {
            return false;
        }

        float statMin = BattleManager.Instance.StatMin;
        float statRange = BattleManager.Instance.StatRange;
        InventoryManager.Instance.AddCreature(CreatureManager.Instance.CreateAdjustedCreature(statRange, statMin).GetComponent<Creature>());
        SpendMoney(advancedCreaturePrice);
        return true;
        
    }
    public void SpendMoney(int price)
    {
        playerMoney -= price;
    }
    public void EarnMoney(int price)
    {
        playerMoney += price;
    }
}
