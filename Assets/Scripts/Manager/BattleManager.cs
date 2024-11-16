using System;
using System.Collections;
using Data;
using Helper.Util;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
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

    [Header("Battle Information")] 
    [SerializeField] private bool battleRunning = true;
    [SerializeField] private bool hasBattleStarted = false;
    private BigDecimal playerWins = 0;
    [SerializeField] private float factorMult = 1.5f;
    private bool autoBattle;

    public bool HasBattleStarted
    {
        get => hasBattleStarted;
        set => hasBattleStarted = value;
    }

    private Coroutine _enemyAttack;
    private Coroutine _playerAttack;
    private Coroutine _battleCoroutine;
    
    public BigDecimal PlayerWins
    {
        get{return playerWins;}
        set => playerWins = value;
    }

    public BigDecimal WinFactor{ get{ return winFactor;} }
    
    public bool AutoBattle { get { return autoBattle;} }
    
    public Creature EnemyCreature
    {
        get => enemyCreature;
        set => enemyCreature = value;
    }

    public bool BattleRunning { get => battleRunning; set => battleRunning = value; }
    
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

    private void Update()
    {
        if (!battleRunning || GameManager.Instance.CurrentState != State.Game)
        {
            return;
        }
        if (InventoryManager.Instance.SelectedCreatureForBattle != null && !hasBattleStarted)
        {
            if(autoBattle) 
            {
                StartBattle();
            }
        }
        Debug.Log("AutoBattle = " + autoBattle);
    }

    private void StartBattle()
    {
        Debug.Log("StartBattle");
        Creature playerCreature = InventoryManager.Instance.SelectedCreatureForBattle;
        if (UI_BattleManager.Instance != null && UI_InventoryHoverManager.Instance != null)
        {
            UI_InventoryHoverManager.Instance.ChangeBattleText("BATTLE ONGOING!");
            UI_BattleManager.Instance.SetNextBattleButtonActive(false);
        }
        hasBattleStarted = true;
        playerCreature.CurrentHealth = playerCreature.MaxHealth;

        if (enemyCreature == null)
        {
            Debug.Log("Create Creature");
            enemyCreature = CreatureManager.Instance.
                CreateAdjustedCreature(statRange + (playerWins * winFactor), 
                                        statMin + (playerWins * winFactor * 2));
            enemyCreature.CreatureName = "Enemy";
        }
        else
        {
            enemyCreature.CurrentHealth = enemyCreature.MaxHealth;
        }

        _battleCoroutine = StartCoroutine(BattleCoroutine());
    }

    public void WinBattle()
    {

        StoreManager.Instance.EarnMoney(enemyCreature.CreatureStats.PowerLevel * 5);
        enemyCreature = null;
        playerWins++;
        hasBattleStarted = false;

        InventoryManager.Instance.SelectedCreatureForBattle.CreatureWins++;
        
        if (playerWins == StoreManager.Instance.WinThreshold)
        {
            winFactor = WinFactor * factorMult; 
        }

        if (_battleCoroutine != null)
        {
            StopCoroutine(_battleCoroutine);
        }

        if(UI_BattleManager.Instance != null && UI_InventoryHoverManager.Instance != null)
        {
            UI_InventoryHoverManager.Instance.ChangeBattleText("READY TO BATTLE");
            UI_BattleManager.Instance.SetNextBattleButtonActive(true);
        }
    }

    private IEnumerator BattleCoroutine()
    {
        Creature playerCreature = InventoryManager.Instance.SelectedCreatureForBattle;

        yield return new WaitForSeconds(1);

        // Start both creatures attacking concurrently without waiting for either to finish
        _playerAttack = StartCoroutine(CreatureAttackCycle(playerCreature, enemyCreature));
        _enemyAttack = StartCoroutine(CreatureAttackCycle(enemyCreature, playerCreature));

        // Keep checking the health status of both creatures in a loop
        while (battleRunning && playerCreature != null && enemyCreature != null)
        {
            // If either creature has 0 health, stop the battle
            if (enemyCreature.CurrentHealth <= 0)
            {
                WinBattle();
                if (_playerAttack != null)
                {
                    StopCoroutine(_playerAttack);
                }
                if (_enemyAttack != null)
                {
                    StopCoroutine(_enemyAttack); 
                }
                yield break;
            }

            if (playerCreature.CurrentHealth <= 0)
            {
                hasBattleStarted = false;
                battleRunning = false;
                
                InventoryManager.Instance.SelectedCreatureForBattle = null;
                UI_BattleManager.Instance.SelectedCreature = null;
                UI_BattleManager.Instance.Refresh();
                UI_InventoryHoverManager.Instance.BattleText.text = playerCreature.CreatureName + " died!";
                if (_playerAttack != null)
                {
                    StopCoroutine(_playerAttack);
                }
                if (_enemyAttack != null)
                {
                    StopCoroutine(_enemyAttack); 
                }
                yield break;
            }

            // Check health frequently but don't block execution (yield for a short time to prevent freezing)
            yield return new WaitForSeconds(0.1f);
        }
    }

    // A coroutine for each creature to handle its attack cycle independently
    private IEnumerator CreatureAttackCycle(Creature attacker, Creature defender)
    {
        while (battleRunning && attacker != null && defender != null && attacker.CurrentHealth > 0 && defender.CurrentHealth > 0)
        {
            BigDecimal attackInterval = speedFactor.Round(3) / attacker.CreatureStats.Speed.Round(3);
            BigDecimal attackDamage = attacker.CreatureStats.Attack.Round(3);
            BigDecimal critchance = attacker.CreatureStats.Dexterity.Round(3) / new BigDecimal(100000,-3);
            // Calculate critical hit chance based on dexterity using a logistic function

            critchance = CalculateCritChance(attacker.CreatureStats.Dexterity);
            
            bool isCriticalHit = UnityEngine.Random.value < critchance; // Random.value gives a value between 0 and 1
            BigDecimal attack = 0;
            
            if (isCriticalHit)
            {
                attackDamage *= 1.2f;
                attack = defender.TakeDamage(attackDamage);
            }
            else
            { 
                attack = defender.TakeDamage(attackDamage);
            }
            
            UI_BattleDisplayManager.Instance.CreateDamagePopUp(attack.ToNumberSuffix(false),isCriticalHit, attacker);

            // Wait for the attack interval based on the attacker's speed before attacking again
            yield return new WaitForSeconds((float)attackInterval);
        }
    }

    private BigDecimal CalculateCritChance(BigDecimal x)
    {
        BigDecimal k = 0.100f;
        BigDecimal p = 0.100f;
        BigDecimal inside = (1.000 + k * x);
        BigDecimal insidePower = inside.Power(p);
        BigDecimal minusPart = (new BigDecimal(1000,-3) / insidePower);
        
        BigDecimal critChance = (1.000 - minusPart);

        return critChance;
    }


    public void StopBattle()
    {
        battleRunning = false;
        hasBattleStarted = false;
        if (UI_BattleManager.Instance != null)
        {
            UI_InventoryHoverManager.Instance.BattleText.text = "NO DATA FOUND!";
        }
        StopAllRoutines();
    }

    public void StopAllRoutines()
    {
        if(_playerAttack == null || _enemyAttack == null || _battleCoroutine == null)
            return;
        StopCoroutine(_playerAttack);
        StopCoroutine(_enemyAttack);
        StopCoroutine(_battleCoroutine);
    }
    
    public void ResumeBattle()
    {
        battleRunning = true;
    }

    public void SwitchAutoBattle()
    {
        autoBattle = !autoBattle;
    }

    public bool NextBattle()
    {
        Creature playerCreature = InventoryManager.Instance.SelectedCreatureForBattle;
        if (playerCreature == null || hasBattleStarted)
        {
            Debug.Log("Creature = "+ playerCreature);
            Debug.Log("hasBattleStarted = " + hasBattleStarted);
            return false;
        }

        StopAllCoroutines();
        StartBattle();
        return true;
    }

    public bool SetNextBattleButton()
    {
        Creature playerCreature = InventoryManager.Instance.SelectedCreatureForBattle;
        if(playerCreature == null || enemyCreature != null)
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
}
