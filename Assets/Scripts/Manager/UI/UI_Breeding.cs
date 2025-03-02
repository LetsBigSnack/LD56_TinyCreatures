using System;
using System.Collections;
using System.Collections.Generic;
using Helper.Util;
using TMPro;
using UnityEngine;

public class UI_BreedingManager : MonoBehaviour
{ 
    static public UI_BreedingManager Instance;
    [SerializeField] private UI_CreatureSprite creatureSpriteLeft;
    [SerializeField] private UI_CreatureSprite creatureSpriteRight;
    [SerializeField] private UI_CreatureSprite creatureSpriteMiddle;
    [SerializeField] private UI_CreatureDetailsText detailsLeft;
    [SerializeField] private UI_CreatureDetailsText detailsRight;
    [SerializeField] private TextMeshProUGUI costText;

    [SerializeField] private GameObject activeLeftCreature;
    [SerializeField] private GameObject inactiveLeftCreature;

    [SerializeField] private GameObject activeRightCreature;
    [SerializeField] private GameObject inactiveRightCreature;

    [SerializeField] private GameObject activeCombinedCreature;
    [SerializeField] private GameObject inactiveCombinedCreature;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        BreedingManager.Instance.OnCreatureChangePod1 += UpdateRepresentationLeftPod;
        BreedingManager.Instance.OnCreatureChangePod2 += UpdateRepresentationRightPod;
        BreedingManager.Instance.OnCreatureChangeResult += UpdateRepresentationMiddlePod;
    }

    private void OnDisable()
    {
        BreedingManager.Instance.OnCreatureChangePod1 -= UpdateRepresentationLeftPod;
        BreedingManager.Instance.OnCreatureChangePod2 -= UpdateRepresentationRightPod;
        BreedingManager.Instance.OnCreatureChangeResult -= UpdateRepresentationMiddlePod;
    }

    private void Start()
    {
        UpdateRepresentationLeftPod(BreedingManager.Instance.CreaturePod1);
        UpdateRepresentationRightPod(BreedingManager.Instance.CreaturePod2);
        UpdateRepresentationMiddlePod(BreedingManager.Instance.Result);
    }

    public void UpdateRepresentationLeftPod(Creature creature)
    {
        if(creature == null)
        {
            activeLeftCreature.SetActive(false);
            inactiveLeftCreature.SetActive(true);
            return;
        }
        activeLeftCreature.SetActive(true);
        inactiveLeftCreature.SetActive(false);
        creatureSpriteLeft.SetupRepresentation(BreedingManager.Instance.CreaturePod1);
        detailsLeft.SetupRepresentation(BreedingManager.Instance.CreaturePod1);
        BreedingManager.Instance.UpdatePrice();
        costText.text = BreedingManager.Instance.BreedingPrice.ToNumberSuffix(false);
    }

    public void UpdateRepresentationRightPod(Creature creature)
    {
        if(creature == null)
        {
            activeRightCreature.SetActive(false);
            inactiveRightCreature.SetActive(true);
            return;
        }
        activeRightCreature.SetActive(true);
        inactiveRightCreature.SetActive(false);
        creatureSpriteRight.SetupRepresentation(BreedingManager.Instance.CreaturePod2);
        detailsRight.SetupRepresentation(BreedingManager.Instance.CreaturePod2);
        BreedingManager.Instance.UpdatePrice();
        costText.text = BreedingManager.Instance.BreedingPrice.ToNumberSuffix(false);
    }

    public void UpdateRepresentationMiddlePod(Creature creature)
    {
        if (creature == null)
        {
            activeCombinedCreature.SetActive(false);
            inactiveCombinedCreature.SetActive(true);
            return;
        }
        activeCombinedCreature.SetActive(true);
        inactiveCombinedCreature.SetActive(false);
        creatureSpriteMiddle.SetupRepresentation(BreedingManager.Instance.Result);
    }

    public void SetPodActive(bool isLeft, Creature creature)
    {
        if (isLeft)
        {
            if (AddToLeftPod(creature))
            {
                SoundManager.Instance.PlaySFX("Drop");
                return;
            }
        } 
        else
        {
            if (AddToRightPod(creature))
            {
                SoundManager.Instance.PlaySFX("Drop");
                return;
            }
        }
    }

    public bool AddToRightPod(Creature creature)
    {
        return InventoryManager.Instance.AddToBreedRight(creature);
    }

    public bool AddToLeftPod(Creature creature)
    {
       return InventoryManager.Instance.AddToBreedLeft(creature);
    }

    public void RemoveCreatureFromPod(bool isLeft)
    {
        if (BreedingManager.Instance.RemoveFromBreed(isLeft))
        {
            SoundManager.Instance.PlaySFX("Click");
        }
        else
        {
            SoundManager.Instance.PlaySFX("Error");
            UI_ToastManager.Instance.CreateToast(NotificationType.Alert, "No Creature", "There no Creature in this pod that could be removed!");
        }
        BreedingManager.Instance.UpdatePrice();
        costText.text = BreedingManager.Instance.BreedingPrice.ToNumberSuffix(false);
        UI_InventoryManager.Instance.RefreshInventory();
    }
    
    public void FuseCreature()
    {
        if (BreedingManager.Instance.Result != null)
        {
            SoundManager.Instance.PlaySFX("Error");
            UI_ToastManager.Instance.CreateToast(NotificationType.Alert, "Already Combined", "Please remove the combined creature before trying to combine again!");
            return;
        }
        if (BreedingManager.Instance.Breed())
        {
            SoundManager.Instance.PlaySFX("Transaction");
            UI_ToggleManager.Instance.SwitchState("Fusion");
        }
        else
        {
            SoundManager.Instance.PlaySFX("Error");
            UI_ToastManager.Instance.CreateToast(NotificationType.Alert, "Something went wrong", "You're either trying to refuse without a creature or you're out of money!");
        }
    }
    
}
