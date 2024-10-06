using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToggleState
{
    Shop,
    Battle,
    Inspector
}

public class UI_ToggleManager : MonoBehaviour
{
    public static UI_ToggleManager Instance;
    
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject battle;
    [SerializeField] private GameObject inspector;
    
    [SerializeField] private ToggleState currentState = ToggleState.Battle;
    
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
            SwitchState("Battle");
            DontDestroyOnLoad(gameObject);
        }
    }
    
    
    
    
    public void SwitchState(string state)
    {

        switch (state)
        {
            case "Shop":
                shop.SetActive(true);
                battle.SetActive(false);
                inspector.SetActive(false);
                currentState = ToggleState.Shop;
                break;
            case "Battle":
                battle.SetActive(true);
                shop.SetActive(false);
                inspector.SetActive(false);
                currentState = ToggleState.Battle;
                break;
            case "Inspector":
                inspector.SetActive(true);
                battle.SetActive(false);
                shop.SetActive(false);
                currentState = ToggleState.Inspector;
                break;
        }
        
    }
    
}
