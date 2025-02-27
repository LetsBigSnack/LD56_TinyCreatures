using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Inspector_Item : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool isLeft;
    [SerializeField] private UICreatureButton creatureButton;
    public void OnDrop(PointerEventData eventData)
    {
        UICreatureButton uiCreatureButton = eventData.pointerDrag.GetComponent<UICreatureButton>();
        if (uiCreatureButton == null || !uiCreatureButton.IsDragable || uiCreatureButton.Creature == null)
        {
            SoundManager.Instance.PlaySFX("Error");
            return;
        }

        if (isLeft)
        {
            InventoryManager.Instance.SelectCreatureLeft(uiCreatureButton.Creature);
        }
        else
        {
            InventoryManager.Instance.SelectCreatureRight(uiCreatureButton.Creature);
        }
        UI_CompareManager.Instance.SetInspector();
        UI_InventoryManager.Instance.RefreshInventory();
        SoundManager.Instance.PlaySFX("Click");
    }
}
