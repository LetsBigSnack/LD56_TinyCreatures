using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToggleState
{
    Shop,
    Battle,
    Inspector,
    Fusion
}

public class UI_ToggleManager : MonoBehaviour
{
    public static UI_ToggleManager Instance;
    
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject battle;
    [SerializeField] private GameObject inspector;
    [SerializeField] private GameObject fuse;

    [SerializeField] private ToggleState currentState = ToggleState.Battle;

    private SoundManager soundManager;
    private PopupManager popupManager;

    public ToggleState CurrentState { get => currentState; set => currentState = value; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        soundManager = FindObjectOfType<SoundManager>();
        popupManager = FindObjectOfType<PopupManager>();
        SwitchState("Battle");
    }

    public void SwitchState(string state)
    {  
        soundManager.PlaySFX("Click");

        switch (state)
        {
            case "Shop":  
                shop.SetActive(true);
                battle.SetActive(false);
                inspector.SetActive(false);
                fuse.SetActive(false);
                currentState = ToggleState.Shop;
                popupManager.ViewPopup(state);
                break;
            case "Battle":
                battle.SetActive(true);
                shop.SetActive(false);
                inspector.SetActive(false);
                fuse.SetActive(false);
                currentState = ToggleState.Battle;
                break;
            case "Inspector":
                inspector.SetActive(true);
                battle.SetActive(false);
                shop.SetActive(false);
                fuse.SetActive(false);
                currentState = ToggleState.Inspector;
                popupManager.ViewPopup(state);
                break;
            case "Fusion":
                fuse.SetActive(true);
                inspector.SetActive(false);
                battle.SetActive(false);
                shop.SetActive(false);
                currentState = ToggleState.Fusion;
                popupManager.ViewPopup(state);
                break;
        }
        
    }
    
}
