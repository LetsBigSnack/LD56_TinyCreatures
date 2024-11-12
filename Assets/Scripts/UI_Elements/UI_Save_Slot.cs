using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Save_Slot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI playtimeText;
    [SerializeField] private TextMeshProUGUI achievmentText;
    [SerializeField] private TextMeshProUGUI slotText;
    [SerializeField] private int slotNumber;

    
    public void Reset()
    {
        slotText.text = slotNumber.ToString();
        nameText.text = "";
        playtimeText.text = "";
        achievmentText.text = "";
    }
    
    public void SetupRepresentation(SaveState saveState)
    {
        if (saveState == null)
        {
            return;
        }
        nameText.text = saveState.saveName;
        //playtimeText.text = "";
        //achievmentText.text = "";
        
    }
    
    public void OnSelectSlot()
    {
        SaveLoadManager.Instance.SelectSlot(slotNumber);
    }

    public void RenameSlot(string text)
    {
        nameText.text = text;
    }

    public void DeleteSlot()
    {
        //TODO: add Confirm PopUp
        SaveLoadManager.Instance.DeleteSlot(slotNumber);
        UI_SaveLoadManager.Instance.SetupRepresentation();
    }
}
