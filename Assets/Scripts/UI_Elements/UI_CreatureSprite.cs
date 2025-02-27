using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Data;
using UnityEngine.UI;

public class UI_CreatureSprite : MonoBehaviour
{
    private Creature currCreature;

    private BaseColor baseColor;
    private AddOnColor addOnColor;

    [SerializeField] private Image creatureHead;
    [SerializeField] private Image creatureBody;
    [SerializeField] private Image creatureArms;
    [SerializeField] private Image creatureLegs;
    [SerializeField] private Image creatureTopHead;
    [SerializeField] private Image creatureBack;
    [SerializeField] private Image creatureTail;

    private Material currentMaterial;

    public Creature CurrentCreature
    {
        get => currCreature;
        set => currCreature = value;
    }

    public BaseColor BaseColor
    {
        get => baseColor;
        set => baseColor = value;
    }

    public AddOnColor AddOnColor
    {
        get => addOnColor;
        set => addOnColor = value;
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

    public Material CurrentMaterial
    {
        get => currentMaterial;
        set => currentMaterial = value;
    }

    public void OnCreatureChanged(Creature creature)
    {
        baseColor = creature.Representation.BaseColor;
        addOnColor = creature.Representation.AddOnColor;
        currentMaterial = ColorManager.Instance.CreateNewColoredMaterial(baseColor, addOnColor);
        SetAllMaterials();
    }

    public void OnCreatureRemoved()
    {
        ResetAllMaterials();
        baseColor = null;
        addOnColor = null;
        currentMaterial = null;
    }

    public void ResetAllMaterials()
    {
        creatureTopHead.material = null;
        creatureHead.material = null;
        creatureArms.material = null;
        creatureBody.material = null;
        creatureLegs.material = null;
        creatureTail.material = null;
        creatureBack.material = null;
    }

    public void SetAllMaterials()
    {
        creatureTopHead.material = currentMaterial;
        creatureHead.material = currentMaterial;
        creatureArms.material = currentMaterial;
        creatureBody.material = currentMaterial;
        creatureLegs.material = currentMaterial;
        creatureTail.material = currentMaterial;
        creatureBack.material = currentMaterial;
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

        OnCreatureRemoved();
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

        OnCreatureChanged(creature);
    }
    
}
