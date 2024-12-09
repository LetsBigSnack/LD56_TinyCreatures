using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MaterialSlotItem : MonoBehaviour, IDropHandler
{
    [SerializeField] private UICreatureButton creatureButton;
    [SerializeField] private UI_CreatureSprite creatureSprite;
    [SerializeField] private Creature currentCreature;
    [SerializeField] private Image miningAnimObject;

    public void Awake()
    {
        creatureButton = GetComponentInChildren<UICreatureButton>();
        creatureSprite = GetComponentInChildren<UI_CreatureSprite>();
        ResetCreatureRepresentation();
        SetMiningAnimation();
    }

    public void OnDrop(PointerEventData eventData)
    {
        SetNewCreature(eventData.pointerDrag.GetComponentInChildren<UICreatureButton>().Creature);
        SoundManager.Instance.PlaySFX("Click");
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
        if(currentCreature != null && !currentCreature.IsNull())
        {
            Withdraw(true);
        }

        InventoryManager.Instance.RemoveCreature(creature);
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
        if (currentCreature != null && !currentCreature.IsNull())
        {
            InventoryManager.Instance.AddCreature(currentCreature);
            //stop current coroutine of the material
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
