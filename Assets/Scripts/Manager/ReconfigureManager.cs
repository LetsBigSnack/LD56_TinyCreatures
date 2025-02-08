using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Data;

public class ReconfigureManager : MonoBehaviour
{
    public static ReconfigureManager Instance;

    private Creature selectedCreature = null;

    [SerializeField] private List<BodyPartSet> bodyParts;

    [SerializeField] private List<BodyPartEntry> heads;
    [SerializeField] private List<BodyPartEntry> bodies;
    [SerializeField] private List<BodyPartEntry> arms;
    [SerializeField] private List<BodyPartEntry> legs;
    [SerializeField] private List<BodyPartEntry> topHeads;
    [SerializeField] private List<BodyPartEntry> tails;
    [SerializeField] private List<BodyPartEntry> backs;

    public List<BodyPartEntry> Heads { get => heads; }
    public List<BodyPartEntry> Bodies { get => bodies; }
    public List<BodyPartEntry> Arms { get => arms; }
    public List<BodyPartEntry> Legs { get => legs; }
    public List<BodyPartEntry> TopHeads { get => topHeads; }
    public List<BodyPartEntry> Backs { get => backs; }
    public List<BodyPartEntry> Tails { get => tails; }

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
        return true;
    }

    public bool ReconfigureSelectedCreature(CreatureRepresentation representation)
    {
        selectedCreature.Representation = representation;
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
        topHeads = bodyParts.SelectMany(bodyPartSet => bodyPartSet.bodyPartEntries).Where(bodyPart => bodyPart.bodyPartType == BodyPartType.TopHead && bodyPart.bodyPart.collected).ToList();
        backs = bodyParts.SelectMany(bodyPartSet => bodyPartSet.bodyPartEntries).Where(bodyPart => bodyPart.bodyPartType == BodyPartType.Back && bodyPart.bodyPart.collected).ToList();
        tails = bodyParts.SelectMany(bodyPartSet => bodyPartSet.bodyPartEntries).Where(bodyPart => bodyPart.bodyPartType == BodyPartType.Tail && bodyPart.bodyPart.collected).ToList();
    }

    public void ClearEntries()
    {
        heads.Clear();
        bodies.Clear();
        arms.Clear();
        legs.Clear();
        topHeads.Clear();
        backs.Clear();
        tails.Clear();
    }
}
