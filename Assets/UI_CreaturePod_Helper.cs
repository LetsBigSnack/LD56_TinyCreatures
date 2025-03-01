using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CreaturePod_Helper : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool isLeft;

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
            UI_BreedingManager.Instance.SetPodActive(isLeft, uiCreatureButton.Creature);
            UI_InventoryManager.Instance.RefreshInventory();
        }
    }

}
