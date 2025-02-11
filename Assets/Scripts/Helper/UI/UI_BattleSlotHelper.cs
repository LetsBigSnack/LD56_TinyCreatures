using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BattleSlotHelper : MonoBehaviour, IDropHandler
{
    [SerializeField] private GameObject activeSlot;
    [SerializeField] private GameObject deactiveSlot;
    [SerializeField] private CreatureBattleSlot slotType;

    private void OnEnable()
    {
        InitialSetup();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (BattleManager.Instance.IsBattleRunning)
        {
            SoundManager.Instance.PlaySFX("Error");
            UI_ToastManager.Instance.CreateToast("Battle Ongoing!", "Can't add/change creature during battle.");
            return;
        }
        Creature creature = InventoryManager.Instance.CreatureBattleSlots[slotType];
        if (creature == null)
        {
            SetFull();
            activeSlot.GetComponentInChildren<UI_BattleCreatureItem>().OnDrop(eventData);
            return;
        }

        activeSlot.GetComponentInChildren<UI_BattleCreatureItem>().OnDrop(eventData);
    }

    public void InitialSetup()
    {
        Creature creature = InventoryManager.Instance.CreatureBattleSlots[slotType];
        if (creature == null)
        {
            SetEmpty();
            return;
        }
        SetFull();
    }

    public void SetEmpty() 
    {
        deactiveSlot.SetActive(true);
        activeSlot.SetActive(false);
    }
    public void SetFull()
    {
        deactiveSlot.SetActive(false);
        activeSlot.SetActive(true);
    }
}
