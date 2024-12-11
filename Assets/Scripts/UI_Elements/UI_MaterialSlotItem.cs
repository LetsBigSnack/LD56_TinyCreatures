using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//TODO: refactor as soon as theme has been set
public enum MaterialType
{
    MaterialA,
    MaterialB,
    MaterialC,
    MaterialD
}

public class UI_MaterialSlotItem : MonoBehaviour, IDropHandler
{
    private Creature currentCreature;
    [SerializeField] private UICreatureButton creatureButton;
    [SerializeField] private UI_CreatureSprite creatureSprite;
    [SerializeField] private Image miningAnimObject;
    [SerializeField] private MaterialType selectedMaterial;

    public void Awake()
    {
        creatureButton = GetComponentInChildren<UICreatureButton>();
        creatureSprite = GetComponentInChildren<UI_CreatureSprite>();
        ResetCreatureRepresentation();
        SetMiningAnimation();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            UICreatureButton uiCreatureButton = eventData.pointerDrag.GetComponent<UICreatureButton>();
            if(uiCreatureButton != null && !uiCreatureButton.IsDragable)
            {
                SoundManager.Instance.PlaySFX("Error");
                return;
            }
            SetNewCreature(uiCreatureButton.Creature);
            SoundManager.Instance.PlaySFX("Click");
        }
    }

    public void SetMiningAnimation()
    {
        if(currentCreature != null && !currentCreature.IsNull())
        {
            miningAnimObject.color = new Color(255,255,255,1);
            return;
        }
        miningAnimObject.color = new Color(255, 255, 255, 0);
    }

    public void SetNewCreature(Creature creature)
    {
        if(currentCreature != null)
        {
            Withdraw(true);
        }
        InventoryManager.Instance.AddCreatureToMaterialSlot(creature, selectedMaterial);
        StartFarming();
        SetCreatureRepresentation(creature);
        SetMiningAnimation();
    }

    private void SetCreatureRepresentation(Creature creature)
    {
        creatureButton.Creature = creature;
        creatureSprite.SetupRepresentation(creature);
        currentCreature = creature;
    }

    private void ResetCreatureRepresentation()
    {
        creatureSprite.Reset();
    }

    public void StartFarming()
    {
       //Start Coroutine in the Materials Backend
    }

    //ButtonAction
    public void Withdraw(bool isExchanged = false)
    {
        if (currentCreature != null)
        {
            InventoryManager.Instance.RemoveCreatureFromMaterialSlot(selectedMaterial);
            ResetCreatureRepresentation();
            creatureButton.Creature = null;
            currentCreature = null;
            SetMiningAnimation();
            if (!isExchanged)
            {
                SoundManager.Instance.PlaySFX("Click");
            }
            return;
        }

        SoundManager.Instance.PlaySFX("Error");
    }
}
