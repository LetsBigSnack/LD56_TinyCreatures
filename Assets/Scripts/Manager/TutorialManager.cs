using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }
    private TutorialData _tutorialData;

    public TutorialData TutorialData
    {
        get => _tutorialData;
        set => _tutorialData = value;
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
            DontDestroyOnLoad(gameObject);
        }

    }

    public void resetTutorial()
    {
        _tutorialData.EntryDone = false;
        _tutorialData.BattleDone = false;
        _tutorialData.InspectorDone = false;
        _tutorialData.FusionDone = false;
        _tutorialData.ShopDone = false;
    }

    public bool CheckBool(string boolToCheck)
    {
        bool returnCase = false;

        switch (boolToCheck)
        {
            case "Entry":
                returnCase = _tutorialData.EntryDone;
                break;
            case "Battle":
                returnCase = _tutorialData.BattleDone;
                break;
            case "Inspector":
                returnCase = _tutorialData.InspectorDone;
                break;
            case "Fusion":
                returnCase = _tutorialData.FusionDone;
                break;
            case "Shop":
                returnCase = _tutorialData.ShopDone;
                break;
        }
        return returnCase;
    }

    public void SetBool(string boolToCheck)
    {
        switch (boolToCheck)
        {
            case "Entry":
                _tutorialData.EntryDone = true;
                break;
            case "Battle":
                _tutorialData.BattleDone = true;
                break;
            case "Inspector":
                _tutorialData.InspectorDone = true;
                break;
            case "Fusion":
                _tutorialData.FusionDone = true;
                break;
            case "Shop":
                _tutorialData.ShopDone = true;
                break;
        }
    }
}
