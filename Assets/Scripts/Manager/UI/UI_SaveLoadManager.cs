using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_SaveLoadManager : MonoBehaviour
{
    
    public static UI_SaveLoadManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;

        }
    }
    
    [SerializeField] private UI_Save_Slot slot1;
    [SerializeField] private UI_Save_Slot slot2;
    [SerializeField] private UI_Save_Slot slot3;
    [SerializeField] private TextMeshProUGUI selectedSlotText;
    [SerializeField] private GameObject selectScreen;
    
    private void Start()
    {
        SetupRepresentation();
    }
    
    public void SetupRepresentation()
    {
        
        
        slot1.Reset();
        slot2.Reset();
        slot3.Reset();
        
        slot1.SetupRepresentation(SaveLoadManager.Instance.SaveSlot1);
        slot2.SetupRepresentation(SaveLoadManager.Instance.SaveSlot2);
        slot3.SetupRepresentation(SaveLoadManager.Instance.SaveSlot3);

        if (SaveLoadManager.Instance.SaveIndex != -1)
        {
            SetSelectedSlot(SaveLoadManager.Instance.SaveIndex);
        }
    }

    public void SetSelectedSlot(int slot)
    {
        selectedSlotText.text = slot.ToString();
    }

    public void SetActiveSelectScreen(bool active)
    {
        selectScreen.SetActive(active);
    }
    
}
