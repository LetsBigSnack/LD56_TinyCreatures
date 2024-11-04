using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICreatureButton : MonoBehaviour, IPointerClickHandler
{
    
    [SerializeField] public Creature creature;
    [SerializeField] public bool isHoverable;

    private SoundManager soundManager;
        
    public Creature Creature
    {
        get => creature;
        set => creature = value;
    }

    public bool IsHoverable
    {
        get => isHoverable;
        set => isHoverable = value;
    }

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
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
            soundManager.PlaySFX("Click");

        }

        if (UI_ToggleManager.Instance.CurrentState == ToggleState.Battle)
        {
            if (UI_BattleManager.Instance.SetInspector(creature))
            {
                soundManager.PlaySFX("Click");
            }
            else
            {
                soundManager.PlaySFX("Error");
            }
        }

        if(UI_ToggleManager.Instance.CurrentState == ToggleState.ReConfigure)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                InventoryManager.Instance.AddToReconfigure(creature);
            }
            UI_InventoryManager.Instance.RefreshInventory();
            soundManager.PlaySFX("Click");
        }
    }

    public void OnHover()
    {
        if (isHoverable)
        {
            UI_InventoryHoverManager.Instance.SetDetails(creature);
        }
    }

    public void OffHover()
    {
        if (isHoverable)
        {
            UI_InventoryHoverManager.Instance.ResetDetails();
        }
    }
    
}
