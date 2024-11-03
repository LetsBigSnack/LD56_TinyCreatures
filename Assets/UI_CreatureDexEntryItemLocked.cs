using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_CreatureDexEntryItemLocked : MonoBehaviour
{
    [SerializeField] private BodyPartSet bodyPartSet;

    [SerializeField] private TextMeshProUGUI descriptionText;


    public BodyPartSet BodySet
    {

        get => bodyPartSet;
        set => bodyPartSet = value;

    }

    void Start()
    {
        descriptionText.text = bodyPartSet.setText;
    }
    
    public void UnlockSet()
    {
        bodyPartSet.unlocked = true;
        UI_CreatureDexManager.Instance.UnlockEntry(bodyPartSet);
    }
}
