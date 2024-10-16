using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CreatureSprite : MonoBehaviour
{
    [SerializeField] private Image creatureHead;
    [SerializeField] private Image creatureBody;
    [SerializeField] private Image creatureArms;
    [SerializeField] private Image creatureLegs;


    public void Reset()
    {
        creatureHead.sprite = null;
        creatureHead.color = new Color(0f, 0f, 0f, 0f);
        creatureBody.sprite = null;
        creatureBody.color = new Color(0f, 0f, 0f, 0f);
        creatureLegs.sprite = null;
        creatureLegs.color = new Color(0f, 0f, 0f, 0f);
        creatureArms.sprite = null;
        creatureArms.color = new Color(0f, 0f, 0f, 0f);
    }
    
    public void SetupRepresentation(Creature creature)
    {
        if (creature == null)
        {
            return;
        }
        
        creatureHead.sprite = creature.Representation.HeadSprite;
        creatureHead.color = creature.Representation.HeadColor;
        
        creatureBody.sprite = creature.Representation.BodySprite;
        creatureBody.color = creature.Representation.BodyColor;
        
        creatureLegs.sprite = creature.Representation.LegsSprite;
        creatureLegs.color = creature.Representation.LegsColor;
        
        creatureArms.sprite = creature.Representation.ArmsSprite;
        creatureArms.color = creature.Representation.ArmsColor;
        
    }
    
}
