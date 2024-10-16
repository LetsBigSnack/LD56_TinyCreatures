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
        hpText.color = new Color(6/255f,40/255f,41/255f,255);
        atkText.text = null;
        atkText.color = new Color(6/255f,40/255f,41/255f,255);
        spdText.text = null;
        spdText.color = new Color(6/255f,40/255f,41/255f,255);
        dexText.text = null;
        dexText.color = new Color(6/255f,40/255f,41/255f,255);
        defText.text = null;
        defText.color = new Color(6/255f,40/255f,41/255f,255);
    }

    public void SetupRepresentation(Creature creature)
    {
        if (creature == null)
        {
            return;
        }
        
        creatureNameText.text = creature.CreatureName;
        powerLevelText.text = creature.CreatureStats.PowerLevel.ToString();
        hpText.text = creature.MaxHealth.ToString("n2");
        hpText.color = new Color(6/255f,40/255f,41/255f,255);
        atkText.text = creature.CreatureStats.Attack.ToString("n2");
        atkText.color = new Color(6/255f,40/255f,41/255f,255);
        spdText.text = creature.CreatureStats.Speed.ToString("n2");
        spdText.color = new Color(6/255f,40/255f,41/255f,255);
        dexText.text = creature.CreatureStats.Dexterity.ToString("n2");
        dexText.color = new Color(6/255f,40/255f,41/255f,255);
        defText.text = creature.CreatureStats.Defense.ToString("n2");
        defText.color = new Color(6/255f,40/255f,41/255f,255);
    }

    public void CompareColor(Creature creature1, Creature creature2)
    {

        Color pos = new Color(10/255f,71/255f,6/255f);
        Color neg = new Color(71/255f,5/255f,5/255f);
        
        if (creature1 == null || creature2 == null)
        {
            return;
        }


        if (creature1.MaxHealth != creature2.MaxHealth)
        {
            if (creature1.MaxHealth < creature2.MaxHealth)
            {
                hpText.color = neg;
            }
            else
            {
                hpText.color = pos;
            }
        }
        
        if (creature1.CreatureStats.Attack != creature2.CreatureStats.Attack)
        {
            if (creature1.CreatureStats.Attack < creature2.CreatureStats.Attack)
            {
                atkText.color = neg;
            }
            else
            {
                atkText.color = pos;
            }
        }
        
        if (creature1.CreatureStats.Speed != creature2.CreatureStats.Speed)
        {
            if (creature1.CreatureStats.Speed < creature2.CreatureStats.Speed)
            {
                spdText.color = neg;
            }
            else
            {
                spdText.color = pos;
            }
        }
        
        if (creature1.CreatureStats.Dexterity != creature2.CreatureStats.Dexterity)
        {
            if (creature1.CreatureStats.Dexterity < creature2.CreatureStats.Dexterity)
            {
                dexText.color = neg;
            }
            else
            {
                dexText.color = pos;
            }
        }
        
        if (creature1.CreatureStats.Defense != creature2.CreatureStats.Defense)
        {
            if (creature1.CreatureStats.Defense < creature2.CreatureStats.Defense)
            {
                defText.color = neg;
            }
            else
            {
                defText.color = pos;
            }
        }
        
    }

}
