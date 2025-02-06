using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance { get; private set; }

    [Header("Prices")] 
    private BigDecimal currentSlotPrice;
    private BigDecimal pricesPerSlot = 10;
    private BigDecimal boughtSlots = 0;
    private BigDecimal basicCreaturePrice = 10;
    private BigDecimal winThreshold = 10;
    private BigDecimal advancedCreaturePrice = 10;
    private BigDecimal pricePerPowerLevel = 2;
    private BigDecimal playerMoney = 70;
    private List<Creature> soledCreatures;
    [SerializeField] private int soldLimit;
    
    public BigDecimal PlayerMoney { get => playerMoney; set => playerMoney = value; }
    
    
    public BigDecimal CurrentSlotPrice { get => currentSlotPrice; set => currentSlotPrice = value; }
    public BigDecimal BasicCreaturePrice { get => basicCreaturePrice; set => basicCreaturePrice = value; }
    public BigDecimal AdvancedCreaturePrice { get => advancedCreaturePrice; set => advancedCreaturePrice = value; }
    
    public List<Creature> SoldCreatures { get => soledCreatures; set => soledCreatures = value; }
    
    
    public BigDecimal WinThreshold { get => winThreshold; set => winThreshold = value; }
    
    public BigDecimal BoughtSlots { get => boughtSlots; set => boughtSlots = value; }
    
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
    
    //TODO: Observer Pattern --> Battle Manager and Here and Battle Manager notifies Store when creates has been deafted
    private void FixedUpdate()
    {
        UpdatePrices();
    }

    public void UpdatePrices()
    {
        BigDecimal currentPrice = ((BattleManager.Instance.GetPredictedPowerLevel() + 5) * 2f);
        advancedCreaturePrice = currentPrice.Round(0);
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

        if (InventoryManager.Instance.AddCreature(CreatureManager.Instance.CreateBasicCreature()))
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

        BigDecimal statMin = BattleManager.Instance.StatMin;
        BigDecimal statRange = BattleManager.Instance.StatRange;
        
        // Battle Creature
        if (InventoryManager.Instance.AddCreature(CreatureManager.Instance.
                CreateAdjustedCreature(statRange + (BattleManager.Instance.PlayerWins * BattleManager.Instance.WinFactor), 
                    statMin + (BattleManager.Instance.PlayerWins * BattleManager.Instance.WinFactor * 2))))
        {
            SpendMoney(advancedCreaturePrice);
            return true;
        }
        
        return false;
        
    }

    public void SellOwnedCreature(Creature creature)
    {
        
        EarnMoney(creature.CreatureStats.PowerLevel);
        
        if (soledCreatures.Count+1 > soldLimit)
        {
            
            Creature soldCrt = soledCreatures.First();
            soledCreatures.Remove(soldCrt);
        }
        
        soledCreatures.Add(creature);
    }
    
    public bool RefuseCreature(Creature creature)
    {
        EarnMoney(creature.CreatureStats.PowerLevel);
        
        if (playerMoney >= creature.CreatureStats.PowerLevel )
        {
            SpendMoney(creature.CreatureStats.PowerLevel);
            return true;
        }
        return false;
    }

    public void SpendMoney(BigDecimal price)
    {
        playerMoney -= price;
    }
    public void EarnMoney(BigDecimal price)
    {
        playerMoney += price;
    }

    public bool CanBuyAdvancedCreature()
    {
        return BattleManager.Instance.PlayerWins >= winThreshold;
    }

    public bool BuyBack(Creature creature)
    {
        if (soledCreatures.Contains(creature) && playerMoney >= creature.CreatureStats.PowerLevel && InventoryManager.Instance.HasSpace())
        {
            SpendMoney(creature.CreatureStats.PowerLevel);
            InventoryManager.Instance.AddCreature(creature);
            soledCreatures.Remove(creature);
            return true;
        }
        return false;
    }
}
