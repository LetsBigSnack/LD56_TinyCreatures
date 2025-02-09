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
    private Creature reconfigCreature;

    [SerializeField] private UICreatureButton creatureButton;

    [SerializeField] private BodyPartToggleTypes currentToggle;

    [SerializeField] private GameObject uiBodyPartItemPrefab;
    [SerializeField] private Transform scrollViewContent;
    
    [SerializeField] private UI_CreatureSprite creaturePreviewSprite;
    [SerializeField] private Image bodyPartPreviewImage;
    [SerializeField] private Sprite bodyPartPreviewBaseImage;
    [SerializeField] private TextMeshProUGUI noDataText;
    [SerializeField] private TextMeshProUGUI noCreatureInConfigureText;

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

    [SerializeField] private List<BodyPartButtonAttributes> topHeadButtons = new List<BodyPartButtonAttributes>();
    [SerializeField] private List<BodyPartButtonAttributes> headButtons = new List<BodyPartButtonAttributes>();
    [SerializeField] private List<BodyPartButtonAttributes> bodyButtons = new List<BodyPartButtonAttributes>();
    [SerializeField] private List<BodyPartButtonAttributes> armsButtons = new List<BodyPartButtonAttributes>();
    [SerializeField] private List<BodyPartButtonAttributes> legsButtons = new List<BodyPartButtonAttributes>();
    [SerializeField] private List<BodyPartButtonAttributes> backButtons = new List<BodyPartButtonAttributes>();
    [SerializeField] private List<BodyPartButtonAttributes> tailButtons = new List<BodyPartButtonAttributes>();

    [SerializeField] private List<GameObject> currentlyDisplayedButtons;

    public BodyPartToggleTypes CurrentToggle
    {
        get => currentToggle;
        set => currentToggle = value;
    }

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

    public void OnEnable()
    {
        InitialSetup();
    }

    public void Start()
    {
        if (currentCreature == null) return;
        ApplyColorsToCreatureButton();
    }

    private void InitialSetup()
    {
        ReconfigureManager.Instance.ClearEntries();
        ReconfigureManager.Instance.CreateEntries();
        currentCreature = InventoryManager.Instance.SelectedCreatureForReConfigure;
        reconfigCreature = SetReconfigureCreature(InventoryManager.Instance.SelectedCreatureForReConfigure);
        ResetBodyPartStatPreview();
        ResetCreatureStatPreview();
        UpdateAllButtonLists();
        ClearCurrentlyDisplayedButtonList();
        UpdateCreaturePreview();
        ToggleBodyParts(BodyPartToggleTypes.Head);
    }

    private void RefreshAfterDrop()
    {
        ResetBodyPartStatPreview();
        ResetCreatureStatPreview();
        UpdateAllButtonLists();
        UpdateCreaturePreview();
        ApplyColorsToCreatureButton();
        ToggleBodyParts(BodyPartToggleTypes.Head); 
    }

    private void ApplyColorsToCreatureButton()
    {
        creatureButton.gameObject.GetComponent<PalletSwap>().BaseColor = currentCreature.Representation.BaseColor;
        creatureButton.gameObject.GetComponent<PalletSwap>().AddOnColor = currentCreature.Representation.AddOnColor;
        creatureButton.gameObject.GetComponent<PalletSwap>().GetAllImageComponentsInChildren();
        creatureButton.gameObject.GetComponent<PalletSwap>().ApplyNewMaterial();
    }

    private Creature SetReconfigureCreature(Creature creature)
    {
        if(creature == null)
        {
            return null;
        }

        return new Creature(creature.CreatureGeneration, creature.MaxHealth, creature.CreatureStats, creature.Representation);
    }

    private void UpdateButtonList(List<BodyPartEntry> bodyPartEntryList, List<BodyPartButtonAttributes> creatureButtonList)
    {
        foreach (BodyPartEntry bodyPart in bodyPartEntryList)
        {
            if (creatureButtonList != null && !creatureButtonList.Exists(c => c.bodyPart == bodyPart.bodyPart) && !creatureButtonList.Exists(c => c.bodyPart.bodyPartName == bodyPart.bodyPart.bodyPartName))
            {
                BodyPartButtonAttributes newButton = new BodyPartButtonAttributes();
                newButton.bodyPart = bodyPart.bodyPart;
                newButton.bodyPartType = bodyPart.bodyPartType;
                newButton.bodyPartSprite = bodyPart.bodyPart.bodyPartSprite;
                creatureButtonList.Add(newButton);
            }
        }
    }

    private void UpdateAllButtonLists()
    {
        var rfm = ReconfigureManager.Instance;
        UpdateButtonList(rfm.TopHeads, topHeadButtons);
        UpdateButtonList(rfm.Heads, headButtons);
        UpdateButtonList(rfm.Bodies, bodyButtons);
        UpdateButtonList(rfm.Arms, armsButtons);
        UpdateButtonList(rfm.Legs, legsButtons);
        UpdateButtonList(rfm.Backs, backButtons);
        UpdateButtonList(rfm.Tails, tailButtons);
    }

    private void CreateNonExistendButtons(List<BodyPartButtonAttributes> buttonList)
    {
        foreach(BodyPartButtonAttributes creaturePartItem in buttonList)
        {
            GameObject newButton = Instantiate(uiBodyPartItemPrefab, scrollViewContent, false);
            newButton.transform.SetParent(scrollViewContent);
            UI_CreaturePart_Item newButtonComponent = newButton.GetComponent<UI_CreaturePart_Item>();
            newButtonComponent.BodyPart = creaturePartItem.bodyPart;
            newButtonComponent.BodyPartType = creaturePartItem.bodyPartType;
            newButtonComponent.BodyPartImage.sprite = creaturePartItem.bodyPartSprite;
            currentlyDisplayedButtons.Add(newButton);
        }
    }

    private void ClearCurrentlyDisplayedButtonList()
    {
        foreach(GameObject button in currentlyDisplayedButtons)
        {
            Destroy(button);
        }
        currentlyDisplayedButtons.Clear();
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
        currentCreature = creature;
        reconfigCreature = SetReconfigureCreature(creature);
        reconfigCreature.Representation = creature.Representation;
        RefreshAfterDrop();
    }

    public void Withdraw(bool isExchanged = false)
    {
        if (currentCreature != null)
        {
            CancleReconfiguration();
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
        ClearCurrentlyDisplayedButtonList();

        switch (toggleTypes)
        {
            case BodyPartToggleTypes.TopHead:
                CreateNonExistendButtons(topHeadButtons);
                break;
            case BodyPartToggleTypes.Head:
                CreateNonExistendButtons(headButtons);
                break;
            case BodyPartToggleTypes.Body:
                CreateNonExistendButtons(bodyButtons);
                break;
            case BodyPartToggleTypes.Arms:
                CreateNonExistendButtons(armsButtons);
                break;
            case BodyPartToggleTypes.Legs:
                CreateNonExistendButtons(legsButtons);
                break;
            case BodyPartToggleTypes.Back:
                CreateNonExistendButtons(backButtons);
                break;
            case BodyPartToggleTypes.Tail:
                CreateNonExistendButtons(tailButtons);
                break;
        }
        currentToggle = toggleTypes;
        UI_InventoryManager.Instance.RefreshInventory();
    }

    public void PickPart(BodyPart bodyPart, BodyPartType bodyPartType)
    {
        if(currentCreature == null)
        {
            UI_ToastManager.Instance.CreateToast("Reconfigure Empty!", "Please drag a creature of your choice into the reconfigure to start!");
            SoundManager.Instance.PlaySFX("Error");
            return;
        }

        switch (bodyPartType)
        {
            case BodyPartType.Head:
                reconfigCreature.Representation.HeadBodyPart = bodyPart;
                break;

            case BodyPartType.Body:
                reconfigCreature.Representation.BodyBodyPart = bodyPart;
                break;

            case BodyPartType.Arms:
                reconfigCreature.Representation.ArmsBodyPart = bodyPart;
                break;

            case BodyPartType.Legs:
                reconfigCreature.Representation.LegsBodyPart = bodyPart;
                break;

            case BodyPartType.TopHead:
                reconfigCreature.Representation.TopHeadBodyPart = bodyPart;
                break;

            case BodyPartType.Back:
                reconfigCreature.Representation.BackBodyPart = bodyPart;
                break;

            case BodyPartType.Tail:
                reconfigCreature.Representation.TailBodyPart = bodyPart;
                break;
        }

        UpdateCreaturePreview();
        UpdateCreatureStatPreview();
    }

    private void UpdateCreaturePreview()
    {
        if(currentCreature == null)
        {
            ResetCreatureStatPreview();
            creaturePreviewSprite.Reset();
            return;
        }

        noCreatureInConfigureText.text = "";
        Creature newCreature = new Creature(
            currentCreature.CreatureGeneration,
            currentCreature.MaxHealth,
            currentCreature.CreatureStats,
            reconfigCreature.Representation);
        creaturePreviewSprite.SetupRepresentation(newCreature);
    }

    public void UpdateBodyPartStatPreview(Sprite sprite, BodyPartType type, BodyPart bodyPart)
    {
        noDataText.text = "";
        bodyPartPreviewImage.sprite = sprite;
        nameText.text = bodyPart.bodyPartName;
        bodyPartTypeText.text = type.ToString();
        bodypartAtkValueText.text = ConcatinateValueText(bodyPart.attackModifier);
        bodypartDefValueText.text = ConcatinateValueText(bodyPart.defenseModifier);
        bodypartHpValueText.text = ConcatinateValueText(bodyPart.healthModifier);
        bodypartCrtValueText.text = ConcatinateValueText(bodyPart.dexterityModifier);
        bodypartSpdValueText.text = ConcatinateValueText(bodyPart.speedModifier);
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

        foreach(BodyPart bodyPart in reconfigCreature.Representation.BodyParts.Values)
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
        noCreatureInConfigureText.text = "Drag a creature here to start!";
        previewAtkValueText.text = ConcatinateValueText(0);
        previewDefValueText.text = ConcatinateValueText(0);
        previewHpValueText.text = ConcatinateValueText(0);
        previewCrtValueText.text = ConcatinateValueText(0);
        previewSpdValueText.text = ConcatinateValueText(0);
    }

    public void BuyCreature()
    {
        if (currentCreature == null)
        {
            UI_ToastManager.Instance.CreateToast("Reconfigure Empty!", "Please drag a creature of your choice into the reconfigure to start!");
            SoundManager.Instance.PlaySFX("Error");
            return;
        }

        if (InventoryManager.Instance.SelectedCreatureForReConfigure != null)
        {
            ReconfigureManager.Instance.ReconfigureSelectedCreature(reconfigCreature.Representation);
            currentCreature = null;
            reconfigCreature = null;
            UpdateCreaturePreview();
            ResetBodyPartStatPreview();
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
            reconfigCreature.Representation = currentCreature.Representation;
            UpdateCreaturePreview();
            UpdateCreatureStatPreview();
        }
    }

    public void CancleReconfiguration()
    {
        if (currentCreature == null)
        {
            UI_ToastManager.Instance.CreateToast("Reconfigure Empty!", "Please drag a creature of your choice into the reconfigure to start!");
            SoundManager.Instance.PlaySFX("Error");
            return;
        }

        if (InventoryManager.Instance.SelectedCreatureForReConfigure != null)
        {
            ReconfigureManager.Instance.RemoveFromReconfigure();
            creatureButton.Creature = null;
            currentCreature = null;
            reconfigCreature = null;
            UpdateCreaturePreview();
            ResetCreatureStatPreview();
            UI_InventoryManager.Instance.RefreshInventory();
            SoundManager.Instance.PlaySFX("Transaction");
            return;
        }
        SoundManager.Instance.PlaySFX("Error");
    }

}
