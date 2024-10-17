using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [SerializeField]
    bool entryDone = false;
    [SerializeField]
    bool battleDone = false;
    [SerializeField]
    bool inspectorDone = false;
    [SerializeField]
    bool fusionDone = false;
    [SerializeField]
    bool shopDone = false;

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
        entryDone = false;
        battleDone = false;
        inspectorDone = false;
        fusionDone = false;
        shopDone = false;
    }

    public bool CheckBool(string boolToCheck)
    {
        bool returnCase = false;

        switch (boolToCheck)
        {
            case "Entry":
                returnCase = entryDone;
                break;
            case "Battle":
                returnCase = battleDone;
                break;
            case "Inspector":
                returnCase = inspectorDone;
                break;
            case "Fusion":
                returnCase = fusionDone;
                break;
            case "Shop":
                returnCase = shopDone;
                break;
        }
        return returnCase;
    }

    public void SetBool(string boolToCheck)
    {
        switch (boolToCheck)
        {
            case "Entry":
                entryDone = true;
                break;
            case "Battle":
                battleDone = true;
                break;
            case "Inspector":
                inspectorDone = true;
                break;
            case "Fusion":
                fusionDone = true;
                break;
            case "Shop":
                shopDone = true;
                break;
        }
    }
}
