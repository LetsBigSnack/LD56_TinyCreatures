using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helper.Util;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattleDisplayManager : MonoBehaviour
{
 
    public static UI_BattleDisplayManager Instance;
    
    [SerializeField] private UI_CreatureSprite enemyCreatureSprite;
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private GameObject critPrefab;
    [SerializeField] private GameObject enemyObject;
    [SerializeField] private Slider enemyHealthBar;
    [SerializeField] private TextMeshProUGUI enemyPL;
    [SerializeField] private TextMeshProUGUI enemyName;

    [SerializeField] private UI_CreatureSprite enemySprite;

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
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        Creature enemyCreature = BattleManager.Instance.EnemyCreature;

        if (enemyCreature == null)
        {
            enemyObject.SetActive(false);
            enemyName.text = "";
        }
        else
        {
            enemyObject.SetActive(true);
            BigDecimal healthPercentage = enemyCreature.CurrentHealth.Round(3) / enemyCreature.MaxHealth.Round(3);
            enemyHealthBar.maxValue = 1;
            enemyHealthBar.value = (float)healthPercentage;
            enemyPL.text = enemyCreature.CreatureStats.PowerLevel.ToNumberSuffix(false);
            if(enemyName.text == "")
            {
                enemyName.text = enemyCreature.GenerateRandomName();
            }  
        }
        SetEnemyCreatureRepresentation(enemyCreature);
    }
    
    public void CreateDamagePopUp(string text, bool isCrit, Creature creature)
    {
        Transform spawnPosition;
        
        //TODO: magic constant
        if(creature.CreatureName != "Enemy")
        {
            spawnPosition = enemyObject.transform;
            chooseAttackPrefab(text, isCrit, spawnPosition);
        } 
        else
        {
            //spawnPosition = playerObject.transform;
            //chooseAttackPrefab(text, isCrit, spawnPosition);
        }
    }

    public void chooseAttackPrefab(string damage, bool isCrit, Transform creature)
    {
        if (isCrit)
        {
            GameObject critEntry = Instantiate(critPrefab, new Vector2(creature.position.x, creature.position.y) , Quaternion.identity);
            critEntry.transform.SetParent(creature.transform, false);
            critEntry.transform.position = new Vector2(creature.position.x, creature.position.y + 100);
            critEntry.GetComponentInChildren<TextMeshProUGUI>().text = damage + "!!";
            Destroy(critEntry, 1);
            return;
        }

        GameObject attackEntry = Instantiate(attackPrefab, new Vector2(creature.position.x, creature.position.y) , Quaternion.identity);
        attackEntry.transform.SetParent(creature.transform, false);
        attackEntry.transform.position = new Vector2(creature.position.x, creature.position.y + 100);
        attackEntry.GetComponentInChildren<TextMeshProUGUI>().text = damage;
        Destroy(attackEntry, 1);
        return;
    }

    public void SetEnemyCreatureRepresentation(Creature creature)
    {
        if (creature == null)
        {
            enemySprite.Reset();
            enemyCreatureSprite.Reset();
            return;
        }
        enemyCreatureSprite.SetupRepresentation(creature);
        enemySprite.SetupRepresentation(creature);
    }

}
