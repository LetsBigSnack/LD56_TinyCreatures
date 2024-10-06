using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UI_BattleDisplayManager : MonoBehaviour
{
 
    public static UI_BattleDisplayManager Instance;
    
    
    [SerializeField] private UI_CreatureSprite battleCreatureSprite;
    [SerializeField] private UI_CreatureSprite enemyCreatureSprite;
    [SerializeField] private GameObject logPrefab;
    [SerializeField] private Transform logParent;
    [SerializeField] private List<GameObject> logs;
    [SerializeField] private int maxLogs = 4;
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
            DontDestroyOnLoad(gameObject);
            CreateLogEntry("log" + logs.Count);
            CreateLogEntry("log" + logs.Count);
            CreateLogEntry("log" + logs.Count);
            CreateLogEntry("log" + logs.Count);
        }
    }


    // Start is called before the first frame update
    private void FixedUpdate()
    {
        Creature battleCreature = InventoryManager.Instance.SelectedCreatureForBattle;
        Creature enemyCreature = BattleManager.Instance.EnemyCreature;
        
        battleCreatureSprite.SetupRepresentation(battleCreature);
        enemyCreatureSprite.SetupRepresentation(enemyCreature);
    }
    
    public void CreateLogEntry(string text)
    {
        GameObject logEntry = Instantiate(logPrefab, logParent.position, Quaternion.identity);
        logEntry.transform.SetParent(logParent);
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
