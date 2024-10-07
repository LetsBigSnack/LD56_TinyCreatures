using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private int winThreshold = 10;
    [SerializeField] private int advancedCreaturePrice = 10;
    [SerializeField] private int pricePerPowerLevel = 2;
    [SerializeField] private int playerMoney = 50;
    
    [SerializeField] private List<Creature> soledCreatures;
    [SerializeField] private int soldLimit;
    
    public int PlayerMoney { get => playerMoney; set => playerMoney = value; }
    
    
    public int CurrentSlotPrice { get => currentSlotPrice; set => currentSlotPrice = value; }
    public int BasicCreaturePrice { get => basicCreaturePrice; set => basicCreaturePrice = value; }
    public int AdvancedCreaturePrice { get => advancedCreaturePrice; set => advancedCreaturePrice = value; }
    
    public List<Creature> SoldCreatures { get => soledCreatures; set => soledCreatures = value; }
    

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
            soledCreatures = new List<Creature>();
        }
    }

    
    //TODO: dont know if i should put in Update but i am a bit tired to maybe needs to change
    private void FixedUpdate()
    {
        UpdatePrices();
    }

    public void UpdatePrices()
    {
        advancedCreaturePrice = BattleManager.Instance.GetPredictedPowerLevel() + 5;
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
        if (playerMoney < basicCreaturePrice || !InventoryManager.Instance.HasSpace())
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
        
        Creature test = InventoryManager.Instance.SelectedCreatureForBattle;

        if (test == creature)
        {
            InventoryManager.Instance.SelectedCreatureForBattle = null;
            UI_BattleManager.Instance.Refresh();
        }
        
        EarnMoney(creature.PowerLevel);
        
        if (soledCreatures.Count+1 > soldLimit)
        {
            
            Creature soldCrt = soledCreatures.First();
            Destroy(soldCrt.gameObject);
            soledCreatures.Remove(soldCrt);
        }
        
        soledCreatures.Add(creature);   
        return true;
    }
    
    public bool RefuseCreature(Creature creature)
    {
        EarnMoney(creature.PowerLevel);
        
        if (playerMoney >= creature.PowerLevel )
        {
            SpendMoney(creature.PowerLevel);
            return true;
        }
        return false;
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

    public bool BuyBack(Creature creature)
    {
        if (soledCreatures.Contains(creature) && playerMoney >= creature.PowerLevel && InventoryManager.Instance.HasSpace())
        {
            InventoryManager.Instance.AddCreature(creature);
            soledCreatures.Remove(creature);
            return true;
        }
        return false;
    }
}
