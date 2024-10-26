using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Helper.Util;

public class UI_InventoryHoverManager : MonoBehaviour
{
    public static UI_InventoryHoverManager Instance { get; private set; }

    [SerializeField] private GameObject detailsWindow;
    [SerializeField] private GameObject battleWindow;

    [SerializeField] private TextMeshProUGUI battleTitleText;

    [SerializeField] private TextMeshProUGUI creatureNameText;
    [SerializeField] private TextMeshProUGUI pwrText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI attText;
    [SerializeField] private TextMeshProUGUI spdText;
    [SerializeField] private TextMeshProUGUI dexText;
    [SerializeField] private TextMeshProUGUI defText;
    [SerializeField] private TextMeshProUGUI winsText;

    [SerializeField] private Creature hoveredCreature;

    public Creature Creature
    {
        get => hoveredCreature;
        set => hoveredCreature = value;
    }

    public TextMeshProUGUI BattleText
    {
        get => battleTitleText;
        set => battleTitleText = value;
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

    void Start()
    {
        battleWindow.SetActive(true);
        battleTitleText.text = "NO DATA!";
    }

    public void SetDetails(Creature creature)
    {
        battleWindow.SetActive(false);
        detailsWindow.SetActive(true);
        creatureNameText.text = creature.CreatureName;
        pwrText.text = Util_LargeNumberDisplay.LargerNumberConversion(creature.CreatureStats.PowerLevel, false);
        hpText.text = Util_LargeNumberDisplay.LargerNumberConversion(creature.MaxHealth, false);
        attText.text = Util_LargeNumberDisplay.LargerNumberConversion(creature.CreatureStats.Attack);
        spdText.text = Util_LargeNumberDisplay.LargerNumberConversion(creature.CreatureStats.Speed);
        dexText.text = Util_LargeNumberDisplay.LargerNumberConversion(creature.CreatureStats.Dexterity);
        defText.text = Util_LargeNumberDisplay.LargerNumberConversion(creature.CreatureStats.Defense);
        winsText.text = Util_LargeNumberDisplay.LargerNumberConversion(creature.CreatureWins, false);
    }

    public void ResetDetails()
    {
        detailsWindow.SetActive(false);
        if (InventoryManager.Instance.SelectedCreatureForBattle != null)
        {
            battleTitleText.text = "BATTLE ONGOING!";
        } 
        else
        {
            battleTitleText.text = "NO DATA!";
        }
        battleWindow.SetActive(true);
        
    }
}
