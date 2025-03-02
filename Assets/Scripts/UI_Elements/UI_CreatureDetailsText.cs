using System.Collections;
using System.Collections.Generic;
using Data;
using Helper.Util;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CreatureDetailsText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI powerLevelText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI spdText;
    [SerializeField] private TextMeshProUGUI dexText;
    [SerializeField] private TextMeshProUGUI defText;
    [SerializeField] private GameObject[] attributes;

    [SerializeField] private Material baseMaterial;
    [SerializeField] private Material upMaterial;
    [SerializeField] private Material downMaterial;

    private void OnEnable()
    {
        Reset();
    }
      
    public void Reset()
    {
        if(nameText != null)
        {
            nameText.text = "";
        }
        powerLevelText.text = "       ";
        hpText.text = "      ";
        hpText.color = new Color(255f, 255f, 255f, 255);
        hpText.fontSharedMaterial = baseMaterial;
        atkText.text = "      ";
        atkText.color = new Color(255f, 255f, 255f, 255);
        atkText.fontSharedMaterial = baseMaterial;
        spdText.text = "      ";
        spdText.color = new Color(255f, 255f, 255f, 255);
        spdText.fontSharedMaterial = baseMaterial;
        dexText.text = "      ";
        dexText.color = new Color(255f, 255f, 255f, 255);
        dexText.fontSharedMaterial = baseMaterial;
        defText.text = "      ";
        defText.color = new Color(255f, 255f, 255f, 255);
        defText.fontSharedMaterial = baseMaterial;
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
        if(nameText != null)
        {
            nameText.text = creature.CreatureName;
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
        if (creature1 == null || creature2 == null)
        {
            return;
        }


        if (creature1.MaxHealth != creature2.MaxHealth)
        {
            if (creature1.MaxHealth < creature2.MaxHealth)
            {
                hpText.fontSharedMaterial = downMaterial;
            }
            else
            {
                hpText.fontSharedMaterial = upMaterial;
            }
        }
        
        if (creature1.CreatureStats.Attack != creature2.CreatureStats.Attack)
        {
            if (creature1.CreatureStats.Attack < creature2.CreatureStats.Attack)
            {
                atkText.fontSharedMaterial = downMaterial;
            }
            else
            {
                atkText.fontSharedMaterial = upMaterial;
            }
        }
        
        if (creature1.CreatureStats.Speed != creature2.CreatureStats.Speed)
        {
            if (creature1.CreatureStats.Speed < creature2.CreatureStats.Speed)
            {
                spdText.fontSharedMaterial = downMaterial;
            }
            else
            {
                spdText.fontSharedMaterial = upMaterial;
            }
        }
        
        if (creature1.CreatureStats.Dexterity != creature2.CreatureStats.Dexterity)
        {
            if (creature1.CreatureStats.Dexterity < creature2.CreatureStats.Dexterity)
            {
                dexText.fontSharedMaterial = downMaterial;
            }
            else
            {
                dexText.fontSharedMaterial = upMaterial;
            }
        }
        
        if (creature1.CreatureStats.Defense != creature2.CreatureStats.Defense)
        {
            if (creature1.CreatureStats.Defense < creature2.CreatureStats.Defense)
            {
                defText.fontSharedMaterial = downMaterial;
            }
            else
            {
                defText.fontSharedMaterial = upMaterial;
            }
        }
        
    }

}
