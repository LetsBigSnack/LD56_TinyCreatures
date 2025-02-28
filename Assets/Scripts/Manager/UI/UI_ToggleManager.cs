using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public enum ToggleState
{
    Shop,
    Battle,
    Inspector,
    Fusion,
    Materials,
    ReConfigure,
    Achievements,
    CreatureDex
}

public class UI_ToggleManager : MonoBehaviour
{
    public static UI_ToggleManager Instance;
    
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject battle;
    [SerializeField] private GameObject inspector;
    [SerializeField] private GameObject fuse;
    [SerializeField] private GameObject materials;
    [SerializeField] private GameObject reconfigure;
    [SerializeField] private GameObject achievements;
    [SerializeField] private GameObject creatureDex;
    
    [SerializeField] private Sprite clickedSprite;
    [SerializeField] private Sprite notClickedSprite;
    [SerializeField] private List<GameObject> tabButtons;

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
        fuse.SetActive(false);
        inspector.SetActive(false);
        battle.SetActive(false);
        shop.SetActive(false);
        materials.SetActive(false);
        reconfigure.SetActive(false);
        achievements.SetActive(false);
        creatureDex.SetActive(false);
        soundManager.PlaySFX("Click");
        UpdateButtonSprite(state);
        switch (state)
        {
            case "Shop":  
                shop.SetActive(true);
                currentState = ToggleState.Shop;
                popupManager.ViewPopup(StringState.Shop);
                break;
            case "Battle":
                battle.SetActive(true);
                currentState = ToggleState.Battle;
                break;
            case "Inspector":
                inspector.SetActive(true);
                currentState = ToggleState.Inspector;
                popupManager.ViewPopup(StringState.Inspector);
                break;
            case "Fusion":
                fuse.SetActive(true);
                currentState = ToggleState.Fusion;
                popupManager.ViewPopup(StringState.Fusion);
                break;
            case "Materials":
                materials.SetActive(true);
                currentState = ToggleState.Materials;
                //popupManager.ViewPopup(state);
                break;
            case "Reconfigure":
                reconfigure.SetActive(true);
                currentState = ToggleState.ReConfigure;
                popupManager.ViewPopup(StringState.Config);
                break;
            case "Achievements":
                achievements.SetActive(true);
                currentState = ToggleState.Achievements;
                break;
            case "CreatureDex":
                creatureDex.SetActive(true);
                currentState = ToggleState.CreatureDex;
                break;

        }
    }

    private void UpdateButtonSprite(string currentState)
    {
        for (int i = 0; i < tabButtons.Count; i++)
        {
            if (tabButtons[i].activeSelf)
            {
                var buttonSprite = tabButtons[i].GetComponent<Image>();
                var buttonName = tabButtons[i].name;
                if (buttonName.Contains(currentState))
                {
                    buttonSprite.sprite = clickedSprite;
                }
                else
                {
                    buttonSprite.sprite = notClickedSprite;
                }
            }
        }
    }
    
}
