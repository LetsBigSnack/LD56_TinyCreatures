using System.Collections;
using System.Collections.Generic;
using Helper.Util;
using TMPro;
using UnityEngine;

public class BuyBackCreature : MonoBehaviour
{
    private Creature creature;
    [SerializeField] private UICreatureButton creatureButton;
    [SerializeField] private UI_CreatureSprite sprite;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI priceText;
    
        
    public Creature Creature
    {
        get => creature;
        set => creature = value;
    }

    public void BuyBack()
    {
        if (StoreManager.Instance.BuyBack(creature))
        {
            SoundManager.Instance.PlaySFX("Click");
            UI_InventoryManager.Instance.RefreshInventory();
            FindObjectOfType<UI_ShopManager>().RefreshInventory();
        }
        else
        {
            SoundManager.Instance.PlaySFX("Error");
        }
    }

    public void Refresh()
    {
        sprite.SetupRepresentation(creature);
        ApplyColorsToCreatureButton();
        text.text = creature.CreatureName;
        priceText.text = "Buy\n" + creature.CreatureStats.PowerLevel.ToNumberSuffix(false) + ",-";
    }

    private void ApplyColorsToCreatureButton()
    {
        creatureButton.gameObject.GetComponent<PalletSwap>().BaseColor = creature.Representation.BaseColor;
        creatureButton.gameObject.GetComponent<PalletSwap>().AddOnColor = creature.Representation.AddOnColor;
        creatureButton.gameObject.GetComponent<PalletSwap>().GetAllImageComponentsInChildren();
        creatureButton.gameObject.GetComponent<PalletSwap>().ApplyNewMaterial();
    }
}
