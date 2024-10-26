using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            playerHealthBar.maxValue = battleCreature.MaxHealth;
            playerHealthBar.value = battleCreature.CurrentHealth;
            playerPL.text = battleCreature.CreatureStats.PowerLevel.ToString();
        }

        if (enemyCreature == null)
        {
            enemyObject.SetActive(false);
        }
        else
        {
            enemyObject.SetActive(true);
            enemyHealthBar.maxValue = enemyCreature.MaxHealth;
            enemyHealthBar.value = enemyCreature.CurrentHealth;
            enemyPL.text = enemyCreature.CreatureStats.PowerLevel.ToString();
        }

        battleCreatureSprite.SetupRepresentation(battleCreature);
        enemyCreatureSprite.SetupRepresentation(enemyCreature);
    }
}
