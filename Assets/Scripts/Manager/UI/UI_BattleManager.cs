using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_BattleManager : MonoBehaviour
{
    
    public static UI_BattleManager Instance;
    
    [SerializeField] private UI_CreatureSprite battleCreatureSprite;
    [SerializeField] private UI_CreatureDetailsText battleCreatureDetails;
    [SerializeField] private UI_CreatureSprite activeBattleCreatureButton;
    [SerializeField] private UICreatureButton activeBattleCreature;
    [SerializeField] private GameObject nextBattleButton;
    [SerializeField] private UI_ToggleButton toggleButton;

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
    
    
    public bool SetInspector(Creature creature)
    {
  
        
        if (creature != null && InventoryManager.Instance.SelectedCreatureForBattle == null)
        {
            battleCreatureDetails.Reset();
            battleCreatureSprite.Reset();
            activeBattleCreatureButton.Reset();
            
            battleCreatureSprite.SetupRepresentation(creature);
            battleCreatureDetails.SetupRepresentation(creature);
            activeBattleCreatureButton.SetupRepresentation(InventoryManager.Instance.SelectedCreatureForBattle);
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
            activeBattleCreatureButton.SetupRepresentation(InventoryManager.Instance.SelectedCreatureForBattle);
        }

        if (InventoryManager.Instance.SelectedCreatureForBattle != null)
        {
            battleCreatureSprite.SetupRepresentation(InventoryManager.Instance.SelectedCreatureForBattle);
            battleCreatureDetails.SetupRepresentation(InventoryManager.Instance.SelectedCreatureForBattle);
            activeBattleCreatureButton.SetupRepresentation(InventoryManager.Instance.SelectedCreatureForBattle);
        }
        
        
        UI_InventoryManager.Instance.RefreshInventory();
    }

    private void OnEnable()
    {
        Refresh();
        BattleManager.Instance.SetNextBattleButton();
        toggleButton.SetToggleState(BattleManager.Instance.AutoBattle);
    }

    public void SetBattleCreature()
    {
        BattleManager.Instance.ResumeBattle();
        
        if (_selectedCreature != null && InventoryManager.Instance.SelectedCreatureForBattle == null)
        {
            InventoryManager.Instance.ChoiceCreatureForBattle(_selectedCreature);

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
            InventoryManager.Instance.RetreatFormBattle(_selectedCreature);
            activeBattleCreature.Creature = null;
            _selectedCreature = null;
            soundManager.PlaySFX("Click");
        }else if (InventoryManager.Instance.SelectedCreatureForBattle != null)
        {
            BattleManager.Instance.StopBattle();
            InventoryManager.Instance.RetreatFormBattle(InventoryManager.Instance.SelectedCreatureForBattle);
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
        Debug.Log("Next button called" + nextBattleButton);
        nextBattleButton.SetActive(isActive);
    }
    
}
