using System.Collections;
using System.Collections.Generic;
using Manager;
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
        popupManager.ViewPopup(StringState.Settings);
    }
}
