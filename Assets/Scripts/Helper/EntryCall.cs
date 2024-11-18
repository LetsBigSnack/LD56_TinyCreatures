using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class EntryCall : MonoBehaviour
{
    private PopupManager popupManager;
    private void Start()
    {
        popupManager = FindObjectOfType<PopupManager>();
        popupManager.ViewPopup(StringState.Entry);
    }
}
