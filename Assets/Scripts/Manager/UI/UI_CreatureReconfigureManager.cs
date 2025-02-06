using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Data;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum BodyPartToggleTypes
{
    TopHead,
    Head,
    Body,
    Arms,
    Legs,
    Back,
    Tail
}

public class UI_CreatureReconfigureManager : MonoBehaviour, IDropHandler
{
    public static UI_CreatureReconfigureManager Instance;


    private Creature currentCreature;
    private CreatureRepresentation currentRepresentation;

    [SerializeField] private UICreatureButton creatureButton;

    [SerializeField] private BodyPartToggleTypes currentToggle;

    [SerializeField] private BodyPart lastSelectedBodyPart;
    
    [SerializeField] private UI_CreatureSprite creaturePreviewSprite;
    [SerializeField] private Image bodyPartPreviewImage;
    [SerializeField] private Sprite bodyPartPreviewBaseImage;
    [SerializeField] private TextMeshProUGUI noDataText;

    [SerializeField] private BodyPart curTopHead;
    [SerializeField] private BodyPart curHead;
    [SerializeField] private BodyPart curBody;
    [SerializeField] private BodyPart curArms;
    [SerializeField] private BodyPart curLegs;
    [SerializeField] private BodyPart curTail;
    [SerializeField] private BodyPart curBack;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI bodyPartTypeText;

    [SerializeField] private TextMeshProUGUI bodypartAtkValueText;
    [SerializeField] private TextMeshProUGUI bodypartDefValueText;
    [SerializeField] private TextMeshProUGUI bodypartHpValueText;
    [SerializeField] private TextMeshProUGUI bodypartCrtValueText;
    [SerializeField] private TextMeshProUGUI bodypartSpdValueText;

    [SerializeField] private TextMeshProUGUI previewAtkValueText;
    [SerializeField] private TextMeshProUGUI previewDefValueText;
    [SerializeField] private TextMeshProUGUI previewHpValueText;
    [SerializeField] private TextMeshProUGUI previewCrtValueText;
    [SerializeField] private TextMeshProUGUI previewSpdValueText;

    [SerializeField] private List<UI_CreaturePart_Item> topHeadButtons;
    [SerializeField] private List<UI_CreaturePart_Item> headButtons;
    [SerializeField] private List<UI_CreaturePart_Item> bodyButtons;
    [SerializeField] private List<UI_CreaturePart_Item> armsButtons;
    [SerializeField] private List<UI_CreaturePart_Item> legsButtons;
    [SerializeField] private List<UI_CreaturePart_Item> backButtons;
    [SerializeField] private List<UI_CreaturePart_Item> tailButtons;

    private void Awake()
    {
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
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            UICreatureButton uiCreatureButton = eventData.pointerDrag.GetComponent<UICreatureButton>();
            if (uiCreatureButton == null || !uiCreatureButton.IsDragable || uiCreatureButton.Creature == null)
            {
                SoundManager.Instance.PlaySFX("Error");
                return;
            }
            SetNewCreature(uiCreatureButton.Creature);
            SoundManager.Instance.PlaySFX("Click");
        }
    }

    private void SetNewCreature(Creature creature)
    {
        if (currentCreature != null)
        {
            Withdraw(true);
        }
        InventoryManager.Instance.AddToReconfigure(creature);
    }

    public void Withdraw(bool isExchanged = false)
    {
        if (currentCreature != null)
        {
            creatureButton.Creature = null;
            currentCreature = null;
            if (!isExchanged)
            {
                SoundManager.Instance.PlaySFX("Click");
            }
            return;
        }

        SoundManager.Instance.PlaySFX("Error");
    }

    public void ToggleBodyParts(BodyPartToggleTypes toggleTypes)
    {
        switch (toggleTypes)
        {
            case BodyPartToggleTypes.TopHead:
                //display only topHeads in scrollview
                break;
            case BodyPartToggleTypes.Head:
                //display only Heads in scrollview
                break;
            case BodyPartToggleTypes.Body:
                //display only body in scrollview
                break;
            case BodyPartToggleTypes.Arms:
                //display only arms in scrollview
                break;
            case BodyPartToggleTypes.Legs:
                //display only legs in scrollview
                break;
            case BodyPartToggleTypes.Back:
                //display only back in scrollview
                break;
            case BodyPartToggleTypes.Tail:
                //display only tails in scrollview
                break;
        }
        currentToggle = toggleTypes;
    }

    public void PickPart(BodyPart bodyPart, BodyPartType bodyPartType)
    {
        switch (bodyPartType)
        {
            case BodyPartType.Head:
                currentRepresentation.HeadBodyPart = bodyPart;
                break;

            case BodyPartType.Body:
                currentRepresentation.BodyBodyPart = bodyPart;
                break;

            case BodyPartType.Arms:
                currentRepresentation.ArmsBodyPart = bodyPart;
                break;

            case BodyPartType.Legs:
                currentRepresentation.LegsBodyPart = bodyPart;
                break;

            case BodyPartType.TopHead:
                currentRepresentation.TopHeadBodyPart = bodyPart;
                break;

            case BodyPartType.Back:
                currentRepresentation.BackBodyPart = bodyPart;
                break;

            case BodyPartType.Tail:
                currentRepresentation.TailBodyPart = bodyPart;
                break;
        }
        UpdateCreaturePreview();
        UpdateCreatureStatPreview();
    }

    private void UpdateCreaturePreview()
    {
        Creature newCreature = new Creature(
            currentCreature.CreatureGeneration, 
            currentCreature.MaxHealth, 
            currentCreature.CreatureStats, 
            currentRepresentation);

        creaturePreviewSprite.SetupRepresentation(newCreature);
    }

    public void UpdateBodyPartStatPreview(Sprite sprite, BodyPartType type)
    {
        noDataText.text = "";
        bodyPartPreviewImage.sprite = sprite;
        nameText.text = lastSelectedBodyPart.bodyPartName;
        bodyPartTypeText.text = type.ToString();
        bodypartAtkValueText.text = ConcatinateValueText(lastSelectedBodyPart.attackModifier);
        bodypartDefValueText.text = ConcatinateValueText(lastSelectedBodyPart.defenseModifier);
        bodypartHpValueText.text = ConcatinateValueText(lastSelectedBodyPart.healthModifier);
        bodypartCrtValueText.text = ConcatinateValueText(lastSelectedBodyPart.dexterityModifier);
        bodypartSpdValueText.text = ConcatinateValueText(lastSelectedBodyPart.speedModifier);
    }

    private string ConcatinateValueText(float value)
    {
        return "[" + value + "]";
    }

    public void ResetBodyPartStatPreview()
    {
        noDataText.text = "Hover over a bodypart for a preview.";
        bodyPartPreviewImage.sprite = bodyPartPreviewBaseImage;
        nameText.text = "No Data";
        bodyPartTypeText.text = "No Data";
        bodypartAtkValueText.text = ConcatinateValueText(0);
        bodypartDefValueText.text = ConcatinateValueText(0);
        bodypartHpValueText.text = ConcatinateValueText(0);
        bodypartCrtValueText.text = ConcatinateValueText(0);
        bodypartSpdValueText.text = ConcatinateValueText(0);
    }

    private void UpdateCreatureStatPreview()
    {
        float atkValue = 0;
        float defValue = 0;
        float hpValue = 0;
        float spdValue = 0;
        float crtValue = 0;

        foreach(BodyPart bodyPart in currentRepresentation.BodyParts.Values)
        {
            atkValue = atkValue + bodyPart.attackModifier;
            defValue = defValue + bodyPart.defenseModifier;
            hpValue = hpValue + bodyPart.healthModifier;
            spdValue = spdValue + bodyPart.speedModifier;
            crtValue = crtValue + bodyPart.dexterityModifier;
        }

        previewAtkValueText.text = ConcatinateValueText(atkValue);
        previewDefValueText.text = ConcatinateValueText(defValue);
        previewHpValueText.text = ConcatinateValueText(hpValue);
        previewCrtValueText.text = ConcatinateValueText(crtValue);
        previewSpdValueText.text = ConcatinateValueText(spdValue);
    }

    private void ResetCreatureStatPreview()
    {
        previewAtkValueText.text = ConcatinateValueText(0);
        previewDefValueText.text = ConcatinateValueText(0);
        previewHpValueText.text = ConcatinateValueText(0);
        previewCrtValueText.text = ConcatinateValueText(0);
        previewSpdValueText.text = ConcatinateValueText(0);
    }

    public void BuyCreature()
    {
        if (InventoryManager.Instance.SelectedCreatureForReConfigure != null)
        {
            ReconfigureManager.Instance.ReconfigureSelectedCreature(currentRepresentation);
            UI_InventoryManager.Instance.RefreshInventory();
            SoundManager.Instance.PlaySFX("Transaction");
            return;
        }
        SoundManager.Instance.PlaySFX("Error");
    }

    public void ResetConfiguration()
    {
        if(currentCreature != null)
        {
            currentRepresentation = currentCreature.Representation;
        }
    }

    public void CancleReconfiguration()
    {
        if (InventoryManager.Instance.SelectedCreatureForReConfigure != null)
        {
            ReconfigureManager.Instance.RemoveFromReconfigure();
            UI_InventoryManager.Instance.RefreshInventory();
            SoundManager.Instance.PlaySFX("Transaction");
            return;
        }
        SoundManager.Instance.PlaySFX("Error");
    }

}
