using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICreatureButton : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private Creature creature;
    [SerializeField] private bool isHoverable;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Transform parentAfterDrag;
    [SerializeField] private bool isDragable = true;
        
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
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = FindObjectOfType<UI_MainCanvasManager>().GetComponent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isDragable)
        {
            SoundManager.Instance.PlaySFX("Drag");
            canvasGroup.blocksRaycasts = false;
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isDragable)
            {
                rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
                canvasGroup.alpha = 0.6f;
            }
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
