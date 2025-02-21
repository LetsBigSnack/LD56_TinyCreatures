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
    
    [SerializeField] private UI_CreatureDetailsText battleCreatureDetails;

    [SerializeField] private GameObject nextBattleButton;

    [SerializeField] private UI_ToggleButton toggleButton;

    [SerializeField] private UI_BattleCreatureItem attackCreature;
    [SerializeField] private UI_BattleCreatureItem healCreature;
    [SerializeField] private UI_BattleCreatureItem defenseCreature;

    [SerializeField] private Slider enemyHealthSlider;
    [SerializeField] private Slider enemyTimeSlider;

    [SerializeField] private UI_CreatureSprite attackSprite;
    [SerializeField] private UI_CreatureSprite healSprite;
    [SerializeField] private UI_CreatureSprite defenseSprite;

    [SerializeField] private UI_BattleSlotHelper attackSlot;
    [SerializeField] private UI_BattleSlotHelper healSlot;
    [SerializeField] private UI_BattleSlotHelper defenseSlot;

    [SerializeField] private GameObject onHoverObj;
    [SerializeField] private GameObject offHoverObj;

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

        SetBattleCreatureRepresentation(type, InventoryManager.Instance.CreatureBattleSlots[type]);

        if (battleCreature == null)
        {
            return;
        }

        if (battleCreature.CurrentCreature == null || battleCreature.CurrentCreature?.CurrentHealth <= 0)
        {
            battleCreature.ResetCreatureRepresentation();
            battleCreature.RetreatButton.interactable = false;
            ReturnBattleSlotHelper(type).SetEmpty();
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

    private UI_BattleSlotHelper ReturnBattleSlotHelper(CreatureBattleSlot type)
    {
        UI_BattleSlotHelper uiBattleCreatureItem = null;
        switch (type)
        {
            case CreatureBattleSlot.Attack:
                uiBattleCreatureItem = attackSlot;
                break;
            case CreatureBattleSlot.Heal:
                uiBattleCreatureItem = healSlot;
                break;
            case CreatureBattleSlot.Defense:
                uiBattleCreatureItem = defenseSlot;
                break;
        }
        return uiBattleCreatureItem;
    }

    private void UpdateEnemyHealthSlider(float amount)
    {
        enemyHealthSlider.value = amount;
        if(amount <= 0)
        {
            UI_BattleInventoryManager.Instance.Enemy.ToggleActiveState();
        }
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
        offHoverObj.SetActive(false);
        onHoverObj.SetActive(true);
        battleCreatureDetails.SetupRepresentation(battleCreature?.CurrentCreature);
    }

    public void OffHoverBattleCreature()
    {
        onHoverObj.SetActive(false);
        offHoverObj.SetActive(true);
        Refresh();
    }

    public void Refresh()
    {
        battleCreatureDetails?.Reset();
        
        UI_InventoryManager.Instance.RefreshInventory();
    }
    
    public void RetreatAllCreatures()
    {

        if (InventoryManager.Instance.HasSpace(3))
        {
            SoundManager.Instance.PlaySFX("Click");
            BattleManager.Instance.RetreatAll();
        
            ReturnBattleSlotItem(CreatureBattleSlot.Attack).CurrentCreature = null;
            ReturnBattleSlotItem(CreatureBattleSlot.Defense).CurrentCreature = null;
            ReturnBattleSlotItem(CreatureBattleSlot.Heal).CurrentCreature = null;
        
            CheckAllSlotsRepresentation();
            SetAllBattleSlotEmpty();
        }
        else
        {
            SoundManager.Instance.PlaySFX("Error");
        }
    }

    public void SetAllBattleSlotEmpty()
    {
        attackSlot.SetEmpty();
        healSlot.SetEmpty();
        defenseSlot.SetEmpty();
    }

    public void SetBattleSlotEmpty(CreatureBattleSlot slot)
    {
        switch(slot)
        {
            case CreatureBattleSlot.Attack:
                attackSlot.SetEmpty();
                break;
            case CreatureBattleSlot.Defense:
                defenseSlot.SetEmpty(); 
                break;
            case CreatureBattleSlot.Heal:
                healSlot.SetEmpty(); 
                break;
        } 
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

    public void SetBattleCreatureRepresentation(CreatureBattleSlot battleSlot, Creature creature)
    {
        if(creature == null)
        {
            ReturnSpriteSlot(battleSlot).Reset();
            return;
        }

        ReturnSpriteSlot(battleSlot).SetupRepresentation(creature);
        UI_BattleInventoryManager.Instance.ReturnedBattleInventoryItem(battleSlot).ToggleActiveState();
    }

    public UI_CreatureSprite ReturnSpriteSlot(CreatureBattleSlot battleSlot)
    {
        UI_CreatureSprite returnSprite = new UI_CreatureSprite();

        switch (battleSlot)
        {
            case CreatureBattleSlot.Attack:
                 returnSprite = attackSprite;
                break;
            case CreatureBattleSlot.Defense:
                 returnSprite = defenseSprite;
                break;
            case CreatureBattleSlot.Heal:
                 returnSprite = healSprite;
                break;
        }

        return returnSprite;
    }
    
}
