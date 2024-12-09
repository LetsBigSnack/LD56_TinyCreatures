using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICreatureButton : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] public Creature creature;
    [SerializeField] public bool isHoverable;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Transform parentAfterDrag;
    public bool isDragable = true;

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

    public bool IsDragable
    {
        get => isDragable;
        set => isDragable = value;
    }

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = FindObjectOfType<UI_MainCanvasManager>().GetComponent<Canvas>();
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isDragable)
        {
            canvasGroup.blocksRaycasts = false;
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (UI_ToggleManager.Instance.CurrentState == ToggleState.Materials && isDragable)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            canvasGroup.alpha = 0.6f;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragable)
        {
            canvasGroup.alpha = 1f;
            transform.SetParent(parentAfterDrag);
            canvasGroup.blocksRaycasts = true;
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
