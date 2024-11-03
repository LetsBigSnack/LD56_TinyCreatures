using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UI_CreatureDexManager : MonoBehaviour
{
    public static UI_CreatureDexManager Instance;

    //list of all sets existing
    [SerializeField] private List<BodyPartSet> bodySets;
    [SerializeField] private List<GameObject> displayedSets;
    [SerializeField] private Dictionary<int, BodyPartSet> indexedBodyParts = new Dictionary<int, BodyPartSet>();

    //prefabs depending on state of set 
    [SerializeField] private GameObject unlockedDexEntryItem;
    [SerializeField] private GameObject lockedDexEntryItem;

    //parent to instantiate
    [SerializeField] private GameObject scrollViewParent;

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
    }

    private void OnDisable()
    {
        indexedBodyParts.Clear();
        ClearEntries();
    }

    //creating an entry based on the status of the bodySet
    private void CreateEntries()
    {
        foreach(BodyPartSet bodySet in bodySets)
        {
            GameObject newBodySetItem;

            newBodySetItem = bodySet.unlocked? Instantiate(unlockedDexEntryItem) : Instantiate(lockedDexEntryItem);
            newBodySetItem.transform.SetParent(scrollViewParent.transform, false);
            SetEntry(newBodySetItem, bodySet, bodySet.unlocked);
            displayedSets.Add(newBodySetItem);
            indexedBodyParts.Add(displayedSets.FindIndex(item => item.Equals(newBodySetItem)), bodySet);            
        }
    }

    private void SetEntry(GameObject uiElement, BodyPartSet bodySet, bool unlocked)
    {
        if (unlocked)
        {
            uiElement.GetComponent<UI_CreatureDexEntryItem>().BodySet = bodySet;
            return;
        }

        uiElement.GetComponent<UI_CreatureDexEntryItemLocked>().BodySet = bodySet;
    }

    private void ClearEntries()
    {
        foreach (GameObject entry in displayedSets)
        {
            Destroy(entry);
        }
        displayedSets.Clear();
    }

    public void UnlockEntry(BodyPartSet bodySet)
    {
        int index = indexedBodyParts.Where(indexedPart => indexedPart.Value.Equals(bodySet)).FirstOrDefault().Key;

        GameObject newUnlockedEntry = Instantiate(unlockedDexEntryItem);
        newUnlockedEntry.transform.SetParent(scrollViewParent.transform, false);
        newUnlockedEntry.transform.SetSiblingIndex(index);
        SetEntry(newUnlockedEntry, bodySet, bodySet.unlocked);

        Destroy(displayedSets[index]);
        displayedSets[index] = newUnlockedEntry;
    }

    public void ResetEntries()
    {
        ClearEntries();
        CreateEntries();
    }
}
