using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Save_Slot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI playtimeText;
    [SerializeField] private TextMeshProUGUI achievmentText;

    // Start is called before the first frame update
    void Start()
    {
        //load stuff from SaveLoadManager to get all text necessary
    }

    public void SetSlot()
    {
        //Set text here
    }

    public void OnSelectSlot()
    {
        //check if the slot has something stored
        //Y: accept input and close menu
        //N: openpopup to name it
        //after submit change to slot
        //close menu
    }

    public void RenameSlot(string text)
    {
        nameText.text = text;
    }

    public void DeleteSlot()
    {
        //delete slot here
    }

}
