using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UI_CreatureReconfigureManager : MonoBehaviour
{
    public static UI_CreatureReconfigureManager Instance;
    
    [SerializeField] private UI_CreatureSprite creaturePreviewSprite;

    [SerializeField] private int headIndex = 0;
    [SerializeField] private int bodyIndex = 0;
    [SerializeField] private int armIndex = 0;
    [SerializeField] private int legIndex = 0;
    
    [SerializeField] private UI_CreatureReconfigurItem headItem;
    [SerializeField] private UI_CreatureReconfigurItem bodyItem;
    [SerializeField] private UI_CreatureReconfigurItem armsItem;
    [SerializeField] private UI_CreatureReconfigurItem legsItem;

    private SoundManager soundManager;
    
    //TODO: REWRITE WHOLE CODE!

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        ReconfigureManager.Instance.ClearEntries();
        ReconfigureManager.Instance.CreateEntries();
        SetImages();
        
    }

    private void OnDisable()
    {
        //ResetCreaturePicked();
    }

    private void SetImages()
    {
        
        if (InventoryManager.Instance.SelectedCreatureForReConfigure != null)
        {
            var currentCreature = InventoryManager.Instance.SelectedCreatureForReConfigure.Representation;
            
            creaturePreviewSprite.CreatureHead.color = currentCreature.HeadColor;
            creaturePreviewSprite.CreatureBody.color = currentCreature.BodyColor;
            creaturePreviewSprite.CreatureArms.color = currentCreature.ArmsColor;
            creaturePreviewSprite.CreatureLegs.color = currentCreature.LegsColor;
            
            creaturePreviewSprite.CreatureHead.sprite =
                SetItemSprite(headItem, ReconfigureManager.Instance.Heads, headIndex);
            creaturePreviewSprite.CreatureBody.sprite =
                SetItemSprite(bodyItem, ReconfigureManager.Instance.Bodies, bodyIndex);
            creaturePreviewSprite.CreatureArms.sprite =
                SetItemSprite(armsItem, ReconfigureManager.Instance.Arms, armIndex);
            creaturePreviewSprite.CreatureLegs.sprite =
                SetItemSprite(legsItem, ReconfigureManager.Instance.Legs, legIndex);
            return;
        }
        creaturePreviewSprite.CreatureHead.color = Color.clear;
        creaturePreviewSprite.CreatureBody.color = Color.clear;
        creaturePreviewSprite.CreatureArms.color = Color.clear;
        creaturePreviewSprite.CreatureLegs.color = Color.clear;
    }

    public void CreaturePicked(Creature creature)
    {
        headIndex = ReconfigureManager.Instance.ReturnIndex(BodyPartType.Head);
        bodyIndex = ReconfigureManager.Instance.ReturnIndex(BodyPartType.Body);
        armIndex = ReconfigureManager.Instance.ReturnIndex(BodyPartType.Arms);
        legIndex = ReconfigureManager.Instance.ReturnIndex(BodyPartType.Legs);

        headItem.CurrentPart.sprite = ReconfigureManager.Instance.ReturnSelectedRepresentation(BodyPartType.Head);
        bodyItem.CurrentPart.sprite = ReconfigureManager.Instance.ReturnSelectedRepresentation(BodyPartType.Body);
        armsItem.CurrentPart.sprite = ReconfigureManager.Instance.ReturnSelectedRepresentation(BodyPartType.Arms);
        legsItem.CurrentPart.sprite = ReconfigureManager.Instance.ReturnSelectedRepresentation(BodyPartType.Legs);

        creaturePreviewSprite.CreatureHead.sprite = ReconfigureManager.Instance.ReturnSelectedRepresentation(BodyPartType.Head);
        creaturePreviewSprite.CreatureBody.sprite = ReconfigureManager.Instance.ReturnSelectedRepresentation(BodyPartType.Body);
        creaturePreviewSprite.CreatureArms.sprite = ReconfigureManager.Instance.ReturnSelectedRepresentation(BodyPartType.Arms);
        creaturePreviewSprite.CreatureLegs.sprite = ReconfigureManager.Instance.ReturnSelectedRepresentation(BodyPartType.Legs);
            
        creaturePreviewSprite.CreatureHead.color = creature.Representation.HeadColor;
        creaturePreviewSprite.CreatureBody.color = creature.Representation.BodyColor;
        creaturePreviewSprite.CreatureArms.color = creature.Representation.ArmsColor;
        creaturePreviewSprite.CreatureLegs.color = creature.Representation.LegsColor;
    }

    private void ResetCreaturePicked()
    {
        headIndex = 0;
        bodyIndex = 0;
        armIndex = 0;
        legIndex = 0;

        creaturePreviewSprite.CreatureHead.sprite = SetItemSprite(headItem, ReconfigureManager.Instance.Heads, headIndex);
        creaturePreviewSprite.CreatureBody.sprite = SetItemSprite(bodyItem, ReconfigureManager.Instance.Bodies, bodyIndex);
        creaturePreviewSprite.CreatureArms.sprite = SetItemSprite(armsItem, ReconfigureManager.Instance.Arms, armIndex);
        creaturePreviewSprite.CreatureLegs.sprite = SetItemSprite(legsItem, ReconfigureManager.Instance.Legs, legIndex);

        ReconfigureManager.Instance.RemoveFromReconfigure();
        UI_InventoryManager.Instance.RefreshInventory();
        
        SetImages();
    }

    public void NextEntry(string part)
    {
        switch (part)
        {
            case "head":
                headIndex = OutOfBoundPrevention(ReconfigureManager.Instance.Heads, headIndex, 1);
                creaturePreviewSprite.CreatureHead.sprite = SetItemSprite(headItem, ReconfigureManager.Instance.Heads, headIndex);
                ReconfigureManager.Instance.SetCurrentParts(BodyPartType.Head, headIndex);
                break;

            case "body":
                bodyIndex = OutOfBoundPrevention(ReconfigureManager.Instance.Bodies, bodyIndex, 1);
                creaturePreviewSprite.CreatureBody.sprite = SetItemSprite(bodyItem, ReconfigureManager.Instance.Bodies, bodyIndex);
                ReconfigureManager.Instance.SetCurrentParts(BodyPartType.Body, bodyIndex);
                break;

            case "arms":
                armIndex = OutOfBoundPrevention(ReconfigureManager.Instance.Arms, armIndex, 1);
                creaturePreviewSprite.CreatureArms.sprite = SetItemSprite(armsItem, ReconfigureManager.Instance.Arms, armIndex);
                ReconfigureManager.Instance.SetCurrentParts(BodyPartType.Arms, armIndex);
                break;

            case "legs":
                legIndex = OutOfBoundPrevention(ReconfigureManager.Instance.Legs, legIndex, 1);
                creaturePreviewSprite.CreatureLegs.sprite = SetItemSprite(legsItem, ReconfigureManager.Instance.Legs, legIndex);
                ReconfigureManager.Instance.SetCurrentParts(BodyPartType.Legs, legIndex);
                break;
        }

        soundManager.PlaySFX("Click");
    }

    public void PreviousEntry(string part)
    {
        switch (part)
        {
            case "head":
                headIndex = OutOfBoundPrevention(ReconfigureManager.Instance.Heads, headIndex, -1);
                creaturePreviewSprite.CreatureHead.sprite = SetItemSprite(headItem, ReconfigureManager.Instance.Heads, headIndex);
                ReconfigureManager.Instance.SetCurrentParts(BodyPartType.Head, headIndex);
                break;

            case "body":
                bodyIndex = OutOfBoundPrevention(ReconfigureManager.Instance.Bodies, bodyIndex, -1);
                creaturePreviewSprite.CreatureBody.sprite = SetItemSprite(bodyItem, ReconfigureManager.Instance.Bodies, bodyIndex);
                ReconfigureManager.Instance.SetCurrentParts(BodyPartType.Body, bodyIndex);
                break;

            case "arms":
                armIndex = OutOfBoundPrevention(ReconfigureManager.Instance.Arms, armIndex, -1);
                creaturePreviewSprite.CreatureArms.sprite = SetItemSprite(armsItem, ReconfigureManager.Instance.Arms, armIndex);
                ReconfigureManager.Instance.SetCurrentParts(BodyPartType.Arms, armIndex);
                break;

            case "legs":
                legIndex = OutOfBoundPrevention(ReconfigureManager.Instance.Legs, legIndex, -1);
                creaturePreviewSprite.CreatureLegs.sprite = SetItemSprite(legsItem, ReconfigureManager.Instance.Legs, legIndex);
                ReconfigureManager.Instance.SetCurrentParts(BodyPartType.Legs, legIndex);
                break;
        }
        soundManager.PlaySFX("Click");
    }

    private Sprite SetItemSprite(UI_CreatureReconfigurItem item, List<BodyPartEntry> parts, int currentIndex)
    {
        item.NameText.text = parts[currentIndex].bodyPart.name;
        return item.CurrentPart.sprite = parts[currentIndex].bodyPart.bodyPartSprite;
    }

    private int OutOfBoundPrevention(List<BodyPartEntry> parts, int currentIndex, int value)
    {
        if (currentIndex + value > parts.Count()-1)
        {
            return 0;
        }
        else if (currentIndex + value < 0)
        {
            return parts.Count()-1;
        }
        else
        {
           return  currentIndex + value;
        }
    }

    public void BuyCreature()
    {
        if (InventoryManager.Instance.SelectedCreatureForReConfigure != null)
        {
            ReconfigureManager.Instance.ReconfigureSelectedCreature();
            ResetCreaturePicked();
            UI_InventoryManager.Instance.RefreshInventory();
            soundManager.PlaySFX("Transaction");
            return;
        }
        soundManager.PlaySFX("Error");
    }

    public void CancleReconfiguration()
    {
        if (InventoryManager.Instance.SelectedCreatureForReConfigure != null)
        {
            ResetCreaturePicked();
            ReconfigureManager.Instance.RemoveFromReconfigure();
            UI_InventoryManager.Instance.RefreshInventory();
            soundManager.PlaySFX("Transaction");
            return;
        }
        soundManager.PlaySFX("Error");
    }
}
