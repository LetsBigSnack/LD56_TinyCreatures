using System.Collections;
using System.Collections.Generic;
using System.IO;
using Data;
using Newtonsoft.Json;
using UnityEngine;

public class JSON_AchievementsTEST : MonoBehaviour
{
    void Start()
    {
        string filePath = $"{Application.persistentDataPath}/Achievements.json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            AchievementJSON test = JsonConvert.DeserializeObject<AchievementJSON>(json);
            Debug.Log("DEKI is CUTE");
        }
    }
}
