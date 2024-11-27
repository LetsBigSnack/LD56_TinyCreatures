using System.Collections;
using System.Collections.Generic;
using Data;
using Manager;
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
        _tutorialData.ConfigDone = false;
    }

    public bool CheckBool(StringState boolToCheck)
    {
        bool returnCase = false;

        switch (boolToCheck)
        {
            case StringState.Entry:
                returnCase = _tutorialData.EntryDone;
                break;
            case StringState.Battle:
                returnCase = _tutorialData.BattleDone;
                break;
            case StringState.Inspector:
                returnCase = _tutorialData.InspectorDone;
                break;
            case StringState.Fusion:
                returnCase = _tutorialData.FusionDone;
                break;
            case StringState.Shop:
                returnCase = _tutorialData.ShopDone;
                break;
            case StringState.Config:
                returnCase = _tutorialData.ConfigDone;
                break;
        }
        return returnCase;
    }

    public void SetBool(StringState boolToCheck)
    {
        switch (boolToCheck)
        {
            case StringState.Entry:
                _tutorialData.EntryDone = true;
                break;
            case StringState.Battle:
                _tutorialData.BattleDone = true;
                break;
            case StringState.Inspector:
                _tutorialData.InspectorDone = true;
                break;
            case StringState.Fusion:
                _tutorialData.FusionDone = true;
                break;
            case StringState.Shop:
                _tutorialData.ShopDone = true;
                break;
            case StringState.Config:
                _tutorialData.ConfigDone = true;
                break;
        }
    }
}
