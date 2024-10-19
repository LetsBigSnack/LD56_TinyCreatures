using System.Collections;
using System.Collections.Generic;
using Helper.Util;
using TMPro;
using UnityEngine;

public class BuyBackCreature : MonoBehaviour
{
    [SerializeField] private Creature creature;
    [SerializeField] private UI_CreatureSprite sprite;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI priceText;
    
    
    private SoundManager soundManager;
        
    public Creature Creature
    {
        get => creature;
        set => creature = value;
    }

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void BuyBack()
    {
        if (StoreManager.Instance.BuyBack(creature))
        {
            soundManager.PlaySFX("Click");
            UI_InventoryManager.Instance.RefreshInventory();
            FindObjectOfType<UI_ShopManager>().RefreshInventory();
        }
        else
        {
            soundManager.PlaySFX("Error");
        }
    }

    public void Refresh()
    {
        sprite.SetupRepresentation(creature);
        text.text = creature.CreatureName;
        priceText.text = "Buy\n" + Util_LargeNumberDisplay.LargerNumberConversion(creature.CreatureStats.PowerLevel,false) + ",-";
    }
}
