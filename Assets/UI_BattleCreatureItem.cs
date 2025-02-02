using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public enum CreatureBattleSlot
{
    Attack,
    Heal,
    Defense
}

public class UI_BattleCreatureItem : MonoBehaviour, IDropHandler
{
    private Creature currentCreature;
    [SerializeField] private UICreatureButton creatureButton;
    [SerializeField] private UI_CreatureSprite creatureSprite;
    [SerializeField] private CreatureBattleSlot creatureBattleSlot;


    public void Start()
    {
        if (InventoryManager.Instance.CreatureBattleSlots[creatureBattleSlot] != null)
        {
            SetCreatureRepresentation(InventoryManager.Instance.CreatureBattleSlots[creatureBattleSlot]);
        }
    }


    public void OnDrop(PointerEventData eventData)
    {
        if (BattleManager.Instance.IsBattleRunning)
        {
            //TODO: PopUp cant add while Battle is ongoing or something like that
            SoundManager.Instance.PlaySFX("Error");
            return;
        }
        
        if (eventData.pointerDrag != null)
        {
            UICreatureButton uiCreatureButton = eventData.pointerDrag.GetComponent<UICreatureButton>();
            if (uiCreatureButton == null || !uiCreatureButton.IsDragable || uiCreatureButton.Creature == null)
            {
                SoundManager.Instance.PlaySFX("Error");
                return;
            }
            SetNewCreature(uiCreatureButton.Creature);
            SoundManager.Instance.PlaySFX("Click");
        }
        UI_InventoryManager.Instance.RefreshInventory();
    }
    public void SetNewCreature(Creature creature)
    {
        //only accept when battle is not running
        if (BattleManager.Instance.IsBattleRunning)
        {
            SoundManager.Instance.PlaySFX("Error");
            return;
        }
        
        if (currentCreature != null)
        {
            Withdraw(true);
        }
        
        
        
        InventoryManager.Instance.SelectCreatureForBattle(creature, creatureBattleSlot);
        SetCreatureRepresentation(creature);
    }

    public void Withdraw(bool isExchanged = false)
    {
        if (currentCreature != null)
        {
            InventoryManager.Instance.RetreatFormBattle(currentCreature, creatureBattleSlot);
            ResetCreatureRepresentation();
            creatureButton.Creature = null;
            currentCreature = null;
            if (!isExchanged)
            {
                SoundManager.Instance.PlaySFX("Click");
            }
            return;
        }

        SoundManager.Instance.PlaySFX("Error");
    }

    private void ResetCreatureRepresentation()
    {
        creatureSprite.Reset();
    }

    private void SetCreatureRepresentation(Creature creature)
    {
        creatureButton.Creature = creature;
        creatureSprite.SetupRepresentation(creature);
        currentCreature = creature;
    }
}
