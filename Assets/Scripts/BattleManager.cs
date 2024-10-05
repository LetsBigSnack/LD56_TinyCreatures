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
        
        while (battleRunning && playerCreature != null && enemyCreature != null && playerCreature.CurrentHealth > 0 && enemyCreature.CurrentHealth > 0)
        {
            yield return StartCoroutine(PerformAttack(playerCreature, enemyCreature));

            if (enemyCreature.CurrentHealth <= 0)
            {
                Debug.Log("Player wins!");
                WinBattle();
                yield break; // Stop the battle immediately when the enemy dies
            }

            yield return StartCoroutine(PerformAttack(enemyCreature, playerCreature));

            if (playerCreature.CurrentHealth <= 0)
            {
                Debug.Log("Enemy wins!");
                hasBattleStarted = false;
                Destroy(playerCreature.gameObject); // Destroy player if defeated
                playerCreature = null;
                yield break; // Stop the battle immediately when the player dies
            }
        }
    }


    private IEnumerator PerformAttack(Creature attacker, Creature defender)
    {
        float attackInterval = speedFactor / attacker.Speed;
        float attackDamage = attacker.Attack;

        Debug.Log($"{attacker.CreatureName} attacks {defender.CreatureName} for {attackDamage} damage.");
        defender.TakeDamage(attackDamage);
        
        // Wait for the attack interval based on the attacker's speed
        yield return new WaitForSeconds(attackInterval);
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
