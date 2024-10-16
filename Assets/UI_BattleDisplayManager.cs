using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattleDisplayManager : MonoBehaviour
{
 
    public static UI_BattleDisplayManager Instance;
    
    
    [SerializeField] private UI_CreatureSprite battleCreatureSprite;
    [SerializeField] private UI_CreatureSprite enemyCreatureSprite;
    [SerializeField] private GameObject logPrefab;
    [SerializeField] private Transform logParent;
    [SerializeField] private List<GameObject> logs;
    [SerializeField] private int maxLogs = 4;
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
            logs = new List<GameObject>();
        }
    }


    // Start is called before the first frame update
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
            playerPL.text = battleCreature.PowerLevel.ToString();
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
            enemyPL.text = enemyCreature.PowerLevel.ToString();
        }
        
        battleCreatureSprite.SetupRepresentation(battleCreature);
        enemyCreatureSprite.SetupRepresentation(enemyCreature);
    }
    
    public void CreateLogEntry(string text)
    {
        GameObject logEntry = Instantiate(logPrefab, logParent.position, Quaternion.identity);
        logEntry.transform.SetParent(logParent, false);
        TextMeshProUGUI logEntryTMP = logEntry.GetComponentInChildren<TextMeshProUGUI>();
        logEntryTMP.text = text;
        logs.Add(logEntry);

        if (logs.Count > maxLogs)
        {
            GameObject logFirst = logs.First();
            Destroy(logFirst);
            logs.Remove(logFirst);
        }
        
    }
}
