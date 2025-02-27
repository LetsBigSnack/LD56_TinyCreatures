using System.Collections;
using System.Collections.Generic;
using Data;
using Helper.Util;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CreatureDetailsText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI powerLevelText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI spdText;
    [SerializeField] private TextMeshProUGUI dexText;
    [SerializeField] private TextMeshProUGUI defText;
    [SerializeField] private GameObject[] attributes;
    
    private void OnEnable()
    {
        Reset();
    }
      
    public void Reset()
    {
        powerLevelText.text = "       ";
        hpText.text = "      ";
        hpText.color = new Color(255f, 255f, 255f, 255);
        atkText.text = "      ";
        atkText.color = new Color(255f, 255f, 255f, 255);
        spdText.text = "      ";
        spdText.color = new Color(255f, 255f, 255f, 255);
        dexText.text = "      ";
        dexText.color = new Color(255f, 255f, 255f, 255);
        defText.text = "      ";
        defText.color = new Color(255f, 255f, 255f, 255);
    }

    public void SetupRepresentation(Creature creature)
    {
        if (creature == null)
        {
            if (attributes.Length > 0)
            {
                for (int i = 0; i < attributes.Length; i++)
                {
                    attributes[i].SetActive(false);
                }
            }
            return;
        }

        if (attributes.Length > 0)
        {
            for (int i = 0; i < attributes.Length; i++)
            {
                attributes[i].SetActive(true);
            }
        }
        hpText.text = creature.MaxHealth.ToNumberSuffix(false);
        hpText.color = new Color(255f,255f,255f,255);
        atkText.text = creature.CreatureStats.Attack.ToNumberSuffix();
        atkText.color = new Color(255f, 255f, 255f, 255);
        spdText.text = creature.CreatureStats.Speed.ToNumberSuffix();
        spdText.color = new Color(255f, 255f, 255f, 255);
        dexText.text = creature.CreatureStats.Dexterity.ToNumberSuffix();
        dexText.color = new Color(255f, 255f, 255f, 255);
        defText.text = creature.CreatureStats.Defense.ToNumberSuffix();
        defText.color = new Color(255f, 255f, 255f, 255);
        powerLevelText.text = creature.CreatureStats.PowerLevel.ToNumberSuffix(false);
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
