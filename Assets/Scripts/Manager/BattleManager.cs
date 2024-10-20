using System;
using System.Collections;
using Helper.Util;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [Header("Battle Parameters")] 
    [SerializeField] private Creature enemyCreature;
    [Range(0, 100.0f)]
    [SerializeField] private float statRange = 3.5f;
    [Range(0, 100.0f)]
    [SerializeField] private float statMin = 10f;
    [SerializeField] private float speedFactor = 60f;
    [SerializeField] private float winFactor = 0.3f;

    public float StatRange{get{return statRange;}}
    public float StatMin{get{return statMin;}}

    [Header("Battle Information")] 
    [SerializeField] private bool battleRunning = true;
    [SerializeField] private bool hasBattleStarted = false;
    [SerializeField] private int playerWins = 0;
    [SerializeField] private float factorMult = 1.5f;
    
    
    public int PlayerWins{get{return playerWins;}}
    
    public float WinFactor{get{return winFactor;}}
    
    private Coroutine battleCoroutine;
    
    public Creature EnemyCreature
    {
        get => enemyCreature;
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

        }
    }

    private void Update()
    {
        if (!battleRunning)
        {
            return;
        }
        if (InventoryManager.Instance.SelectedCreatureForBattle != null && !hasBattleStarted)
        {
            StartBattle();
        }
    }

    public void StartBattle()
    {
        Creature playerCreature = InventoryManager.Instance.SelectedCreatureForBattle;
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

        battleCoroutine = StartCoroutine(BattleCoroutine());
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

        if (battleCoroutine != null)
        {
            StopCoroutine(battleCoroutine);
        }
    }

    private IEnumerator BattleCoroutine()
    {
        Creature playerCreature = InventoryManager.Instance.SelectedCreatureForBattle;

        yield return new WaitForSeconds(1);

        // Start both creatures attacking concurrently without waiting for either to finish
        Coroutine playerAttack = StartCoroutine(CreatureAttackCycle(playerCreature, enemyCreature));
        Coroutine enemyAttack = StartCoroutine(CreatureAttackCycle(enemyCreature, playerCreature));

        // Keep checking the health status of both creatures in a loop
        while (battleRunning && playerCreature != null && enemyCreature != null)
        {
            // If either creature has 0 health, stop the battle
            if (enemyCreature.CurrentHealth <= 0)
            {
                WinBattle();
                StopCoroutine(playerAttack);
                StopCoroutine(enemyAttack);
                yield break;
            }

            if (playerCreature.CurrentHealth <= 0)
            {
                hasBattleStarted = false;
                battleRunning = false;
                
                InventoryManager.Instance.SelectedCreatureForBattle = null;
                UI_BattleManager.Instance.SelectedCreature = null;
                UI_BattleManager.Instance.Refresh();
                StopCoroutine(playerAttack);
                StopCoroutine(enemyAttack);
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
            float attackInterval = speedFactor / attacker.CreatureStats.Speed;
            float attackDamage = attacker.CreatureStats.Attack;

            // Calculate critical hit chance based on dexterity using a logistic function
            float critChance = 1 - Mathf.Exp(-attacker.CreatureStats.Dexterity / 100f); // This approaches 1 but never reaches it

            bool isCriticalHit = UnityEngine.Random.value < critChance; // Random.value gives a value between 0 and 1
            int attack = 0;
            
            if (isCriticalHit)
            {
                attackDamage *= 1.2f;
                attack = defender.TakeDamage(attackDamage);
            }
            else
            { 
                attack = defender.TakeDamage(attackDamage);
            }
            
            UI_BattleDisplayManager.Instance.CreateDamagePopUp(Util_LargeNumberDisplay.LargerNumberConversion(attack, false),isCriticalHit, attacker);


            // Wait for the attack interval based on the attacker's speed before attacking again
            yield return new WaitForSeconds(attackInterval);
        }
    }


    public void StopBattle()
    {
        battleRunning = false;
    }
    
    public void ResumeBattle()
    {
        battleRunning = true;
    }

    public int GetPredictedPowerLevel()
    {
        // Adjust stat range and minimum based on player wins and win factor
        float adjustedStatRange = statRange + (playerWins * winFactor);
        float adjustedStatMin = statMin + (playerWins * winFactor * 2);

        // Calculate the average of the adjusted stat range for each individual stat
        float averageMaxHealth = adjustedStatMin;
        float averageSpeed = adjustedStatMin;
        float averageAttack = adjustedStatMin;
        float averageDefense = adjustedStatMin;
        float averageDexterity = adjustedStatMin;

        // Define weights for each stat component
        float healthWeight = 0.10f;    // Weight for HP
        float speedWeight = 0.25f;      // Weight for speed
        float attackWeight = 0.25f;     // Weight for attack
        float defenseWeight = 0.2f;    // Weight for defense
        float dexterityWeight = 0.2f; // Weight for dexterity

        // Calculate the weighted average power level using the actual average stats
        float averagePowerLevel = (averageMaxHealth * healthWeight) +
                                  (averageSpeed * speedWeight) +
                                  (averageAttack * attackWeight) +
                                  (averageDefense * defenseWeight) +
                                  (averageDexterity * dexterityWeight);

        // Return the rounded average power level
        Debug.Log(averagePowerLevel);
        return Mathf.RoundToInt(averagePowerLevel);
    }
}
