using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BodyPartEntry
{
    public BodyPartType bodyPartType;
    public BodyPart bodyPart;
}

[CreateAssetMenu(fileName = "NewBodyPartSet", menuName = "Game/BodyPartSet")]
public class BodyPartSet : ScriptableObject
{
    public List<BodyPartEntry> bodyPartEntries;
    public bool unlocked = false;
    public string setName = "";
    public string setText = "";
}

