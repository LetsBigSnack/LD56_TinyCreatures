using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Manager;
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
        bool notCreated = !SaveLoadManager.Instance.SaveStateExists(slotNumber);
        if (notCreated)
        {
            UI_SaveSlotHelper.Instance.CurrSlot = slotNumber;
            PopupManager.Instance.ViewPopup(StringState.Input);
        }
        else
        {
            SaveLoadManager.Instance.SelectSlot(slotNumber);
            UI_SaveLoadManager.Instance.SetActiveSelectScreen(false);
        }
    }

    public bool OnConfirmSlot() 
    {
        string newName = PopupManager.Instance.InputName;
        bool notCreated = !SaveLoadManager.Instance.SaveStateExists(slotNumber);
        
        if(!string.IsNullOrEmpty(newName)) 
        {
            if (notCreated)
            {
                SaveLoadManager.Instance.SelectSlot(slotNumber);
            }
            SaveLoadManager.Instance.RenameSaveSlot(newName, slotNumber);
            SoundManager.Instance.PlaySFX("Click");
            UI_SaveLoadManager.Instance.SetupRepresentation();
            return true;
        }
        SoundManager.Instance.PlaySFX("Error");
        return false;
    }

    public void RenameSlot()
    {
        SoundManager.Instance.PlaySFX("Click");
        UI_SaveSlotHelper.Instance.CurrSlot = slotNumber;
        PopupManager.Instance.SetInputText(nameText.text);
        PopupManager.Instance.ViewPopup(StringState.Input);
    }


    public void OnDeselectSlot()
    {
        UI_SaveSlotHelper.Instance.CurrSlot = slotNumber;
        PopupManager.Instance.ViewPopup(StringState.Delete);
    }
    
    
    public void DeleteSlot()
    {
        SaveLoadManager.Instance.DeleteSlot(slotNumber);
        UI_SaveLoadManager.Instance.SetupRepresentation();
    }
}
