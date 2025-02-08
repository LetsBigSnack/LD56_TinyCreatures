using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CreatureSprite : MonoBehaviour
{
    private Creature currCreature;

    [SerializeField] private Image creatureHead;
    [SerializeField] private Image creatureBody;
    [SerializeField] private Image creatureArms;
    [SerializeField] private Image creatureLegs;
    [SerializeField] private Image creatureTopHead;
    [SerializeField] private Image creatureBack;
    [SerializeField] private Image creatureTail;

    private void Start()
    {
        if (gameObject.GetComponent<UICreatureButton>() != null && gameObject.GetComponent<UICreatureButton>().Creature != null)
        {
            currCreature = gameObject.GetComponent<UICreatureButton>().Creature;
            SetupInitialRepresentation(currCreature);
        }
    }

    public Image CreatureHead
    {
        get => creatureHead;
        set => creatureHead = value;
    }
    public Image CreatureBody
    {
        get => creatureBody;
        set => creatureBody = value;
    }
    public Image CreatureArms
    {
        get => creatureArms;
        set => creatureArms = value;
    }
    public Image CreatureLegs
    {
        get => creatureLegs;
        set => creatureLegs = value;
    }
    public Image CreatureTopHead
    {
        get => creatureTopHead;
        set => creatureTopHead = value;
    }
    public Image CreatureBack
    {
        get => creatureBack;
        set => creatureBack = value;
    }
    public Image CreatureTail
    {
        get => creatureTail;
        set => creatureTail = value;
    }

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
        creatureTopHead.sprite = null;
        creatureTopHead.color = new Color(0f, 0f, 0f, 0f);
        creatureBack.sprite = null;
        creatureBack.color = new Color(0f, 0f, 0f, 0f);
        creatureTail.sprite = null;
        creatureTail.color = new Color(0f, 0f, 0f, 0f);
    }
    
    public void SetupInitialRepresentation(Creature creature)
    {
        if (creature == null)
        {
            return;
        }
        
        creatureHead.sprite = ColorManager.Instance.RepaintSprite(creature.Representation.HeadSprite, creature.Representation.BaseColor, creature.Representation.AddOnColor);
        creatureHead.color = new Color(255f, 255f, 255f, 255f);

        creatureBody.sprite = ColorManager.Instance.RepaintSprite(creature.Representation.BodySprite, creature.Representation.BaseColor, creature.Representation.AddOnColor);
        creatureBody.color = new Color(255f, 255f, 255f, 255f);

        creatureLegs.sprite = ColorManager.Instance.RepaintSprite(creature.Representation.LegsSprite, creature.Representation.BaseColor, creature.Representation.AddOnColor);
        creatureLegs.color = new Color(255f, 255f, 255f, 255f);

        creatureArms.sprite = ColorManager.Instance.RepaintSprite(creature.Representation.ArmsSprite, creature.Representation.BaseColor, creature.Representation.AddOnColor);
        creatureArms.color = new Color(255f, 255f, 255f, 255f);

        creatureTopHead.sprite = ColorManager.Instance.RepaintSprite(creature.Representation.TopHeadSprite, creature.Representation.BaseColor, creature.Representation.AddOnColor);
        creatureTopHead.color = new Color(255f, 255f, 255f, 255f);

        creatureBack.sprite = ColorManager.Instance.RepaintSprite(creature.Representation.BackSprite, creature.Representation.BaseColor, creature.Representation.AddOnColor);
        creatureBack.color = new Color(255f, 255f, 255f, 255f);

        creatureTail.sprite = ColorManager.Instance.RepaintSprite(creature.Representation.TailSprite, creature.Representation.BaseColor, creature.Representation.AddOnColor);
        creatureTail.color = new Color(255f, 255f, 255f, 255f);
    }

    public void SetupRepresentation(Creature creature)
    {
        if (creature == null)
        {
            return;
        }

        creatureHead.sprite = creature.Representation.HeadSprite;
        creatureHead.color = new Color(255f, 255f, 255f, 255f);

        creatureBody.sprite = creature.Representation.BodySprite;
        creatureBody.color = new Color(255f, 255f, 255f, 255f);

        creatureLegs.sprite = creature.Representation.LegsSprite;
        creatureLegs.color = new Color(255f, 255f, 255f, 255f);

        creatureArms.sprite = creature.Representation.ArmsSprite;
        creatureArms.color = new Color(255f, 255f, 255f, 255f);

        creatureTopHead.sprite = creature.Representation.TopHeadSprite;
        creatureTopHead.color = new Color(255f, 255f, 255f, 255f);

        creatureBack.sprite = creature.Representation.BackSprite;
        creatureBack.color = new Color(255f, 255f, 255f, 255f);

        creatureTail.sprite = creature.Representation.TailSprite;
        creatureTail.color = new Color(255f, 255f, 255f, 255f);
    }
    
}
