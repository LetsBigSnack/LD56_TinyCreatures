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
        powerLevelText.text = creature.PowerLevel.ToString();
        hpText.text = creature.MaxHealth.ToString("n2");
        hpText.color = new Color(6/255f,40/255f,41/255f,255);
        atkText.text = creature.Attack.ToString("n2");
        atkText.color = new Color(6/255f,40/255f,41/255f,255);
        spdText.text = creature.Speed.ToString("n2");
        spdText.color = new Color(6/255f,40/255f,41/255f,255);
        dexText.text = creature.Dexterity.ToString("n2");
        dexText.color = new Color(6/255f,40/255f,41/255f,255);
        defText.text = creature.Defense.ToString("n2");
        defText.color = new Color(6/255f,40/255f,41/255f,255);
    }

    public void CompareColor(Creature creature1, Creature creature2)
    {

        Color pos = new Color(10/255f,27/255f,1/255f);
        Color neg = new Color(90/10f,13/255f,6/255f);
        
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
        
        if (creature1.Attack != creature2.Attack)
        {
            if (creature1.Attack < creature2.Attack)
            {
                atkText.color = neg;
            }
            else
            {
                atkText.color = pos;
            }
        }
        
        if (creature1.Speed != creature2.Speed)
        {
            if (creature1.Speed < creature2.Speed)
            {
                spdText.color = neg;
            }
            else
            {
                spdText.color = pos;
            }
        }
        
        if (creature1.Dexterity != creature2.Dexterity)
        {
            if (creature1.Dexterity < creature2.Dexterity)
            {
                dexText.color = neg;
            }
            else
            {
                dexText.color = pos;
            }
        }
        
        if (creature1.Defense != creature2.Defense)
        {
            if (creature1.Defense < creature2.Defense)
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
