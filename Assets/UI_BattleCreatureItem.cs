using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public void OnDrop(PointerEventData eventData)
    {
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
    }
    public void SetNewCreature(Creature creature)
    {
        if (currentCreature != null)
        {
            Withdraw(true);
        }
        //InventoryManager.Instance.AddCreatureToMaterialSlot(creature, selectedMaterial);
        SetCreatureRepresentation(creature);
    }

    public void Withdraw(bool isExchanged = false)
    {
        if (currentCreature != null)
        {
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
