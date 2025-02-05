using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CreaturePartToggle_Button : MonoBehaviour
{
    [SerializeField] private BodyPartToggleTypes toggleType;

    public void ToggleAction()
    {
        //UI_CreatureReconfigureManager.Instance.ToggleBodyParts();
    }
}
