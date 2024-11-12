using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


[System.Serializable]
public class BodyPartEntry
{
    public BodyPartType bodyPartType;
    public BodyPart bodyPart;
}

[CreateAssetMenu(fileName = "NewBodyPartSet", menuName = "Game/BodyPartSet")]
[Serializable]
[JsonObject(MemberSerialization.OptIn)]
public class BodyPartSet : ScriptableObject
{
    public List<BodyPartEntry> bodyPartEntries;
    public bool unlocked;
    public bool unlockedByDefault;
    [JsonProperty]
    public string setName;
    public string setText;
}

