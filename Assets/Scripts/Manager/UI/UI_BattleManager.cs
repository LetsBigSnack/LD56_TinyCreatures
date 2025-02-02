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
    
    [SerializeField] private UI_CreatureSprite battleCreatureSprite;
    [SerializeField] private UI_CreatureDetailsText battleCreatureDetails;
    [SerializeField] private UI_CreatureSprite activeBattleCreatureButton;
    [SerializeField] private UICreatureButton activeBattleCreature;
    [SerializeField] private GameObject nextBattleButton;
    [SerializeField] private UI_ToggleButton toggleButton;

    [SerializeField] private Slider attackHealthSlider;
    [SerializeField] private Slider attackTimeSlider;
    [SerializeField] private Slider attackShieldSlider;

    [SerializeField] private Slider healHealthSlider;
    [SerializeField] private Slider healTimeSlider;
    [SerializeField] private Slider healShieldSlider;

    [SerializeField] private Slider defenseHealthSlider;
    [SerializeField] private Slider defenseTimeSlider;
    [SerializeField] private Slider defenseShieldSlider;

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

    private void UpdateHealthSlider(float amount, CreatureBattleSlot type)
    {
        Debug.Log("Health: " + amount + ", slot:" + type);
        switch (type)
        {
            case CreatureBattleSlot.Attack:
                attackHealthSlider.value = amount;
                break;
            case CreatureBattleSlot.Heal:
                healHealthSlider.value = amount;
                break;
            case CreatureBattleSlot.Defense:
                defenseHealthSlider.value = amount;
                break;
        }
    }


    private void UpdateShieldSlider(float amount, CreatureBattleSlot type)
    {
        Debug.Log("Shield: " + amount + ", slot:" + type);
        switch (type)
        {
            case CreatureBattleSlot.Attack:
                attackShieldSlider.value = amount;
                break;
            case CreatureBattleSlot.Heal:
                healShieldSlider.value = amount;
                break;
            case CreatureBattleSlot.Defense:
                defenseShieldSlider.value = amount;
                break;
        }
    }


    private void UpdateTimeSlider(float amount, CreatureBattleSlot type)
    {
        Debug.Log("Time: " + amount + ", slot:" + type);
        switch (type)
        {
            case CreatureBattleSlot.Attack:
                attackTimeSlider.value = amount;
                break;
            case CreatureBattleSlot.Heal:
                healTimeSlider.value = amount;
                break;
            case CreatureBattleSlot.Defense:
                defenseTimeSlider.value = amount;
                break;
        }
    }

    private void UpdateEnemyHealthSlider(float amount)
    {
        enemyHealthSlider.value = amount;
    }

    private void UpdateEnemyTimeSlider(float amount)
    {
        enemyTimeSlider.value = amount;
    }

    public bool SetInspector(Creature creature)
    {
  
        //TODO:rework
        if (creature != null && InventoryManager.Instance.CreatureBattleSlots[CreatureBattleSlot.Attack] == null)
        {
            battleCreatureDetails.Reset();
            battleCreatureSprite.Reset();
            activeBattleCreatureButton.Reset();
            
            battleCreatureSprite.SetupRepresentation(creature);
            battleCreatureDetails.SetupRepresentation(creature);
            activeBattleCreatureButton.SetupRepresentation(InventoryManager.Instance.CreatureBattleSlots[CreatureBattleSlot.Attack]);
            _selectedCreature = creature;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Refresh()
    {
        battleCreatureDetails.Reset();
        battleCreatureSprite.Reset();
        activeBattleCreatureButton.Reset();
        
        if (_selectedCreature != null)
        {
            battleCreatureSprite.SetupRepresentation(_selectedCreature);
            battleCreatureDetails.SetupRepresentation(_selectedCreature);
            activeBattleCreatureButton.SetupRepresentation(InventoryManager.Instance.CreatureBattleSlots[CreatureBattleSlot.Attack]);
        }

        if (InventoryManager.Instance.CreatureBattleSlots[CreatureBattleSlot.Attack] != null)
        {
            battleCreatureSprite.SetupRepresentation(InventoryManager.Instance.CreatureBattleSlots[CreatureBattleSlot.Attack]);
            battleCreatureDetails.SetupRepresentation(InventoryManager.Instance.CreatureBattleSlots[CreatureBattleSlot.Attack]);
            activeBattleCreatureButton.SetupRepresentation(InventoryManager.Instance.CreatureBattleSlots[CreatureBattleSlot.Attack]);
        }
        
        
        UI_InventoryManager.Instance.RefreshInventory();
    }

   
    public void SetBattleCreature()
    {
        BattleManager.Instance.ResumeBattle();
        
        if (_selectedCreature != null && InventoryManager.Instance.CreatureBattleSlots[CreatureBattleSlot.Attack] == null)
        {
            
            InventoryManager.Instance.SelectCreatureForBattle(_selectedCreature, CreatureBattleSlot.Attack);
            //InventoryManager.Instance.ChoiceCreatureForBattle(_selectedCreature);

            if (_selectedCreature == InventoryManager.Instance.CreatureInspectorLeft)
            {
                InventoryManager.Instance.CreatureInspectorLeft = null;
                UI_CompareManager.Instance.SetInspector();
            }
            
            if (_selectedCreature == InventoryManager.Instance.CreatureInspectorRight)
            {
                InventoryManager.Instance.CreatureInspectorRight = null;
                UI_CompareManager.Instance.SetInspector();
            }
            
            activeBattleCreature.Creature = _selectedCreature;
            soundManager.PlaySFX("Click");
        }
        else
        {
            soundManager.PlaySFX("Error");
        }
        
        Refresh();
    }
    
    public void RetreatCreature()
    {
        
        if (_selectedCreature != null)
        {
            BattleManager.Instance.StopBattle();
            //TODO:rework
            InventoryManager.Instance.RetreatFormBattle(_selectedCreature, CreatureBattleSlot.Attack);
            activeBattleCreature.Creature = null;
            _selectedCreature = null;
            soundManager.PlaySFX("Click");
        }else if (InventoryManager.Instance.CreatureBattleSlots[CreatureBattleSlot.Attack] != null)
        {
            BattleManager.Instance.StopBattle();
            InventoryManager.Instance.RetreatFormBattle(InventoryManager.Instance.CreatureBattleSlots[CreatureBattleSlot.Attack], CreatureBattleSlot.Attack);
            activeBattleCreature.Creature = null;
            _selectedCreature = null;
            soundManager.PlaySFX("Click");
        }
        else
        {
            soundManager.PlaySFX("Error");
        }
        
        Refresh();
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
