using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Data;

public class UI_FuseManager : MonoBehaviour
{
    [SerializeField] private UICreatureButton creatureButton;
    [SerializeField] private UI_CreatureSprite fuseSprite;
    [SerializeField] private UI_CreatureDetailsText fuseDetailsText;

    [SerializeField] private GameObject activeTop;
    [SerializeField] private GameObject inactiveTop;

    [SerializeField] private GameObject activeMid;
    [SerializeField] private GameObject inactiveMid;

    [SerializeField] private GameObject activeBottom;
    [SerializeField] private GameObject inactiveBottom;

    [SerializeField] private TextMeshProUGUI reducedCostText;


    private void OnEnable()
    {
        ToggleSetup(BreedingManager.Instance.Result);
        BreedingManager.OnCreatureChangeResult += ToggleSetup;
    }

    private void OnDisable()
    {
        BreedingManager.OnCreatureChangeResult -= ToggleSetup;
    }


    private void UpdatePrice()
    {
        reducedCostText.text = "[ " + BreedingManager.Instance.Result.CreatureStats.PowerLevel.ToNumberSuffix(false) + " ]";
    }

    private void ToggleSetup(Creature creature)
    {
        if (creature != null)
        {
            ToggleGameObject(inactiveTop, false);
            ToggleGameObject(inactiveMid, false);
            ToggleGameObject(inactiveBottom, false);
            ToggleGameObject(activeTop, true);
            ToggleGameObject(activeMid, true);
            ToggleGameObject(activeBottom, true);
            SetCreature(creature);
            UpdatePrice();
            return;
        }
        ToggleGameObject(inactiveTop, true);
        ToggleGameObject(inactiveMid, true);
        ToggleGameObject(inactiveBottom, true);
        ToggleGameObject(activeTop, false);
        ToggleGameObject(activeMid, false);
        ToggleGameObject(activeBottom, false);
    }

    private void ToggleGameObject(GameObject obj, bool turnOn)
    {
        if(obj.activeInHierarchy && turnOn)
        {
            return;
        }

        if(!obj.activeInHierarchy && !turnOn)
        {
            return;
        }

        if (turnOn && !obj.activeInHierarchy)
        {
            obj.SetActive(true);
            return;
        }

        if (obj.activeInHierarchy)
        {
            obj.SetActive(false);
        }
    }

    private void SetCreature(Creature creature)
    {
        fuseSprite.SetupRepresentation(creature);
        fuseDetailsText.SetupRepresentation(creature);
    }

    public void CollectCreature()
    {
        if (BreedingManager.Instance.Collect())
        {
            SoundManager.Instance.PlaySFX("Click");
            UI_ToggleManager.Instance.SwitchState("Inspector");
        }
        else
        {
            SoundManager.Instance.PlaySFX("Error");
            UI_ToastManager.Instance.CreateToast(NotificationType.Alert, "No Creature", "You need to combine a creature first to be able to collect!");
        }
        UI_InventoryManager.Instance.RefreshInventory();
    }

    public void Sell()
    {
        if (BreedingManager.Instance.Result != null)
        {
            BreedingManager.Instance.SellResult();
            UI_ToggleManager.Instance.SwitchState("Inspector");
            SoundManager.Instance.PlaySFX("Transaction");
        }
        else
        {
            SoundManager.Instance.PlaySFX("Error");
            UI_ToastManager.Instance.CreateToast(NotificationType.Alert, "No Creature", "You need to combine a creature first to be able to sell it!");
        }

    }

    public void Refuse()
    {
        Creature creature = BreedingManager.Instance.Result;
        if (creature != null && BreedingManager.Instance.Breed(pay:true, refuse:true))
        {
            SoundManager.Instance.PlaySFX("Transaction");

        }
        else
        {
            SoundManager.Instance.PlaySFX("Error");
            UI_ToastManager.Instance.CreateToast(NotificationType.Alert, "Something went wrong", "You're either trying to refuse without a creature or you're out of money!");
        }
    }
}
