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

    private Creature _selectedCreature;
    
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void SetInspector(Creature creature)
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
        if (_selectedCreature != null)
        {
            InventoryManager.Instance.ChoiceCreatureForBattle(_selectedCreature);
            activeBattleCreature.Creature = _selectedCreature;
        }
        
        Refresh();
    }
    
    public void RetreatCreature()
    {
        if (_selectedCreature != null)
        {
            InventoryManager.Instance.RetreatFormBattle(_selectedCreature);
            activeBattleCreature.Creature = null;
        }
        
        Refresh();
    }
    
}
