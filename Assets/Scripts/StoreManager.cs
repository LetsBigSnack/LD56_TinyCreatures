using System;
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
    [SerializeField] private int pricePerPowerLevel = 2;
    [SerializeField] private int playerMoney = 50;
    
    public int PlayerMoney { get => playerMoney; set => playerMoney = value; }
    
    
    public int CurrentSlotPrice { get => currentSlotPrice; set => currentSlotPrice = value; }
    public int BasicCreaturePrice { get => basicCreaturePrice; set => basicCreaturePrice = value; }
    public int AdvancedCreaturePrice { get => advancedCreaturePrice; set => advancedCreaturePrice = value; }
    
    
    

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

    
    //TODO: dont know if i should put in Update but i am a bit tired to maybe needs to change
    private void FixedUpdate()
    {
        UpdatePrices();
    }

    public void UpdatePrices()
    {
        advancedCreaturePrice = BattleManager.Instance.GetPredictedPowerLevel()  * pricePerPowerLevel;
        currentSlotPrice = pricesPerSlot + pricesPerSlot * boughtSlots;
    }

    public bool BuyNewSlot()
    {
        if (playerMoney < currentSlotPrice)
        {
            return false;
        }

        InventoryManager.Instance.AddSlot();
        SpendMoney(currentSlotPrice);
        boughtSlots++;
        return true;
    }
    
    public bool BuyBasicCreature()
    {
        if (playerMoney < basicCreaturePrice && InventoryManager.Instance.HasSpace())
        {
            return false;
        }

        if (InventoryManager.Instance.AddCreature(CreatureManager.Instance.CreateBasicCreature()
                .GetComponent<Creature>()))
        {
            SpendMoney(basicCreaturePrice);
            return true;
        }
        
        return false;

    }
    
    public bool BuyAdvancedCreature()
    {
        if (playerMoney < advancedCreaturePrice)
        {
            return false;
        }

        float statMin = BattleManager.Instance.StatMin;
        float statRange = BattleManager.Instance.StatRange;
        
        if (InventoryManager.Instance.AddCreature(CreatureManager.Instance.CreateAdjustedCreature(statRange, statMin).GetComponent<Creature>()))
        {
            SpendMoney(advancedCreaturePrice);
            return true;
        }
        
        return false;
        
    }

    public bool SellOwnedCreature(Creature creature)
    {
        EarnMoney(creature.PowerLevel);
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

    public bool CanBuyAdvancedCreature()
    {
        return BattleManager.Instance.PlayerWins >= winThreshold;
    }
}
