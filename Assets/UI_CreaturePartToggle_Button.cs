using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CreaturePartToggle_Button : MonoBehaviour
{
    [SerializeField] private BodyPartToggleTypes toggleType;

    public void ToggleAction()
    {
        if(UI_CreatureReconfigureManager.Instance.CurrentToggle != toggleType)
        {
            UI_CreatureReconfigureManager.Instance.ToggleBodyParts(toggleType);
        }
    }
}
