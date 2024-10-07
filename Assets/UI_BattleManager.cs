using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BattleManager : MonoBehaviour
{
    
    public static UI_BattleManager Instance;
    
    [SerializeField] private UI_CreatureSprite battleCreatureSprite;
    [SerializeField] private UI_CreatureDetailsText battleCreatureDetails;
    [SerializeField] private UI_CreatureSprite activeBattleCreatureButton;
    [SerializeField] private UICreatureButton activeBattleCreature;

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
        
        UI_InventoryManager.Instance.RefreshInventory();
    }
    
    
    public void SetBattleCreature()
    {
        BattleManager.Instance.ResumeBattle();
        
        if (_selectedCreature != null)
        {
            InventoryManager.Instance.ChoiceCreatureForBattle(_selectedCreature);
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
        }
        else
        {
            soundManager.PlaySFX("Error");
        }
        
        Refresh();
    }
    
}
