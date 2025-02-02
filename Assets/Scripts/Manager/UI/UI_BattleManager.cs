using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Data;

public class UI_BattleManager : MonoBehaviour
{
    public static UI_BattleManager Instance;
    
    [SerializeField] private UI_CreatureSprite battleCreatureSpriteDetails;
    [SerializeField] private UI_CreatureDetailsText battleCreatureDetails;

    [SerializeField] private GameObject nextBattleButton;

    [SerializeField] private UI_ToggleButton toggleButton;

    [SerializeField] private UI_BattleCreatureItem attackCreature;
    [SerializeField] private UI_BattleCreatureItem healCreature;
    [SerializeField] private UI_BattleCreatureItem defenseCreature;

    [SerializeField] private Slider enemyHealthSlider;
    [SerializeField] private Slider enemyTimeSlider;

    private SoundManager soundManager;

    private Creature _selectedCreature;
    public Creature SelectedCreature { get => _selectedCreature; set => _selectedCreature = value; }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            soundManager = FindObjectOfType<SoundManager>();
            SetNextBattleButtonActive(false);
        }
    }

    private void OnEnable()
    {
        Refresh();
        CheckAllSlotsRepresentation();
        BattleManager.Instance.SetNextBattleButton();
        toggleButton.SetToggleState(BattleManager.Instance.AutoBattle);
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

    private void CheckAllSlotsRepresentation()
    {
        CheckCreatureRepresentation(CreatureBattleSlot.Attack);
        CheckCreatureRepresentation(CreatureBattleSlot.Heal);
        CheckCreatureRepresentation(CreatureBattleSlot.Defense);
    }

    private void UpdateHealthSlider(float amount, CreatureBattleSlot type)
    {
        ReturnBattleSlotItem(type)?.UpdateHealthSlider(amount);
        CheckCreatureRepresentation(type);
    }

    private void CheckCreatureRepresentation(CreatureBattleSlot type)
    {
        UI_BattleCreatureItem battleCreature = ReturnBattleSlotItem(type) ?? null;

        if(battleCreature == null)
        {
            return;
        }

        if (battleCreature.CurrentCreature == null || battleCreature.CurrentCreature?.CurrentHealth <= 0)
        {
            battleCreature.ResetCreatureRepresentation();
            return;
        }
        battleCreature.SetCreatureRepresentation(battleCreature.CurrentCreature);
    }

    private void UpdateShieldSlider(float amount, CreatureBattleSlot type)
    {
        ReturnBattleSlotItem(type)?.UpdateShieldSlider(amount);
    }

    private void UpdateTimeSlider(float amount, CreatureBattleSlot type)
    {
        ReturnBattleSlotItem(type)?.UpdateTimeSlider(amount);
    }

    private UI_BattleCreatureItem ReturnBattleSlotItem(CreatureBattleSlot type)
    {
        UI_BattleCreatureItem uiBattleCreatureItem = null;
        switch (type)
        {
            case CreatureBattleSlot.Attack:
                uiBattleCreatureItem = attackCreature;
                break;
            case CreatureBattleSlot.Heal:
                uiBattleCreatureItem = healCreature;
                break;
            case CreatureBattleSlot.Defense:
                uiBattleCreatureItem = defenseCreature;
                break;
        }
        return uiBattleCreatureItem;
    }

    private void UpdateEnemyHealthSlider(float amount)
    {
        enemyHealthSlider.value = amount;
    }

    private void UpdateEnemyTimeSlider(float amount)
    {
        enemyTimeSlider.value = amount;
    }

    public void OnHoverBattleCreature(CreatureBattleSlot type)
    {
        UI_BattleCreatureItem battleCreature = ReturnBattleSlotItem(type) ?? null;

        if(battleCreature == null || battleCreature.CurrentCreature == null)
        {
            return;
        }
        battleCreatureSpriteDetails.SetupRepresentation(battleCreature?.CurrentCreature);
        battleCreatureDetails.SetupRepresentation(battleCreature?.CurrentCreature);
    }

    public void OffHoverBattleCreature()
    {
        Refresh();
    }

    public void Refresh()
    {
        battleCreatureDetails?.Reset();
        battleCreatureSpriteDetails?.Reset();
        
        UI_InventoryManager.Instance.RefreshInventory();
    }
    
    public void RetreatAllCreatures()
    {
        BattleManager.Instance.RetreatAll();
        
        ReturnBattleSlotItem(CreatureBattleSlot.Attack).CurrentCreature = null;
        ReturnBattleSlotItem(CreatureBattleSlot.Defense).CurrentCreature = null;
        ReturnBattleSlotItem(CreatureBattleSlot.Heal).CurrentCreature = null;
        
        CheckAllSlotsRepresentation();
    }

    public void SwitchAutoBattle()
    {
        BattleManager.Instance.SwitchAutoBattle();
        toggleButton.SetToggleState(BattleManager.Instance.AutoBattle);
        soundManager.PlaySFX("Click");
    }

    public void NextBatlle()
    {
        if (BattleManager.Instance.NextBattle())
        {
            soundManager.PlaySFX("Click");
        }
        else 
        { 
            soundManager.PlaySFX("Error");
        }
    }

    public void SetNextBattleButtonActive(bool isActive)
    {
        nextBattleButton.SetActive(isActive);
    }

    public void StartBattle()
    {
        BattleManager.Instance.NextBattle();
    }
    
    
}
