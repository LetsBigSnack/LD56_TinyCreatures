using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using Data;
using Newtonsoft.Json;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;
    private string _savePath;
    
    private SaveState _saveSlot1;
    private SaveState _saveSlot2;
    private SaveState _saveSlot3;

    public SaveState SaveSlot1
    {
        get => _saveSlot1;
        set => _saveSlot1 = value;
    }

    public SaveState SaveSlot2
    {
        get => _saveSlot2;
        set => _saveSlot2 = value;
    }

    public SaveState SaveSlot3
    {
        get => _saveSlot3;
        set => _saveSlot3 = value;
    }

    private int _saveIndex = -1;
    
    public int SaveIndex
    {
        get => _saveIndex;
        set => _saveIndex = value;
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _savePath = Application.persistentDataPath;
            LoadSaveSlots();
            if (_saveIndex != -1)
            {
                UI_SaveLoadManager.Instance.SetSelectedSlot(_saveIndex);
                LoadGame();
            }
            else
            {
                UI_SaveLoadManager.Instance.SetActiveSelectScreen(true);
            }
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
    
    private void LoadSaveSlots()
    {
        DateTime latestSaveTime = DateTime.MinValue;
        
        for (int i = 0; i < 3; i++) // Start from slot 3 and go down to 1
        {
            string filePath = $"{_savePath}/SaveSlot{i}.json";
            if (File.Exists(filePath))
            {
                DateTime saveTime = File.GetLastWriteTime(filePath);
                
                if (saveTime > latestSaveTime)
                {
                    latestSaveTime = saveTime;
                    _saveIndex = i;
                }
                
                string json = File.ReadAllText(filePath);
                SaveState saveState = JsonConvert.DeserializeObject<SaveState>(json);
                
                switch (i)
                {
                    case 0: _saveSlot1 = saveState; break;
                    case 1: _saveSlot2 = saveState; break;
                    case 2: _saveSlot3 = saveState; break;
                }
                
            }
        }
        
    }

    public void SaveGame()
    {
        
        SaveState tempSaveState;
        switch (_saveIndex)
        {
            case 0:
                tempSaveState = _saveSlot1;
                break;
            case 1: 
                tempSaveState = _saveSlot2;
                break;
            case 2: 
                tempSaveState = _saveSlot3;
                break;
            default:
                Debug.LogWarning("Invalid slot number provided.");
                return;
        }
        
        SaveState saveState = new SaveState();
        saveState.InitializeDefaults();
        saveState.saveName = tempSaveState.saveName;
        saveState.savedSets = CreatureManager.Instance.BodyPartSets
            .Where(set => set.unlocked)
            .Select(set => new TransientBodyPartSet
            {
                setName = set.setName,
                unlocked = set.unlocked,
                bodyParts = set.bodyPartEntries.Select(entry => new TransientBodyPart
                {
                    bodyPartName = entry.bodyPart.bodyPartName,
                    collected = entry.bodyPart.collected
                }).ToList()
            }).ToList();
        
        saveState.playerWins = BattleManager.Instance.PlayerWins;
        saveState.enemyCreature = BattleManager.Instance.EnemyCreature;
        saveState.autoBattle = BattleManager.Instance.AutoBattle;
        
        saveState.tutorialData = TutorialManager.Instance.TutorialData;
        
        saveState.boughtSlots = StoreManager.Instance.BoughtSlots;
        saveState.playerMoney = StoreManager.Instance.PlayerMoney;
        saveState.soledCreatures = StoreManager.Instance.SoldCreatures;
        saveState.inventorySpace = InventoryManager.Instance.InventorySpace;
        
        saveState.inventory = InventoryManager.Instance.InventoryCreatures;
        
        saveState.selectedCreatureBattle = InventoryManager.Instance.SelectedCreatureForBattle;
        saveState.selectedCreaturePodOne = BreedingManager.Instance.CreaturePod1;
        saveState.selectedCreaturePodTwo = BreedingManager.Instance.CreaturePod2;
        saveState.breedingCreatureResult = BreedingManager.Instance.Result;
        saveState.selectedCreatureReconfigure = InventoryManager.Instance.SelectedCreatureForReConfigure;

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };
        
        string json = JsonConvert.SerializeObject(saveState, settings);
        File.WriteAllText(_savePath+"/SaveSlot"+_saveIndex+".json", json);
        Debug.Log("Saved!" + _savePath);
    }
    
    public void LoadGame()
    {
        ResetCollectedAndUnlockedStates();
        
        SaveState loadedSaveState = null;
        
        switch (_saveIndex)
        {
            case 0:
                loadedSaveState = _saveSlot1;
                break;
            case 1:
                loadedSaveState = _saveSlot2;
                break;
            case 2:
                loadedSaveState = _saveSlot3;
                break;
            default:
                Debug.LogWarning("Invalid save slot selected. Please select a valid slot.");
                return;
        }

        if (loadedSaveState == null)
        {
            Debug.LogError("Save data is null. Unable to load game.");
            return;
        }

  
        foreach (var bodyPartSet in CreatureManager.Instance.BodyPartSets)
        {
            var savedSet = loadedSaveState.savedSets.FirstOrDefault(set => set.setName == bodyPartSet.setName);
            if (savedSet != null)
            {
                bodyPartSet.unlocked = savedSet.unlocked;

                foreach (var entry in bodyPartSet.bodyPartEntries)
                {
                    var savedPart = savedSet.bodyParts.FirstOrDefault(part => part.bodyPartName == entry.bodyPart.bodyPartName);
                    if (savedPart != null)
                    {
                        entry.bodyPart.collected = savedPart.collected;
                    }
                    else
                    {
                        entry.bodyPart.collected = false;
                    }
                }
            }
        }
        
        TutorialManager.Instance.TutorialData = loadedSaveState.tutorialData;
        
        BattleManager.Instance.PlayerWins = loadedSaveState.playerWins;
        BattleManager.Instance.EnemyCreature = loadedSaveState.enemyCreature;
        BattleManager.Instance.AutoBattle = loadedSaveState.autoBattle;

        StoreManager.Instance.BoughtSlots = loadedSaveState.boughtSlots;
        StoreManager.Instance.PlayerMoney = loadedSaveState.playerMoney;
        StoreManager.Instance.SoldCreatures = loadedSaveState.soledCreatures;

        InventoryManager.Instance.InventorySpace = loadedSaveState.inventorySpace;
        InventoryManager.Instance.InventoryCreatures = loadedSaveState.inventory;

        InventoryManager.Instance.SelectedCreatureForBattle = loadedSaveState.selectedCreatureBattle;
        BreedingManager.Instance.CreaturePod1 = loadedSaveState.selectedCreaturePodOne;
        BreedingManager.Instance.CreaturePod2 = loadedSaveState.selectedCreaturePodTwo;
        BreedingManager.Instance.Result = loadedSaveState.breedingCreatureResult;
        InventoryManager.Instance.SelectedCreatureForReConfigure = loadedSaveState.selectedCreatureReconfigure;

        Debug.Log("Game loaded successfully from slot " + _saveIndex);
    }

    public void DeleteSlot(int slotNumber)
    {
        string filePath = $"{_savePath}/SaveSlot{slotNumber}.json";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"Save slot {slotNumber} deleted successfully.");
            switch (slotNumber)
            {
                case 0:
                    _saveSlot1 = null;
                    break;
                case 1:
                    _saveSlot2 = null;
                    break;
                case 2:
                    _saveSlot3 = null;
                    break;
                default:
                    Debug.LogWarning("Invalid slot number provided.");
                    return;
            }
            if (_saveIndex == slotNumber)
            {
                _saveIndex = -1;
                Debug.Log("Most recent save slot deleted. No save slot currently loaded.");
            }
        }
    }

    public void SelectSlot(int slotNumber)
    {
        string filePath = $"{_savePath}/SaveSlot{slotNumber}.json";

        if (!SaveStateExists(slotNumber))
        {
            ResetCollectedAndUnlockedStates();
            SaveState newSaveState = new SaveState();
            newSaveState.InitializeDefaults();
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };
            string json = JsonConvert.SerializeObject(newSaveState, settings);
            File.WriteAllText(filePath, json);
            Debug.Log($"New save slot created at {filePath}");
            switch (slotNumber)
            {
                case 0: _saveSlot1 = newSaveState; break;
                case 1: _saveSlot2 = newSaveState; break;
                case 2: _saveSlot3 = newSaveState; break;
                default:
                    Debug.LogWarning("Invalid slot number provided.");
                    return;
            }
        }
        else
        {
            string json = File.ReadAllText(filePath);
            SaveState loadedSaveState = JsonConvert.DeserializeObject<SaveState>(json);
            switch (slotNumber)
            {
                case 0: _saveSlot1 = loadedSaveState; break;
                case 1: _saveSlot2 = loadedSaveState; break;
                case 2: _saveSlot3 = loadedSaveState; break;
                default:
                    Debug.LogWarning("Invalid slot number provided.");
                    return;
            }

            Debug.Log($"Save slot {slotNumber} loaded successfully.");
        }
        _saveIndex = slotNumber;
        UI_SaveLoadManager.Instance.SetSelectedSlot(slotNumber);
        LoadGame();
        
    }

    public bool SaveStateExists(int slotNumber)
    {
        string filePath = $"{_savePath}/SaveSlot{slotNumber}.json";

        if (File.Exists(filePath))
        {
            return true ;
        }
        return false ;
    }
    
    private void ResetCollectedAndUnlockedStates()
    {
        foreach (var bodyPartSet in CreatureManager.Instance.BodyPartSets)
        {
            if (!bodyPartSet.unlockedByDefault)
            {
                bodyPartSet.unlocked = false; // Reset set to locked
            }
            foreach (var entry in bodyPartSet.bodyPartEntries)
            {
                entry.bodyPart.collected = false; // Reset part to uncollected
            }
        }
    }

    public void RenameSaveSlot(string saveName, int slotIndex)
    {
        SaveState tempSaveState;
        switch (slotIndex)
        {
            case 0:
                tempSaveState = _saveSlot1;
                break;
            case 1: 
                tempSaveState = _saveSlot2;
                break;
            case 2: 
                tempSaveState = _saveSlot3;
                break;
            default:
                Debug.LogWarning("Invalid slot number provided.");
                return;
        }
        
        //FAIL SAVE
        tempSaveState.saveName = saveName;
        
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };
        
        string json = JsonConvert.SerializeObject(tempSaveState, settings);
        File.WriteAllText(_savePath+"/SaveSlot"+slotIndex+".json", json);
    }

    private void OnDestroy()
    {
        if (_saveIndex != -1)
        {
            SaveGame();
        }
    }
}
