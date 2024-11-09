using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Numerics;
using Data;
using Newtonsoft.Json;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;
    
    private string _savePath;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _savePath = Application.persistentDataPath;
            //SaveGame();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }


    public void SaveGame()
    {
        Creature creature = CreatureManager.Instance.CreateBasicCreature();
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };
        
        string json = JsonConvert.SerializeObject(creature, settings);
        File.WriteAllText(_savePath+"/Creaturesave.json", json);
        Debug.Log("Saved!" + _savePath);
        
    }

    public Creature LoadCreature()
    {
        Creature test = JsonConvert.DeserializeObject<Creature>(File.ReadAllText(_savePath+"/Creaturesave.json"));
        test.CurrentHealth = test.MaxHealth;
        Debug.Log(test);
        return test;
    }
    
    
    public void LoadGame()
    {
        throw new System.NotImplementedException();
    }

    public void StoreInventory()
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };
        
        string json = JsonConvert.SerializeObject(InventoryManager.Instance.InventoryCreatures, settings);
        File.WriteAllText(_savePath+"/Inventory.json", json);
        Debug.Log("Saved!" + _savePath);
    }
    public List<Creature> LoadInventory()
    {
        List<Creature> test = JsonConvert.DeserializeObject<List<Creature>>(File.ReadAllText(_savePath+"/Inventory.json"));
        Debug.Log(test);
        return test;
    }
}
