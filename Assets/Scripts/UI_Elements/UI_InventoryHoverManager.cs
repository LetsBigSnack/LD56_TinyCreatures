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
        pwrText.text = creature.CreatureStats.PowerLevel.ToNumberSuffix(false);
        hpText.text = creature.MaxHealth.ToNumberSuffix(false);
        attText.text = creature.CreatureStats.Attack.ToNumberSuffix();
        spdText.text = creature.CreatureStats.Speed.ToNumberSuffix();
        dexText.text = creature.CreatureStats.Dexterity.ToNumberSuffix();
        defText.text = creature.CreatureStats.Defense.ToNumberSuffix();
        winsText.text = creature.CreatureWins.ToNumberSuffix(false);
    }

    public void ResetDetails()
    {
        detailsWindow.SetActive(false);
        if (BattleManager.Instance.IsBattleRunning)
        {
            ChangeBattleText("BATTLE ONGOING!");
        } 
        else
        {
            ChangeBattleText("NO DATA!");
        }
        battleWindow.SetActive(true);
        
    }

    public void ChangeBattleText(string text)
    {
        battleTitleText.text = text;
    }
}
