using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ReconfigureManager : MonoBehaviour
{
    public static ReconfigureManager Instance;

    private Creature selectedCreature = null;

    [SerializeField] private List<BodyPartSet> bodyParts;

    [SerializeField] private List<BodyPartEntry> heads;
    [SerializeField] private List<BodyPartEntry> bodies;
    [SerializeField] private List<BodyPartEntry> arms;
    [SerializeField] private List<BodyPartEntry> legs;

    [SerializeField] private BodyPart currentHead = null;
    [SerializeField] private BodyPart currentBody = null;
    [SerializeField] private BodyPart currentArms = null;
    [SerializeField] private BodyPart currentLegs = null;

    public List<BodyPartEntry> Heads { get => heads; }
    public List<BodyPartEntry> Bodies { get => bodies; }
    public List<BodyPartEntry> Arms { get => arms; }
    public List<BodyPartEntry> Legs { get => legs; }
    
    public BodyPart CurrentHead
    {
        get => currentHead;
        set => currentHead = value;
    }

    public BodyPart CurrentBody
    {
        get => currentBody;
        set => currentBody = value;
    }

    public BodyPart CurrentArms
    {
        get => currentArms;
        set => currentArms = value;
    }

    public BodyPart CurrentLegs
    {
        get => currentLegs;
        set => currentLegs = value;
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

    private void Start()
    {
        if(InventoryManager.Instance.SelectedCreatureForReConfigure != null)
        {
            selectedCreature = InventoryManager.Instance.SelectedCreatureForReConfigure;
        }
    }

    public bool AddToReconfigure(Creature creature)
    {
        selectedCreature = creature;
        CreaturePicked(creature);
        return true;
    }

    public bool ReconfigureSelectedCreature()
    {

        if(currentHead != null) selectedCreature.Representation.BodyParts[BodyPartType.Head] = currentHead;

        if(currentBody != null) selectedCreature.Representation.BodyParts[BodyPartType.Body] = currentBody;

        if(currentArms != null) selectedCreature.Representation.BodyParts[BodyPartType.Arms] = currentArms;

        if(currentLegs != null) selectedCreature.Representation.BodyParts[BodyPartType.Legs] = currentLegs;

        InventoryManager.Instance.RemoveFromReconfigure(selectedCreature);
        selectedCreature = null;
        return true;
    }

    public bool RemoveFromReconfigure()
    {
        if (selectedCreature != null)
        {
            InventoryManager.Instance.RemoveFromReconfigure(selectedCreature);
            selectedCreature = null;
            return true;
        }
        return false;
    }  

    public void CreateEntries()
    {
        heads = bodyParts.SelectMany(bodyPartSet => bodyPartSet.bodyPartEntries).Where(bodyPart => bodyPart.bodyPartType == BodyPartType.Head && bodyPart.bodyPart.collected).ToList();
        bodies = bodyParts.SelectMany(bodyPartSet => bodyPartSet.bodyPartEntries).Where(bodyPart => bodyPart.bodyPartType == BodyPartType.Body && bodyPart.bodyPart.collected).ToList();
        arms = bodyParts.SelectMany(bodyPartSet => bodyPartSet.bodyPartEntries).Where(bodyPart => bodyPart.bodyPartType == BodyPartType.Arms && bodyPart.bodyPart.collected).ToList();
        legs = bodyParts.SelectMany(bodyPartSet => bodyPartSet.bodyPartEntries).Where(bodyPart => bodyPart.bodyPartType == BodyPartType.Legs && bodyPart.bodyPart.collected).ToList();
    }

    public void SetCurrentParts(BodyPartType partType, int index)
    {
        switch (partType)
        {
            case BodyPartType.Head:
                currentHead = heads[index].bodyPart;
                break;
            case BodyPartType.Body:
                currentBody = bodies[index].bodyPart;
                break;
            case BodyPartType.Arms:
                currentArms = arms[index].bodyPart;
                break;
            case BodyPartType.Legs:
                currentLegs = legs[index].bodyPart;
                break;
        }
    }

    public void ClearEntries()
    {
        heads.Clear();
        bodies.Clear();
        arms.Clear();
        legs.Clear();
    }

    public void CreaturePicked(Creature creature)
    {
        currentHead = creature.Representation.BodyParts.Where(bodyPart => bodyPart.Key == BodyPartType.Head).FirstOrDefault().Value;
        currentBody = creature.Representation.BodyParts.Where(bodyPart => bodyPart.Key == BodyPartType.Body).FirstOrDefault().Value;
        currentArms = creature.Representation.BodyParts.Where(bodyPart => bodyPart.Key == BodyPartType.Arms).FirstOrDefault().Value;
        currentLegs = creature.Representation.BodyParts.Where(bodyPart => bodyPart.Key == BodyPartType.Legs).FirstOrDefault().Value;

        UI_CreatureReconfigureManager.Instance.CreaturePicked(creature);
    }

    public Sprite ReturnSelectedRepresentation(BodyPartType partType)
    {
        Sprite spriteToReturn;
        switch (partType)
        {
            case BodyPartType.Head:
                spriteToReturn = selectedCreature.Representation.HeadSprite;
                break;
            case BodyPartType.Body:
                spriteToReturn = selectedCreature.Representation.BodySprite;
                break;
            case BodyPartType.Arms:
                spriteToReturn = selectedCreature.Representation.ArmsSprite;
                break;
            case BodyPartType.Legs:
                spriteToReturn = selectedCreature.Representation.LegsSprite;
                break;
            default:
                spriteToReturn = null;
                break;
        }
        return spriteToReturn;
    }

    public int ReturnIndex(BodyPartType partType)
    {
        int indexToReturn;
        switch (partType)
        {
            case BodyPartType.Head:
                indexToReturn = heads.FindIndex(head => head.bodyPart.Equals(currentHead));
                break;
            case BodyPartType.Body:
                indexToReturn = bodies.FindIndex(head => head.bodyPart.Equals(currentBody));
                break;
            case BodyPartType.Arms:
                indexToReturn = arms.FindIndex(head => head.bodyPart.Equals(currentArms));
                break;
            case BodyPartType.Legs:
                indexToReturn = legs.FindIndex(head => head.bodyPart.Equals(currentLegs));
                break;
            default:
                indexToReturn = 0;
                break;
        }
        return indexToReturn;
    }
}
