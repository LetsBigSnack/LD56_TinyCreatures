using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[Serializable]
public enum CreatureBattleSlot
{
    Attack,
    Heal,
    Defense,
    Enemy
}

public class UI_BattleCreatureItem : MonoBehaviour, IDropHandler
{
    private Creature currentCreature;
    [SerializeField] private UICreatureButton creatureButton;
    [SerializeField] private UI_CreatureSprite creatureSprite;
    [SerializeField] private CreatureBattleSlot creatureBattleSlot;
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider timeSlider;
    [SerializeField] private Slider shieldSlider;

    [SerializeField] private Button retreatButton;

    public Button RetreatButton
    {
        get { return retreatButton; }
        set { retreatButton = value; }
    }
    
    public Creature CurrentCreature
    {
        get { return currentCreature; }
        set { currentCreature = value; }
    }

    public void Start()
    {
        if (InventoryManager.Instance.CreatureBattleSlots[creatureBattleSlot] != null)
        {
            SetCreatureRepresentation(InventoryManager.Instance.CreatureBattleSlots[creatureBattleSlot]);
        }
        else
        {
            retreatButton.interactable = false;
        }
        
    }

    public void UpdateHealthSlider(float amount)
    {
        healthSlider.value = amount;
    }

    public void UpdateTimeSlider(float amount)
    {
        timeSlider.value = amount;
    }

    public void UpdateShieldSlider(float amount)
    {
        shieldSlider.value = amount;
    }

    public void RetreatCreature()
    {
        if (BattleManager.Instance.IsBattleRunning)
        {
            UI_ToastManager.Instance.CreateToast("Battle Ongoing!", "Can't remove a creature during battle!");
            SoundManager.Instance.PlaySFX("Error");
            return;
        }

        if (BattleManager.Instance.RetreatCreature(creatureBattleSlot))
        {
            currentCreature = null;
            ResetCreatureRepresentation();
            UI_BattleManager.Instance.OnCreatureRemoved(creatureBattleSlot);
            SoundManager.Instance.PlaySFX("Click");
            retreatButton.interactable = false;
        }
        else
        {
            UI_ToastManager.Instance.CreateToast("Can't remove creature", "There is no space in the inventory.");
            SoundManager.Instance.PlaySFX("Error");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (BattleManager.Instance.IsBattleRunning)
        {
            UI_ToastManager.Instance.CreateToast("Battle Ongoing!", "Can't add a creature during battle!");
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
            OnCreatureAdded(uiCreatureButton.Creature);
            SoundManager.Instance.PlaySFX("Click");
        }
        UI_InventoryManager.Instance.RefreshInventory();
    }

    public void OnHover()
    {
        if(currentCreature == null)
        {
            return;
        }
        UI_BattleManager.Instance.OnHoverBattleCreature(creatureBattleSlot);
    }

    public void OffHover()
    {
        UI_BattleManager.Instance.OffHoverBattleCreature();
    }

    public void OnCreatureAdded(Creature creature)
    {        
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
        UI_ToastManager.Instance.CreateToast("No Creature!", "There's no creature in this slot!");
        SoundManager.Instance.PlaySFX("Error");
    }

    public void ResetCreatureRepresentation()
    {
        creatureSprite.Reset();
    }

    public void SetCreatureRepresentation(Creature creature)
    {
        nameText.text = creature.CreatureName;
        retreatButton.interactable = true;
        creatureButton.Creature = creature;
        creatureSprite.SetupRepresentation(creature);
        currentCreature = creature;
    }
}
