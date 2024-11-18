using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SaveSlotHelper : MonoBehaviour
{
    public static UI_SaveSlotHelper Instance {  get; private set; }

    [SerializeField] private UI_Save_Slot slot1;
    [SerializeField] private UI_Save_Slot slot2;
    [SerializeField] private UI_Save_Slot slot3;
    private int currSlot;

    public int CurrSlot
    {
        get => currSlot;
        set => currSlot = value;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        };
    }

    public UI_Save_Slot SelectSlot()
    {
        switch (currSlot)
        {
            case 0:
                return slot1;
            case 1:
                return slot2;
            case 2:
                return slot3;
            default:
                return null;
        }
    }
}
