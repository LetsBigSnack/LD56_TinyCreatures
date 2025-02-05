using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Data;
using UnityEngine.UI;

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

public class UI_CreatureReconfigureManager : MonoBehaviour
{
    public static UI_CreatureReconfigureManager Instance;

    [SerializeField] private BodyPartToggleTypes currentToggle;

    [SerializeField] private BodyPart lastSelectedBodyPart;
    
    [SerializeField] private UI_CreatureSprite creaturePreviewSprite;
    [SerializeField] private Image bodyPartPreviewImage;

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

    private CreatureRepresentation originalRepresentation;
    private CreatureRepresentation currentRepresentation;

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

    private void ToggleBodyParts(BodyPartToggleTypes toggleTypes)
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

    private void PickPart(BodyPart bodyPart, BodyPartType bodyPartType)
    {
        switch (bodyPartType)
        {
            case BodyPartType.Head:
                break;

            case BodyPartType.Body:
                break;

            case BodyPartType.Arms:
                break;

            case BodyPartType.Legs:
                break;
        }
    }

    private void SetCreatureHead()
    {
        //changes the head sprite of the preview
    }

    private void SetCreatureBody()
    {
        //changes the body sprite of the preview
    }

    private void SetCreatureArms()
    {
        //Changes the arms sprite of the preview
    }

    private void SetCreatureLegs()
    {
        //Changes the leg sprite of the preview
    }

    private void SetBodyPartStatPreview()
    {
        //displays all stats available in the bodypart
    }

    private void SetCreatureStatPreview()
    {
        //displays all accumulated stats for the current creature 
    }

    private void SetNameAndTypeText()
    {
        //sets the text name
        //sets the bodyPartType
    }
    public void BuyCreature()
    {
        if (InventoryManager.Instance.SelectedCreatureForReConfigure != null)
        {
            ReconfigureManager.Instance.ReconfigureSelectedCreature();
            UI_InventoryManager.Instance.RefreshInventory();
            SoundManager.Instance.PlaySFX("Transaction");
            return;
        }
        SoundManager.Instance.PlaySFX("Error");
    }

    public void ResetConfiguration()
    {
        //reset the configuration to be the original creature;
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
