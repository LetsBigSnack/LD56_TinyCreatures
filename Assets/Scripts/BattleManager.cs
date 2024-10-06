using System;
using System.Collections;
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
    [SerializeField] private float speedFactor = 15f;
    [SerializeField] private float winFactor = 0.6f;

    public float StatRange{get{return statRange;}}
    public float StatMin{get{return statMin;}}

    [Header("Battle Information")] 
    [SerializeField] private bool battleRunning = true;
    [SerializeField] private bool hasBattleStarted = false;
    [SerializeField] private int playerWins = 0;

    public int PlayerWins{get{return playerWins;}}
    
    
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
            DontDestroyOnLoad(gameObject);
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
                                        statMin + (playerWins * winFactor * 2))
                .GetComponent<Creature>();
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

        StoreManager.Instance.EarnMoney(enemyCreature.PowerLevel * 5);
        Destroy(enemyCreature.gameObject);
        enemyCreature = null;
        playerWins++;
        hasBattleStarted = false;

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
                UI_BattleDisplayManager.Instance.CreateLogEntry($"Player: {playerCreature.CreatureName} defeated {enemyCreature.CreatureName}!");
                WinBattle();
                StopCoroutine(playerAttack);
                StopCoroutine(enemyAttack);
                yield break;
            }

            if (playerCreature.CurrentHealth <= 0)
            {
                UI_BattleDisplayManager.Instance.CreateLogEntry($"Enemy: {enemyCreature.CreatureName} defeated {playerCreature.CreatureName}!");
                hasBattleStarted = false;
                if (playerCreature.gameObject != null)
                {
                    Destroy(playerCreature.gameObject);
                }
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
            float attackInterval = speedFactor / attacker.Speed;
            float attackDamage = attacker.Attack;

            UI_BattleDisplayManager.Instance.CreateLogEntry($"{attacker.CreatureName} attacks {defender.CreatureName} for {attackDamage} damage.");
            defender.TakeDamage(attackDamage);

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

    public int GetPredictedPowerLevel(float adjustment = 1.0f)
    {
        float adjustedStat = statMin + (playerWins * winFactor * 2) + (statRange + (playerWins * winFactor));
        float predictedPowerLevel = ((adjustedStat * 4) / 4) * adjustedStat;
        return  Mathf.RoundToInt(predictedPowerLevel);
    }

    
}
