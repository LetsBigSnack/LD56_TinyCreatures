using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupHelper : MonoBehaviour
{
    private PopupManager popupManager;

    private void Awake()
    {
        popupManager = FindObjectOfType<PopupManager>();
    }

    public void SettingsBtnPressed()
    {
        popupManager.ViewPopup("Settings");
    }
}
