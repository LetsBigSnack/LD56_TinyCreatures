using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattleInventoryManager : MonoBehaviour
{
    public static UI_BattleInventoryManager Instance;
    [SerializeField] private UI_CreatureSprite battleCreatureSprite;
    [SerializeField] private UI_CreatureSprite enemyCreatureSprite;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject enemyObject;
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private Slider enemyHealthBar;
    [SerializeField] private TextMeshProUGUI playerPL;
    [SerializeField] private TextMeshProUGUI enemyPL;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void FixedUpdate()
    {
        Creature battleCreature = InventoryManager.Instance.SelectedCreatureForBattle;
        Creature enemyCreature = BattleManager.Instance.EnemyCreature;

        if (battleCreature == null)
        {
            playerObject.SetActive(false);
        }
        else
        {
            playerObject.SetActive(true);
            BigDecimal healthPercentage = battleCreature.CurrentHealth.Round(3) / battleCreature.MaxHealth.Round(3);
            playerHealthBar.maxValue = 1;
            playerHealthBar.value = (float)healthPercentage;
            playerPL.text = battleCreature.CreatureStats.PowerLevel.ToNumberSuffix(false);
        }

        if (enemyCreature == null)
        {
            enemyObject.SetActive(false);
        }
        else
        {
            enemyObject.SetActive(true);
            BigDecimal healthPercentage = enemyCreature.CurrentHealth.Round(3) / enemyCreature.MaxHealth.Round(3);
            enemyHealthBar.maxValue = 1;
            enemyHealthBar.value = (float)healthPercentage;
            enemyPL.text = enemyCreature.CreatureStats.PowerLevel.ToNumberSuffix(false);
        }

        battleCreatureSprite.SetupRepresentation(battleCreature);
        enemyCreatureSprite.SetupRepresentation(enemyCreature);
    }
}
