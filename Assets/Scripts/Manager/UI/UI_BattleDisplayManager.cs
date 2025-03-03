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
    [SerializeField] private GameObject enemyObject;
    [SerializeField] private Slider enemyHealthBar;
    [SerializeField] private TextMeshProUGUI enemyPL;
    [SerializeField] private TextMeshProUGUI enemyName;

    [SerializeField] private Transform enemyEffectBox;
    [SerializeField] private Transform attackerEffectBox;
    [SerializeField] private Transform defenderEffectBox;
    [SerializeField] private Transform healerEffectBox;

    [SerializeField] private GameObject effectPrefab;

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

    public void OnEnable()
    {
        UpdateEnemy(BattleManager.Instance.EnemyCreature);
        BattleManager.OnEnemyCreatureChanged += UpdateEnemy;
    }

    public void OnDisable()
    {
        BattleManager.OnEnemyCreatureChanged -= UpdateEnemy;
    }

    public void UpdateEnemy(Creature enemyCreature)
    {
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
            enemyName.text = enemyCreature.GenerateRandomName();
        }
        SetEnemyCreatureRepresentation(enemyCreature);
    }

    public void SpawnEffect(EffectType effectType, CreatureBattleSlot slotType, BigDecimal amount, bool isCritical)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        Transform effectBoxTransform = ReturnEffectBoxTransform(slotType);
        GameObject damageEffect = Instantiate(effectPrefab, effectBoxTransform.position, Quaternion.identity);
        damageEffect.transform.SetParent(effectBoxTransform, false);
        damageEffect.transform.position = effectBoxTransform.position;

        switch (effectType){
            case EffectType.Damage:
                damageEffect.GetComponent<UI_EffectItem>().SetEffect(amount, EffectType.Damage, isCritical);
                break;
            case EffectType.Shield:
                damageEffect.GetComponent<UI_EffectItem>().SetEffect(amount, EffectType.Shield, isCritical);
                break;
            case EffectType.Heal:
                damageEffect.GetComponent<UI_EffectItem>().SetEffect(amount, EffectType.Heal, isCritical);
                break;
        }
    }

    public Transform ReturnEffectBoxTransform(CreatureBattleSlot type)
    {
        Transform transform = null;

        switch (type)
        {
            case CreatureBattleSlot.Attack:
                transform = attackerEffectBox;
                break;
            case CreatureBattleSlot.Heal:
                transform = healerEffectBox;
                break;
            case CreatureBattleSlot.Defense:
                transform = defenderEffectBox;
                break;
            case CreatureBattleSlot.Enemy:
                transform = enemyEffectBox;
                break;
        }

        return transform;
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
