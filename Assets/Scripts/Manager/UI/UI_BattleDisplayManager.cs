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
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private GameObject critPrefab;
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
    
    public void CreateDamagePopUp(string text, bool isCrit, Creature creature)
    {
        Transform spawnPosition;

        if(creature.CreatureName == playerObject.GetComponentInChildren<UICreatureButton>().creature.CreatureName)
        {
            spawnPosition = enemyObject.transform;
            chooseAttackPrefab(text, isCrit, spawnPosition);
        } 
        else
        {
            spawnPosition = playerObject.transform;
            chooseAttackPrefab(text, isCrit, spawnPosition);
        }
    }

    public void chooseAttackPrefab(string damage, bool isCrit, Transform creature)
    {
        if (isCrit)
        {
            GameObject critEntry = Instantiate(critPrefab, new Vector2(creature.position.x, creature.position.y) , Quaternion.identity);
            critEntry.transform.parent = creature.transform;
            critEntry.transform.position = new Vector2(creature.position.x, creature.position.y + 100);
            critEntry.GetComponentInChildren<TextMeshProUGUI>().text = damage + "!!";
            Destroy(critEntry, 1);
            return;
        }

        GameObject attackEntry = Instantiate(attackPrefab, new Vector2(creature.position.x, creature.position.y) , Quaternion.identity);
        attackEntry.transform.parent = creature.transform;
        attackEntry.transform.position = new Vector2(creature.position.x, creature.position.y + 100);
        attackEntry.GetComponentInChildren<TextMeshProUGUI>().text = damage;
        Destroy(attackEntry, 1);
        return;
    }
}
