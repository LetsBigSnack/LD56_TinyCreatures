using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CreatureDetailsText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI creatureNameText;
    [SerializeField] private TextMeshProUGUI powerLevelText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI spdText;
    [SerializeField] private TextMeshProUGUI dexText;
    [SerializeField] private TextMeshProUGUI defText;

    public void Reset()
    {
        creatureNameText.text = null;
        powerLevelText.text = null;
        hpText.text = null;
        atkText.text = null;
        spdText.text = null;
        dexText.text = null;
        defText.text = null;
    }

    public void SetupRepresentation(Creature creature)
    {
        creatureNameText.text = creature.CreatureName;
        powerLevelText.text = creature.PowerLevel.ToString();
        hpText.text = creature.MaxHealth.ToString();
        atkText.text = creature.Attack.ToString();
        spdText.text = creature.Speed.ToString();
        dexText.text = creature.Dexterity.ToString();
        defText.text = creature.Defense.ToString();
    }

}
