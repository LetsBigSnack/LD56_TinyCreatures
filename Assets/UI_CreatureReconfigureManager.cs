using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UI_CreatureReconfigureManager : MonoBehaviour
{
    public static UI_CreatureReconfigureManager Instance;

    [SerializeField] private Creature selectedCreature = null;
    [SerializeField] private UI_CreatureSprite creaturePreviewSprite;

    [SerializeField] private List<BodyPartSet> bodyParts;

    [SerializeField] private List<BodyPartEntry> heads;
    [SerializeField] private List<BodyPartEntry> bodies;
    [SerializeField] private List<BodyPartEntry> arms;
    [SerializeField] private List<BodyPartEntry> legs;

    [SerializeField] private int headIndex = 0;
    [SerializeField] private int bodyIndex = 0;
    [SerializeField] private int armIndex = 0;
    [SerializeField] private int legIndex = 0;

    [SerializeField] private BodyPart currentHead = null;
    [SerializeField] private BodyPart currentBody = null;
    [SerializeField] private BodyPart currentArms = null;
    [SerializeField] private BodyPart currentLegs = null;

    [SerializeField] private UI_CreatureReconfigurItem headItem;
    [SerializeField] private UI_CreatureReconfigurItem bodyItem;
    [SerializeField] private UI_CreatureReconfigurItem armsItem;
    [SerializeField] private UI_CreatureReconfigurItem legsItem;

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

    private void OnEnable()
    {
        CreateEntries();
        SetImages();
    }

    private void OnDisable()
    {
        ClearEntries();
    }

    private void CreateEntries()
    {
        heads = new List<BodyPartEntry>(); // Ensure a new list
        foreach (var bodyPartSet in bodyParts)
        {
            foreach (var entry in bodyPartSet.bodyPartEntries)
            {
                if (entry.bodyPartType == BodyPartType.Head && entry.bodyPart.unlocked)
                {
                    heads.Add(entry);
                }
            }
        }
        bodies = bodyParts.SelectMany(bodyPartSet => bodyPartSet.bodyPartEntries).Where(bodyPart => bodyPart.bodyPartType == BodyPartType.Body && bodyPart.bodyPart.unlocked).ToList();
        arms = bodyParts.SelectMany(bodyPartSet => bodyPartSet.bodyPartEntries).Where(bodyPart => bodyPart.bodyPartType == BodyPartType.Arms && bodyPart.bodyPart.unlocked).ToList();
        legs = bodyParts.SelectMany(bodyPartSet => bodyPartSet.bodyPartEntries).Where(bodyPart => bodyPart.bodyPartType == BodyPartType.Legs && bodyPart.bodyPart.unlocked).ToList();
    }

    private void SetImages()
    {
        creaturePreviewSprite.CreatureHead.sprite = SetItemSprite(headItem, heads, headIndex);
        creaturePreviewSprite.CreatureBody.sprite = SetItemSprite(bodyItem, bodies, bodyIndex);
        creaturePreviewSprite.CreatureArms.sprite = SetItemSprite(armsItem, arms, armIndex);
        creaturePreviewSprite.CreatureLegs.sprite = SetItemSprite(legsItem, legs, legIndex);
    }

    private void ClearEntries()
    {
        heads.Clear();
        bodies.Clear();
        arms.Clear();
        legs.Clear();
    }

    private void CreaturePicked()
    {
        currentHead = selectedCreature.Representation.BodyParts.Where(bodyPart => bodyPart.Key == BodyPartType.Head).FirstOrDefault().Value;
        currentBody = selectedCreature.Representation.BodyParts.Where(bodyPart => bodyPart.Key == BodyPartType.Body).FirstOrDefault().Value;
        currentArms = selectedCreature.Representation.BodyParts.Where(bodyPart => bodyPart.Key == BodyPartType.Arms).FirstOrDefault().Value;
        currentLegs = selectedCreature.Representation.BodyParts.Where(bodyPart => bodyPart.Key == BodyPartType.Legs).FirstOrDefault().Value;

        headIndex = heads.FindIndex(head => head.bodyPart.Equals(currentHead));
        bodyIndex = bodies.FindIndex(head => head.bodyPart.Equals(currentBody));
        armIndex = arms.FindIndex(head => head.bodyPart.Equals(currentArms));
        legIndex = legs.FindIndex(head => head.bodyPart.Equals(currentLegs));

        headItem.CurrentPart.sprite = selectedCreature.Representation.HeadSprite;
        bodyItem.CurrentPart.sprite = selectedCreature.Representation.BodySprite;
        armsItem.CurrentPart.sprite = selectedCreature.Representation.ArmsSprite;
        legsItem.CurrentPart.sprite = selectedCreature.Representation.LegsSprite;
    }

    public void NextEntry(string part)
    {
        Debug.Log("next entry");
        switch (part)
        {
            case "head":
                headIndex = OutOfBoundPrevention(heads, headIndex, 1);
                SetItemSprite(headItem, heads, headIndex);
                creaturePreviewSprite.CreatureHead.sprite = SetItemSprite(headItem, heads, headIndex);
                break;

            case "body":
                bodyIndex = OutOfBoundPrevention(bodies, bodyIndex, 1);
                creaturePreviewSprite.CreatureBody.sprite = SetItemSprite(bodyItem, bodies, bodyIndex);
                break;

            case "arms":
                armIndex = OutOfBoundPrevention(arms, armIndex, 1);
                creaturePreviewSprite.CreatureArms.sprite = SetItemSprite(armsItem, arms, armIndex);
                break;

            case "legs":
                legIndex = OutOfBoundPrevention(legs, legIndex, 1);
                creaturePreviewSprite.CreatureLegs.sprite = SetItemSprite(legsItem, legs, legIndex);
                break;
        }
    }

    public void PreviousEntry(string part)
    {
        Debug.Log("previousEntry");
        switch (part)
        {
            case "head":
                headIndex = OutOfBoundPrevention(heads, headIndex, -1);
                creaturePreviewSprite.CreatureHead.sprite = SetItemSprite(headItem, heads, headIndex);
                break;

            case "body":
                bodyIndex = OutOfBoundPrevention(bodies, bodyIndex, -1);
                creaturePreviewSprite.CreatureBody.sprite = SetItemSprite(bodyItem, bodies, bodyIndex);
                break;

            case "arms":
                armIndex = OutOfBoundPrevention(arms, armIndex, -1);
                creaturePreviewSprite.CreatureArms.sprite = SetItemSprite(armsItem, arms, armIndex);
                break;

            case "legs":
                legIndex = OutOfBoundPrevention(legs, legIndex, -1);
                creaturePreviewSprite.CreatureLegs.sprite = SetItemSprite(legsItem, legs, legIndex);
                break;
        }
    }

    private Sprite SetItemSprite(UI_CreatureReconfigurItem item, List<BodyPartEntry> parts, int currentIndex)
    {
       return item.CurrentPart.sprite = parts[currentIndex].bodyPart.bodyPartSprite;
    }

    private int OutOfBoundPrevention(List<BodyPartEntry> parts, int currentIndex, int value)
    {
        if (currentIndex + value > parts.Count()-1)
        {
            return 0;
        }
        else if (currentIndex + value < 0)
        {
            return parts.Count()-1;
        }
        else
        {
           return  currentIndex + value;
        }
    }
}
