using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Data;

public class UI_BattleManager : MonoBehaviour
{
    public static UI_BattleManager Instance;
    
    [SerializeField] private UI_CreatureDetailsText battleCreatureDetails;

    [SerializeField] private GameObject nextBattleButton;
    [SerializeField] private TextMeshProUGUI startButtonText;

    [SerializeField] private UI_ToggleButton toggleButton;

    [SerializeField] private UI_BattleCreatureItem attackCreature;
    [SerializeField] private UI_BattleCreatureItem healCreature;
    [SerializeField] private UI_BattleCreatureItem defenseCreature;

    [SerializeField] private Slider enemyHealthSlider;
    [SerializeField] private Slider enemyTimeSlider;
    [SerializeField] private GameObject activeEnemy;
    [SerializeField] private GameObject inactiveEnemy;


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
        }
    }

    private void OnEnable()
    {
        RefreshCreatureDetails();
        CheckAllSlotsRepresentation();
        BattleManager.Instance.SetNextBattleButton();
        toggleButton.SetToggleState(BattleManager.Instance.AutoBattle);
        BattleManager.OnCreatureHealthChanged += UpdateHealthSlider;
        BattleManager.OnCreatureShieldChanged += UpdateShieldSlider;
        BattleManager.OnCreatureTimeChanged += UpdateTimeSlider;
        BattleManager.OnEnemyHealthChanged += UpdateEnemyHealthSlider;
        BattleManager.OnEnemyTimeChanged += UpdateEnemyTimeSlider;
        InventoryManager.OnCreatureChanged += CheckCreatureRepresentation;
        BattleManager.OnBattleRunningChanged += ToggleStartButton;
        ToggleStartButton(BattleManager.Instance.IsBattleRunning);
    }

    private void OnDisable()
    {
        BattleManager.OnCreatureHealthChanged -= UpdateHealthSlider;
        BattleManager.OnCreatureShieldChanged -= UpdateShieldSlider;
        BattleManager.OnCreatureTimeChanged -= UpdateTimeSlider;
        BattleManager.OnEnemyHealthChanged -= UpdateEnemyHealthSlider;
        BattleManager.OnEnemyTimeChanged -= UpdateEnemyTimeSlider;
        InventoryManager.OnCreatureChanged -= CheckCreatureRepresentation;
    }

    private void ToggleStartButton(bool isBattleRunning)
    {
        if (isBattleRunning)
        {
            startButtonText.text = "Stop";
            return;
        }
        startButtonText.text = "Start";
    }

    private void UpdateHealthSlider(float amount, CreatureBattleSlot type)
    {
        ReturnBattleSlotItem(type)?.UpdateHealthSlider(amount);
        if(amount <= 0)
        {
            OnCreatureRemoved(type);
        }
    }

    private void UpdateShieldSlider(float amount, CreatureBattleSlot type)
    {
        ReturnBattleSlotItem(type)?.UpdateShieldSlider(amount);
    }

    private void UpdateTimeSlider(float amount, CreatureBattleSlot type)
    {
        ReturnBattleSlotItem(type)?.UpdateTimeSlider(amount);
    }

    private void UpdateEnemyHealthSlider(float amount)
    {
        if(inactiveEnemy.activeInHierarchy && amount > 0)
        {
            inactiveEnemy.SetActive(false);
            activeEnemy.SetActive(true);
        }

        if (!BattleManager.Instance.AutoBattle && amount <= 0)
        {
            activeEnemy.SetActive(false);
            inactiveEnemy.SetActive(true);
            ResetSlider(CreatureBattleSlot.Attack);
            ResetSlider(CreatureBattleSlot.Heal);
            ResetSlider(CreatureBattleSlot.Defense);
            UI_BattleInventoryManager.Instance.ResetAllSliders();
        }
        enemyHealthSlider.value = amount;
        UI_BattleInventoryManager.Instance.Enemy.ToggleActiveState();
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
        RefreshCreatureDetails();
    }

    private void CheckAllSlotsRepresentation()
    {
        CheckCreatureRepresentation(CreatureBattleSlot.Attack);
        CheckCreatureRepresentation(CreatureBattleSlot.Heal);
        CheckCreatureRepresentation(CreatureBattleSlot.Defense);
    }

    private void CheckCreatureRepresentation(CreatureBattleSlot type)
    {
        UI_BattleCreatureItem battleCreature = ReturnBattleSlotItem(type) ?? null;

        if (battleCreature == null)
        {
            return;
        }

        UpdateBattleCreatureRepresentation(type, InventoryManager.Instance.CreatureBattleSlots[type]);
    }

    public void ResetSlider(CreatureBattleSlot type)
    {
        UpdateHealthSlider(1, type);
        UpdateTimeSlider(0, type);
        UpdateShieldSlider(0, type);
    }

    public void RefreshCreatureDetails()
    {
        battleCreatureDetails?.Reset();   
        UI_InventoryManager.Instance.RefreshInventory();
    }

    public void StopBattle()
    {
        BattleManager.Instance.StopBattle();
        UI_BattleInventoryManager.Instance.ResetAllSliders();
    }
    
    public void RetreatAllCreatures()
    {
        if (!IsACreatureInASlot())
        {
            SoundManager.Instance.PlaySFX("Error");
            UI_ToastManager.Instance.CreateToast(NotificationType.Alert, "No Creature", "There's no creature to return to inventory!");
            return;
        }


        if (InventoryManager.Instance.HasSpace(3))
        {
            SoundManager.Instance.PlaySFX("Click");
            BattleManager.Instance.RetreatAll();
        
            ReturnBattleSlotItem(CreatureBattleSlot.Attack).CurrentCreature = null;
            ReturnBattleSlotItem(CreatureBattleSlot.Defense).CurrentCreature = null;
            ReturnBattleSlotItem(CreatureBattleSlot.Heal).CurrentCreature = null;

            UI_BattleInventoryManager.Instance.ResetAllSliders();
        
            CheckAllSlotsRepresentation();
            SetAllBattleSlotEmpty();
            return;
        }
        else
        {
            SoundManager.Instance.PlaySFX("Error");
            UI_ToastManager.Instance.CreateToast(NotificationType.Alert, "Inventory full", "You don't have the space, sell some creature or buy space!");
        }
    }

    private bool IsACreatureInASlot()
    {
        if(ReturnBattleSlotItem(CreatureBattleSlot.Attack).CurrentCreature != null ||
            ReturnBattleSlotItem(CreatureBattleSlot.Defense).CurrentCreature != null ||
            ReturnBattleSlotItem(CreatureBattleSlot.Heal).CurrentCreature != null)
        {
            return true;
        }

        return false;
    }

    private void SetAllBattleSlotEmpty()
    {
        attackSlot.SetEmpty();
        healSlot.SetEmpty();
        defenseSlot.SetEmpty();
    }

    public void OnCreatureRemoved(CreatureBattleSlot type)
    {
        ReturnBattleSlotItem(type).ResetCreatureRepresentation();
        ReturnCreatureSprite(type).Reset();
        ReturnBattleSlotHelper(type).SetEmpty();
        UI_BattleInventoryManager.Instance.ReturnedBattleInventoryItem(type).ToggleActiveState();
    }

    public void SwitchAutoBattle()
    {
        BattleManager.Instance.SwitchAutoBattle();
        toggleButton.SetToggleState(BattleManager.Instance.AutoBattle);
        soundManager.PlaySFX("Click");
    }

    public void NextBattle()
    {
        if (BattleManager.Instance.NextBattle())
        {
            soundManager.PlaySFX("Click");
        }
        else 
        { 
            soundManager.PlaySFX("Error");
            UI_ToastManager.Instance.CreateToast(NotificationType.Alert, "No Attacker", "You need at least an attacker creature to start the training!");
        }
    }

    public void SetNextBattleButtonActive(bool isActive)
    {
        nextBattleButton.SetActive(isActive);
    }

    public void StartBattle()
    {
        if (BattleManager.Instance.IsBattleRunning) StopBattle();
        else if (!BattleManager.Instance.NextBattle())
        {
            soundManager.PlaySFX("Error");
            UI_ToastManager.Instance.CreateToast(NotificationType.Alert, "No Attacker", "You need at least an attacker creature to start the training!");
        }
    }

    public void UpdateBattleCreatureRepresentation(CreatureBattleSlot battleSlot, Creature creature)
    {
        if (creature == null)
        {
            ReturnCreatureSprite(battleSlot).Reset();
            ResetSlider(battleSlot);
            ReturnBattleSlotHelper(battleSlot).SetEmpty();
            ReturnBattleSlotItem(battleSlot).ResetCreatureRepresentation();
            return;
        }

        ReturnCreatureSprite(battleSlot).SetupRepresentation(creature);
        ReturnBattleSlotHelper(battleSlot).SetFull();
        ReturnBattleSlotItem(battleSlot).SetCreatureRepresentation(creature);
        UI_BattleInventoryManager.Instance.ReturnedBattleInventoryItem(battleSlot).ToggleActiveState();
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

    private UI_CreatureSprite ReturnCreatureSprite(CreatureBattleSlot type)
    {
        UI_CreatureSprite uiCreatureSprite = null;
        switch (type)
        {
            case CreatureBattleSlot.Attack:
                uiCreatureSprite = attackSprite;
                break;
            case CreatureBattleSlot.Heal:
                uiCreatureSprite = healSprite;
                break;
            case CreatureBattleSlot.Defense:
                uiCreatureSprite = defenseSprite;
                break;
        }
        return uiCreatureSprite;
    }
}
