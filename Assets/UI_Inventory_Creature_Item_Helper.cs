using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inventory_Creature_Item_Helper : MonoBehaviour, IPointerClickHandler 
{
    [SerializeField] private GameObject sellButton;
    [SerializeField] private UICreatureButton creatureButton;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked_regisered");
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("click_right_registered");
            sellButton.SetActive(true);
        }
    }
    public void SellCreature()
    {
        StoreManager.Instance.SellOwnedCreature(creatureButton.Creature);
        InventoryManager.Instance.RemoveCreature(creatureButton.Creature);
        if(UI_ToggleManager.Instance.CurrentState == ToggleState.Inspector)
        {
            UI_CompareManager.Instance.SetInspector();
        }
        SoundManager.Instance.PlaySFX("Transaction");
        UI_InventoryManager.Instance.RefreshInventory();
    }
}
