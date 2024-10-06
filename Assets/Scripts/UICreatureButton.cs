using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICreatureButton : MonoBehaviour, IPointerClickHandler
{
    
    [SerializeField] private Creature creature;
    
    public Creature Creature
    {
        get => creature;
        set => creature = value;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {

        if (UI_ToggleManager.Instance.CurrentState == ToggleState.Inspector)
        {
            // Detect left click
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                InventoryManager.Instance.SelectCreatureLeft(creature);
            }
            // Detect right click
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                InventoryManager.Instance.SelectCreatureRight(creature);
            }
            UI_CompareManager.Instance.SetInspector();
            UI_InventoryManager.Instance.RefreshInventory();
        }

        if (UI_ToggleManager.Instance.CurrentState == ToggleState.Battle)
        {
            UI_BattleManager.Instance.SetInspector(creature);
        }
        
    }
    
}
