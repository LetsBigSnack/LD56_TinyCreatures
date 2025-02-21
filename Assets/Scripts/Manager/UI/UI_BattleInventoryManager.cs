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
    [SerializeField] private List<UI_BattleInventoryItem> uiCreatureItem;
    [SerializeField] private UI_BattleInventoryItem enemyItem;

    public UI_BattleInventoryItem Enemy
    {
        get { return enemyItem; }
    }

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

    private void OnEnable()
    {
        BattleManager.OnCreatureHealthChanged += UpdateHealthSlider;
        BattleManager.OnCreatureShieldChanged += UpdateShieldSlider;
        BattleManager.OnCreatureTimeChanged += UpdateTimeSlider;
        BattleManager.OnEnemyHealthChanged += UpdateEnemyHealthSlider;
        BattleManager.OnEnemyTimeChanged += UpdateEnemyTimeSlider;
    }

    private void OnDisable()
    {
        BattleManager.OnCreatureHealthChanged -= UpdateHealthSlider;
        BattleManager.OnCreatureShieldChanged -= UpdateShieldSlider;
        BattleManager.OnCreatureTimeChanged -= UpdateTimeSlider;
        BattleManager.OnEnemyHealthChanged -= UpdateEnemyHealthSlider;
        BattleManager.OnEnemyTimeChanged -= UpdateEnemyTimeSlider;
    }

    private void UpdateEnemyHealthSlider(float amount)
    {
        enemyItem.UpdateHealthSlider(amount);
    }

    private void UpdateEnemyTimeSlider(float amount)
    {
        enemyItem.UpdateTimeSlider(amount);
    }

    private void UpdateHealthSlider(float amount, CreatureBattleSlot type)
    {
        ReturnedBattleInventoryItem(type).UpdateHealthSlider(amount);
    }

    private void UpdateShieldSlider(float amount, CreatureBattleSlot type)
    {
        ReturnedBattleInventoryItem(type).UpdateShieldSlider(amount);
    }
    private void UpdateTimeSlider(float amount, CreatureBattleSlot type)
    {
        ReturnedBattleInventoryItem(type).UpdateTimeSlider(amount);
    }

    public UI_BattleInventoryItem ReturnedBattleInventoryItem(CreatureBattleSlot type)
    {
        UI_BattleInventoryItem currentItem = null;
        switch (type)
        {
            case CreatureBattleSlot.Attack:
                currentItem = uiCreatureItem.Find(c => c.Type == CreatureBattleSlot.Attack);
                break;
            case CreatureBattleSlot.Defense:
                currentItem = uiCreatureItem.Find(c => c.Type == CreatureBattleSlot.Defense);
                break;
            case CreatureBattleSlot.Heal:
                currentItem = uiCreatureItem.Find(c => c.Type == CreatureBattleSlot.Heal);
                break;
        }

        return currentItem;
    }

}
