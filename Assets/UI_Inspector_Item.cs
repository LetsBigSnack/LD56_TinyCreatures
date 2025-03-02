using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UI_Inspector_Item : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool isLeft;
    [SerializeField] private UICreatureButton creatureButton;

    [SerializeField] private GameObject activeItem;
    [SerializeField] private GameObject deactiveItem;

    private void OnEnable()
    {
        if (isLeft)
        {
            InventoryManager.OnChangesCreatureInspectorLeft += UpdateInspectorItem;
            UpdateInspectorItem(InventoryManager.Instance.CreatureInspectorLeft);
        }
        else
        {
            InventoryManager.OnChangesCreatureInspectorRight += UpdateInspectorItem;
            UpdateInspectorItem(InventoryManager.Instance.CreatureInspectorRight);
        }   
    }

    private void OnDisable()
    {
        if (isLeft)
        {
            InventoryManager.OnChangesCreatureInspectorLeft -= UpdateInspectorItem;
        }
        else
        {
            InventoryManager.OnChangesCreatureInspectorRight -= UpdateInspectorItem;
        }
    }

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
        SoundManager.Instance.PlaySFX("Drop");
    }

    public void UpdateInspectorItem(Creature creature)
    {
        if(creature == null)
        {
            activeItem.SetActive(false);
            deactiveItem.SetActive(true);
            return;
        }

        activeItem.SetActive(true);
        deactiveItem.SetActive(false);
    }
}
