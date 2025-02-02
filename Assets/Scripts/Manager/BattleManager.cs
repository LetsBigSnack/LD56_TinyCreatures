using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Helper.Util;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static Action<BigDecimal> OnCreatureWinsChanged;
    public static Action<BigDecimal> OnPlayerWinsChanged;
    
    public static Action<float, CreatureBattleSlot> OnCreatureHealthChanged;
    public static Action<float, CreatureBattleSlot> OnCreatureShieldChanged;
    public static Action<float, CreatureBattleSlot> OnCreatureTimeChanged;
    
    public static Action<float> OnEnemyHealthChanged;
    public static Action<float> OnEnemyTimeChanged;
    
    public static BattleManager Instance { get; private set; }

    [Header("Battle Parameters")] 
    private Creature enemyCreature;
    private BigDecimal statRange = new BigDecimal(35,-1);
    private BigDecimal statMin = new BigDecimal(8,0);
    //how this shit is displayed in the inspector
    private BigDecimal speedFactor = 60f;
    private BigDecimal winFactor = 0.25f;

    public BigDecimal StatRange{get{return statRange;}}
    public BigDecimal StatMin{get{return statMin;}}

    [Range(0, 3.0f)] [SerializeField] private float tickSpeedFactorHeal = 1;
    [Range(0, 2.0f)] [SerializeField] private float multFactorHeal = 1;
    
    [Range(0, 3.0f)] [SerializeField] private float tickSpeedFactorDefense = 1;
    [Range(0, 2.0f)] [SerializeField] private float multFactorDefense = 1;

    
    [Range(0, 4.0f)] [SerializeField] private float enemyScale = 4;
    
    [Header("Battle Information")]
    private BigDecimal playerWins = 0;
    [SerializeField] private float factorMult = 1.5f;
    private bool autoBattle;
    
    [SerializeField] private bool isBattleRunning;


    private Dictionary<CreatureBattleSlot, Coroutine> _creatureBattleCoroutines =
    new Dictionary<CreatureBattleSlot, Coroutine>()
    {
        { CreatureBattleSlot.Attack , null},
        { CreatureBattleSlot.Defense , null},
        { CreatureBattleSlot.Heal , null},
    };
    private Coroutine _enemyAttack;
    private Coroutine _battleCoroutine;
    
    public BigDecimal PlayerWins
    {
        get{return playerWins;}
        set => playerWins = value;
    }

    public BigDecimal WinFactor{ get{ return winFactor;} }
    
    public bool AutoBattle { get => autoBattle; set => autoBattle = value;  }
    
    public bool IsBattleRunning { get => isBattleRunning; set => isBattleRunning = value;  }

    
    public Creature EnemyCreature
    {
        get => enemyCreature;
        set => enemyCreature = value;
    }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            autoBattle = true;
        }
    }
    
    
    
    //Observer-Pattern
    
    private bool StartBattle()
    {
        Dictionary<CreatureBattleSlot, Creature> creatureBattleSlot = InventoryManager.Instance.CreatureBattleSlots;
        

        if (creatureBattleSlot[CreatureBattleSlot.Attack] == null)
        {
            return false;
        }
        
        if (UI_BattleManager.Instance != null && UI_InventoryHoverManager.Instance != null)
        {
            UI_InventoryHoverManager.Instance.ChangeBattleText("BATTLE ONGOING!");
            //UI_BattleManager.Instance.SetNextBattleButtonActive(false);
        }
        
        isBattleRunning = true;

        ResetCreatures(creatureBattleSlot);

        if (enemyCreature == null)
        {
            //TODO: adjust creature growth
            enemyCreature = CreatureManager.Instance.
                CreateAdjustedCreature(statRange + (playerWins * winFactor), 
                                        (statMin + (playerWins * winFactor))* enemyScale);
            enemyCreature.CreatureName = "Enemy";
        }
        else
        {
            enemyCreature.CurrentHealth = enemyCreature.MaxHealth;
        }
        
        BigDecimal percentage = enemyCreature.CurrentHealth / enemyCreature.MaxHealth;
        percentage = BigDecimal.Min(1, percentage);
        percentage = percentage.Round(3);
        
        OnEnemyHealthChanged?.Invoke((float)percentage);

        _battleCoroutine = StartCoroutine(BattleCoroutine());
        return true;
    }
    
    /// <summary>
    /// This Function resets all currently selected Creatures for Battle
    /// Following Values are going to be reset:
    /// - CurrentHealth --> going to be set to MaxHealth
    /// - CurrentShield --> going to be set to 0
    /// </summary>
    /// <param name="creatureBattleSlot">This is the Dictionary object which is passed from the InventoryManager which stores all selected Creatures for battle</param>
    private void ResetCreatures(Dictionary<CreatureBattleSlot, Creature> creatureBattleSlot)
    {
        foreach(KeyValuePair<CreatureBattleSlot, Creature> entry in creatureBattleSlot)
        {
            CreatureBattleSlot slot = entry.Key;
            Creature creature = entry.Value;
            
            if (creature != null)
            {
                creature.CurrentHealth = creature.MaxHealth;
                creature.CurrentShield = 0;
                
                
                BigDecimal percentageH = creature.CurrentHealth / creature.MaxHealth;
                percentageH = BigDecimal.Min(1, percentageH);
                percentageH = percentageH.Round(3);
                
                BigDecimal percentageS = creature.CurrentShield / creature.MaxHealth;
                percentageS = BigDecimal.Min(1, percentageS);
                percentageS = percentageS.Round(3);
                
                
                OnCreatureHealthChanged?.Invoke((float) percentageH, slot);
                OnCreatureShieldChanged?.Invoke((float) percentageS, slot);
            }
        }
    }

    public void WinBattle()
    {
        
        if (_enemyAttack != null)
        {
            Debug.LogError("Stopping Enemy Attack");
            StopCoroutine(_enemyAttack); 
        }
        
        StoreManager.Instance.EarnMoney(enemyCreature.CreatureStats.PowerLevel * 5);
        enemyCreature = null;
        playerWins++;

        AddCreatureWins();
        
        // Trigger to notify listeners in UI about the win count change - 
        //TODO: rework and enable again
        // OnCreatureWinsChanged?.Invoke(playerCreature.CreatureWins);
        
        //Achievements
        OnPlayerWinsChanged?.Invoke(playerWins);
        
        if (playerWins == StoreManager.Instance.WinThreshold)
        {
            winFactor = WinFactor * factorMult; 
        }

        if (_battleCoroutine != null)
        {
            StopCoroutine(_battleCoroutine);
        }

        if (autoBattle)
        {
            NextBattle();
        }
        else
        {
            if(!autoBattle && UI_BattleManager.Instance != null && UI_InventoryHoverManager.Instance != null)
            {
                UI_InventoryHoverManager.Instance.ChangeBattleText("READY TO BATTLE");
                UI_BattleManager.Instance.SetNextBattleButtonActive(true);
            }
        }
    }

    private void AddCreatureWins()
    {
        Dictionary<CreatureBattleSlot, Creature> creatureBattleSlot = InventoryManager.Instance.CreatureBattleSlots;

        foreach(KeyValuePair<CreatureBattleSlot, Creature> entry in creatureBattleSlot)
        {
            Creature creature = entry.Value;
            
            if (creature != null)
            {
                creature.CreatureWins++;
            }
        }
    }

    private void StartBattleCoroutines()
    {
        Dictionary<CreatureBattleSlot, Creature> creatureBattleSlot = InventoryManager.Instance.CreatureBattleSlots;

        foreach(KeyValuePair<CreatureBattleSlot, Creature> entry in creatureBattleSlot)
        {
            CreatureBattleSlot slot = entry.Key;
            Creature creature = entry.Value;
            
            if (creature != null)
            {
                switch (slot)
                {
                    case CreatureBattleSlot.Attack:
                        _creatureBattleCoroutines[CreatureBattleSlot.Attack] = StartCoroutine(CreatureAttackCycle(creature));
                        break;
                    case CreatureBattleSlot.Defense:
                        _creatureBattleCoroutines[CreatureBattleSlot.Defense] = StartCoroutine(CreatureDefenseCycle(creature));
                        break;
                    case CreatureBattleSlot.Heal:
                        _creatureBattleCoroutines[CreatureBattleSlot.Heal] = StartCoroutine(CreatureHealCycle(creature));
                        break;
                }
            }
        }
    }
    
    private void StopBattleCoroutines()
    {
        foreach (KeyValuePair<CreatureBattleSlot, Coroutine> entry in _creatureBattleCoroutines)
        {
            CreatureBattleSlot slot = entry.Key;
            Coroutine coroutine = entry.Value;

            if (coroutine != null)
            {
                Debug.Log("Coroutine for Slot "+slot+ " stopped");
                StopCoroutine(coroutine);
            }
        }
        //Dont want to .Remove() want to still have the Enum
        //Need to do that outside of the foreach otherwise changing while iteration
        _creatureBattleCoroutines[CreatureBattleSlot.Attack] = null;
        _creatureBattleCoroutines[CreatureBattleSlot.Defense] = null;
        _creatureBattleCoroutines[CreatureBattleSlot.Heal] = null;
    }

    
    private IEnumerator CreatureHealCycle(Creature healer)
    {
        BigDecimal healInterval = (speedFactor.Round(3) / healer.CreatureStats.Speed.Round(3)) * tickSpeedFactorHeal;
        BigDecimal healValue = healer.MaxHealth * multFactorHeal;
        BigDecimal elapsedTime = 0f;
        OnCreatureTimeChanged?.Invoke(0f, CreatureBattleSlot.Heal);
        while (isBattleRunning && healer != null && enemyCreature != null && healer.CurrentHealth > 0 && enemyCreature.CurrentHealth > 0)
        {
            
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= healInterval)
            {
                BigDecimal critChance = CalculateCritChance(healer.CreatureStats.Dexterity);
            
                bool isCriticalHit = UnityEngine.Random.value < critChance; // Random.value gives a value between 0 and 1
                BigDecimal attack = 0;
            
                if (isCriticalHit)
                {
                    List<Creature> creatures = GetCreatures();
                    foreach (Creature creature in creatures)
                    {
                        creature.ReceiveHeal(healValue);
                        CreatureBattleSlot slot = InventoryManager.Instance.CreatureBattleSlots.FirstOrDefault(x => x.Value == creature).Key;
                        
                        BigDecimal percentageH = creature.CurrentHealth / creature.MaxHealth;
                        percentageH = BigDecimal.Min(1, percentageH);
                        percentageH = percentageH.Round(3);
                        
                        OnCreatureHealthChanged?.Invoke((float)percentageH, slot);
                    
                    }
                    Debug.LogWarning("Healing creatures");
                }
                else
                { 
                    Creature creature = GetRandomCreature();
                    Debug.LogWarning("Healing "+ creature.CreatureName);
                    creature.ReceiveHeal(healValue);
                    CreatureBattleSlot slot = InventoryManager.Instance.CreatureBattleSlots.FirstOrDefault(x => x.Value == creature).Key;
                    
                    BigDecimal percentageH = creature.CurrentHealth / creature.MaxHealth;
                    percentageH = BigDecimal.Min(1, percentageH);
                    percentageH = percentageH.Round(3);
                        
                    OnCreatureHealthChanged?.Invoke((float)percentageH, slot);
                
                }

                if (UI_BattleDisplayManager.Instance != null)
                {
                    //TODO: create Heal PopUp
                }
                
                elapsedTime = 0f;
            }
            
            BigDecimal percentage = elapsedTime / healInterval;
            percentage = BigDecimal.Min(1, percentage);
            percentage = percentage.Round(3);
            OnCreatureTimeChanged?.Invoke((float)percentage, CreatureBattleSlot.Heal);
            yield return null;
        }
    }

    private List<Creature> GetCreatures()
    {
        List<Creature> creatures = new List<Creature>();

        foreach (KeyValuePair<CreatureBattleSlot, Creature> entry in InventoryManager.Instance.CreatureBattleSlots)
        {
            CreatureBattleSlot slot = entry.Key;
            Creature creature = entry.Value;

            if (creature != null)
            {
                creatures.Add(creature);
            }
        }
        return creatures;
    }

    private Creature GetRandomCreature()
    {
        List<Creature> creatures = GetCreatures();
        
        return creatures[UnityEngine.Random.Range(0, creatures.Count)];
    }

    private IEnumerator CreatureDefenseCycle(Creature defender)
    {
        BigDecimal defendInterval = (speedFactor.Round(3) / defender.CreatureStats.Speed.Round(3)) * tickSpeedFactorDefense;
        BigDecimal shieldValue = defender.CreatureStats.Defense.Round(3) * multFactorDefense;
        BigDecimal elapsedTime = 0f;
        OnCreatureTimeChanged?.Invoke(0f, CreatureBattleSlot.Defense);
        
        while (isBattleRunning && defender != null && enemyCreature != null && defender.CurrentHealth > 0 && enemyCreature.CurrentHealth > 0)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= defendInterval)
            {
                BigDecimal critChance = CalculateCritChance(defender.CreatureStats.Dexterity);
            
                bool isCriticalHit = UnityEngine.Random.value < critChance; // Random.value gives a value between 0 and 1
                BigDecimal attack = 0;
            
                if (isCriticalHit)
                {
                    List<Creature> creatures = GetCreatures();
                    foreach (Creature creature in creatures)
                    {
                        creature.ReceiveShield(shieldValue);
                        CreatureBattleSlot slot = InventoryManager.Instance.CreatureBattleSlots.FirstOrDefault(x => x.Value == creature).Key;
                        
                        BigDecimal percentageS = creature.CurrentHealth / creature.MaxHealth;
                        percentageS = BigDecimal.Min(1, percentageS);
                        percentageS = percentageS.Round(3);
                        
                        OnCreatureShieldChanged?.Invoke((float)percentageS, slot);

                    }
                    Debug.LogWarning("Defending all");
                }
                else
                { 
                    Creature creature = GetRandomCreature();
                    creature.ReceiveShield(shieldValue);
                    CreatureBattleSlot slot = InventoryManager.Instance.CreatureBattleSlots.FirstOrDefault(x => x.Value == creature).Key;
                    
                    BigDecimal percentageS = creature.CurrentHealth / creature.MaxHealth;
                    percentageS = BigDecimal.Min(1, percentageS);
                    percentageS = percentageS.Round(3);
                        
                    OnCreatureShieldChanged?.Invoke((float)percentageS, slot);

                    Debug.LogWarning("Defending " + creature.CreatureName);
                }

                if (UI_BattleDisplayManager.Instance != null)
                {
                    //TODO: create Shield PopUp
                }
                elapsedTime = 0f;
            }
            BigDecimal percentage = elapsedTime / defendInterval;
            percentage = BigDecimal.Min(1, percentage);
            percentage = percentage.Round(3);
            OnCreatureTimeChanged?.Invoke((float)percentage, CreatureBattleSlot.Defense);
            yield return null;
        }
    }

    private void RemoveCreature(Creature creature)
    {
        
    }
    
    private IEnumerator BattleCoroutine()
    {

        Dictionary<CreatureBattleSlot, Creature> creatureBattleSlot = InventoryManager.Instance.CreatureBattleSlots;

        StartBattleCoroutines();
        _enemyAttack = StartCoroutine(EnemyAttackCycle(enemyCreature));

        // Keep checking the health status of both creatures in a loop
        while (isBattleRunning && creatureBattleSlot[CreatureBattleSlot.Attack] != null && enemyCreature != null)
        {
            // If either creature has 0 health, stop the battle
            if (enemyCreature.CurrentHealth <= 0)
            {
                StopBattleCoroutines();
                WinBattle();
                
                yield break; //Stopped
            }
            
            
            CheckCreatureStatus();
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator EnemyAttackCycle(Creature enemy)
    {
        BigDecimal attackInterval = speedFactor.Round(3) / enemyCreature.CreatureStats.Speed.Round(3);
        //doesnt reliably work
        //attackInterval = BigDecimal.Max(3, attackInterval);
        BigDecimal attackDamage = enemyCreature.CreatureStats.Attack.Round(3);
        BigDecimal elapsedTime = 0f;
        OnEnemyTimeChanged?.Invoke(0);
        
         while (isBattleRunning && InventoryManager.Instance.CreatureBattleSlots[CreatureBattleSlot.Attack] != null && enemyCreature != null && InventoryManager.Instance.CreatureBattleSlots[CreatureBattleSlot.Attack].CurrentHealth > 0 && enemyCreature.CurrentHealth > 0)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= attackInterval)
            {
                BigDecimal critchance = CalculateCritChance(enemyCreature.CreatureStats.Dexterity);
            
                bool isCriticalHit = UnityEngine.Random.value < critchance; // Random.value gives a value between 0 and 1
            
                BigDecimal attack = 0;
            
                if (isCriticalHit)
                {
                    List<Creature> creatures = GetCreatures();
                    foreach (Creature creature in creatures)
                    {
                        creature.TakeDamage(attackDamage);
                        CreatureBattleSlot slot = InventoryManager.Instance.CreatureBattleSlots.FirstOrDefault(x => x.Value == creature).Key;
                        
                        BigDecimal percentageS = creature.CurrentShield / creature.MaxHealth;
                        percentageS = BigDecimal.Min(1, percentageS);
                        percentageS = percentageS.Round(3);
                        
                        BigDecimal percentageH = creature.CurrentHealth / creature.MaxHealth;
                        percentageH = BigDecimal.Min(1, percentageH);
                        percentageH = percentageH.Round(3);
                        
                        OnCreatureShieldChanged?.Invoke((float)percentageS, slot);
                        OnCreatureHealthChanged?.Invoke((float)percentageH, slot);
                    }
                    Debug.LogError("Enemy: Attacking all");
                }
                else
                { 
                    Creature creature = GetRandomCreature();
                    creature.TakeDamage(attackDamage);
                    CreatureBattleSlot slot = InventoryManager.Instance.CreatureBattleSlots.FirstOrDefault(x => x.Value == creature).Key;
                    BigDecimal percentageS = creature.CurrentShield / creature.MaxHealth;
                    percentageS = BigDecimal.Min(1, percentageS);
                    percentageS = percentageS.Round(3);
                        
                    BigDecimal percentageH = creature.CurrentHealth / creature.MaxHealth;
                    percentageH = BigDecimal.Min(1, percentageH);
                    percentageH = percentageH.Round(3);
                        
                    OnCreatureShieldChanged?.Invoke((float)percentageS, slot);
                    OnCreatureHealthChanged?.Invoke((float)percentageH, slot);
                    Debug.LogError("Enemy: Attacking "+ creature.CreatureName);
                }

                if (UI_BattleDisplayManager.Instance != null)
                {
                    //UI_BattleDisplayManager.Instance.CreateDamagePopUp(attack.ToNumberSuffix(false),isCriticalHit, enemyCreature);
                }
                elapsedTime = 0f;
            }
            BigDecimal percentage = elapsedTime / attackInterval;
            percentage = BigDecimal.Min(1, percentage);
            percentage = percentage.Round(3);
            OnEnemyTimeChanged?.Invoke((float)percentage);
            yield return null;
        }
    }

    // A coroutine for each creature to handle its attack cycle independently
    private IEnumerator CreatureAttackCycle(Creature attacker)
    {
        BigDecimal attackInterval = speedFactor.Round(3) / attacker.CreatureStats.Speed.Round(3);
        BigDecimal attackDamage = attacker.CreatureStats.Attack.Round(3);
        BigDecimal elapsedTime = 0f;
        OnCreatureTimeChanged?.Invoke(0f, CreatureBattleSlot.Attack);
        
        while (isBattleRunning && attacker != null && enemyCreature != null && attacker.CurrentHealth > 0 && enemyCreature.CurrentHealth > 0)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= attackInterval)
            {
                
                BigDecimal critchance = CalculateCritChance(attacker.CreatureStats.Dexterity);
            
                bool isCriticalHit = UnityEngine.Random.value < critchance; // Random.value gives a value between 0 and 1
            
                BigDecimal attack = 0;
            
                if (isCriticalHit)
                {
                    attackDamage *= 2.0f;
                    attack = enemyCreature.TakeDamage(attackDamage);
                    Debug.LogWarning("Crit Attack");
                }
                else
                { 
                    attack = enemyCreature.TakeDamage(attackDamage);
                    Debug.LogWarning("Normal Attack");
                }
                
                BigDecimal percentageH = enemyCreature.CurrentHealth / enemyCreature.MaxHealth;
                percentageH = BigDecimal.Min(1, percentageH);
                percentageH = percentageH.Round(3);
                
                OnEnemyHealthChanged?.Invoke((float)percentageH);

                if (UI_BattleDisplayManager.Instance != null)
                {
                    UI_BattleDisplayManager.Instance.CreateDamagePopUp(attack.ToNumberSuffix(false),isCriticalHit, attacker);
                }
                
                elapsedTime = 0f;
            }
            BigDecimal percentage = elapsedTime / attackInterval;
            percentage = BigDecimal.Min(1, percentage);
            percentage = percentage.Round(3);
            OnCreatureTimeChanged?.Invoke((float)percentage, CreatureBattleSlot.Attack);
            yield return null;
        }
    }

    private BigDecimal CalculateCritChance(BigDecimal x)
    {
        BigDecimal k = 0.1f;
        BigDecimal p = 0.04f;
        BigDecimal inside = (1.000 + k * x);
        BigDecimal insidePower = inside.Power(p);
        BigDecimal minusPart = (new BigDecimal(1000,-3) / insidePower);
        BigDecimal limiter = 0.75f;
        BigDecimal critChance = (1.000 - minusPart) * limiter;

        return critChance;
    }


    public void StopBattle()
    {
        isBattleRunning = false;
        
        if (UI_BattleManager.Instance != null)
        {
            UI_InventoryHoverManager.Instance.BattleText.text = "NO DATA FOUND!";
        }
        StopAllRoutines();
    }

    public void StopAllRoutines()
    {
        StopBattleCoroutines();
        if(_enemyAttack == null || _battleCoroutine == null)
            return;
        StopCoroutine(_enemyAttack);
        StopCoroutine(_battleCoroutine);
    }
    
    public void ResumeBattle()
    {
        isBattleRunning = true;
    }

    public void SwitchAutoBattle()
    {
        autoBattle = !autoBattle;
    }

    public bool NextBattle()
    {
        if (InventoryManager.Instance.CreatureBattleSlots[CreatureBattleSlot.Attack] == null || _creatureBattleCoroutines[CreatureBattleSlot.Attack] != null)
        {
            return false;
        }

        StopAllCoroutines();
        StartBattle();
        return true;
    }

    private void CheckCreatureStatus()
    {
        Dictionary<CreatureBattleSlot, Creature> creatureBattleSlots = InventoryManager.Instance.CreatureBattleSlots
            .ToDictionary(
                entry => entry.Key,
                entry => (Creature)entry.Value
            );
        
        foreach (KeyValuePair<CreatureBattleSlot, Creature> entry in creatureBattleSlots)
        {
            CreatureBattleSlot slot = entry.Key;
            Creature creature = entry.Value;
            
            if (creature != null && creature.CurrentHealth <= 0)
            {
                StopCoroutine(_creatureBattleCoroutines[slot]);
                InventoryManager.Instance.CreatureBattleSlots[slot] = null;
                
                if (slot == CreatureBattleSlot.Attack)
                {
                    isBattleRunning = false;
                
                    
                    UI_BattleManager.Instance.SelectedCreature = null;
                    UI_BattleManager.Instance.Refresh();
                
                    UI_InventoryHoverManager.Instance.BattleText.text = "Defeated!";
                    //POPUP
                
                    StopBattleCoroutines();
                
                    if (_enemyAttack != null)
                    {
                        Debug.LogError("Enemy Stopped");
                        StopCoroutine(_enemyAttack); 
                    }
                    return;
                }
            }
        }
    }

    public bool SetNextBattleButton()
    {
        var test = InventoryManager.Instance.CreatureBattleSlots;
        if(InventoryManager.Instance.CreatureBattleSlots[CreatureBattleSlot.Attack] == null  || enemyCreature != null)
        {
            return false;
        }
        if(UI_BattleManager.Instance != null)
        {
            UI_BattleManager.Instance.SetNextBattleButtonActive(true);
        }
        return true;
    }

    public BigDecimal GetPredictedPowerLevel()
    {
        // Adjust stat range and minimum based on player wins and win factor
        BigDecimal adjustedStatRange = statRange + (playerWins * winFactor);
        BigDecimal adjustedStatMin = statMin + (playerWins * winFactor * 2);

        // Calculate the average of the adjusted stat range for each individual stat
        BigDecimal averageMaxHealth = adjustedStatMin;
        BigDecimal averageSpeed = adjustedStatMin;
        BigDecimal averageAttack = adjustedStatMin;
        BigDecimal averageDefense = adjustedStatMin;
        BigDecimal averageDexterity = adjustedStatMin;

        // Define weights for each stat component
        float healthWeight = 0.10f;    // Weight for HP
        float speedWeight = 0.25f;      // Weight for speed
        float attackWeight = 0.25f;     // Weight for attack
        float defenseWeight = 0.2f;    // Weight for defense
        float dexterityWeight = 0.2f; // Weight for dexterity

        // Calculate the weighted average power level using the actual average stats
        BigDecimal averagePowerLevel = (averageMaxHealth * healthWeight) +
                                       (averageSpeed * speedWeight) +
                                       (averageAttack * attackWeight) +
                                       (averageDefense * defenseWeight) +
                                       (averageDexterity * dexterityWeight);
        
        return averagePowerLevel.Round(0);
    }

    public void RetreatCreature(CreatureBattleSlot creatureBattleSlot)
    {
        if (!isBattleRunning && InventoryManager.Instance.CreatureBattleSlots[creatureBattleSlot] != null)
        {
            InventoryManager.Instance.RetreatFormBattle(InventoryManager.Instance.CreatureBattleSlots[creatureBattleSlot], creatureBattleSlot);
        }
    }

    public void RetreatAll()
    {
        StopBattle();
        
        foreach (KeyValuePair<CreatureBattleSlot, Creature> entry in InventoryManager.Instance.CreatureBattleSlots)
        {
            CreatureBattleSlot slot = entry.Key;
            Creature creature = entry.Value;

            if (creature != null)
            {
                InventoryManager.Instance.RetreatFormBattle(creature, slot);
            }
        }
    }
}
