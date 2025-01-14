using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Runtime.InteropServices;
using Data;
using Manager;
using Newtonsoft.Json;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveLoadManager : MonoBehaviour
{
    
    [DllImport("__Internal")]
    private static extern void JS_SaveGameToIndexedDB(string jsonString, int slotIndex);

    [DllImport("__Internal")]
    private static extern string JS_LoadSaveSlotFromIndexedDB(int slotIndex);

    [DllImport("__Internal")]
    private static extern bool JS_DeleteSaveFromIndexedDB(int slotIndex);

    [DllImport("__Internal")]
    private static extern int JS_SaveStateExistsInIndexedDB(int slotIndex);
    
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
            LoadAllSaveSlots();
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
    
    private void LoadAllSaveSlots()
    {

        for (int i = 0; i < 3; i++) // Start from slot 3 and go down to 1
        {

            SaveState saveState = LoadSaveSlot(i);
            
            switch (i)
            {
                case 0: _saveSlot1 = saveState; break;
                case 1: _saveSlot2 = saveState; break;
                case 2: _saveSlot3 = saveState; break;
            }
        }
        
        List<SaveState> tempSaveStates = new List<SaveState>();
        tempSaveStates.Add(_saveSlot1);
        tempSaveStates.Add(_saveSlot2);
        tempSaveStates.Add(_saveSlot3);
        tempSaveStates = tempSaveStates
            .Where(element => element != null)
            .ToList();

        if (tempSaveStates.Count <= 0)
        {
            return;
        }
        
        SaveState indexElement = tempSaveStates.OrderByDescending(c => c.lastUpdate).First();
        
        _saveIndex = tempSaveStates.IndexOf(indexElement);

    }

    private SaveState LoadSaveSlot(int slotIndex)
    {
        SaveState saveState;
        #if !UNITY_EDITOR && UNITY_WEBGL
                saveState = LoadSaveSlotFromIndexedDB(slotIndex);
        #else
                saveState = LoadSaveSlotFromFile(slotIndex);
        #endif
        
        return saveState;
    }

    private SaveState LoadSaveSlotFromFile(int slotIndex)
    {
        string filePath = $"{_savePath}/SaveSlot{slotIndex}.json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<SaveState>(json);
        }
        return null;
    }

    private SaveState LoadSaveSlotFromIndexedDB(int slotIndex)
    {
        Debug.Log("Unity Call for Slot"+slotIndex+"::"+SaveStateExistsInIndexedDB(slotIndex));
        if (SaveStateExistsInIndexedDB(slotIndex))
        {
            string json = JS_LoadSaveSlotFromIndexedDB(slotIndex);
            return JsonConvert.DeserializeObject<SaveState>(json);
        }
        return null;
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
        saveState.material_1 = InventoryManager.Instance.MaterialA;
        saveState.material_2 = InventoryManager.Instance.MaterialB;
        saveState.material_3 = InventoryManager.Instance.MaterialC;
        saveState.material_4 = InventoryManager.Instance.MaterialD;

        saveState.materialA = MaterialManager.Instance.MaterialStatsA;
        saveState.materialB = MaterialManager.Instance.MaterialStatsB;
        saveState.materialC = MaterialManager.Instance.MaterialStatsC;
        saveState.materialD = MaterialManager.Instance.MaterialStatsD;

        saveState.selectedCreatureBattle = InventoryManager.Instance.SelectedCreatureForBattle;
        saveState.selectedCreaturePodOne = BreedingManager.Instance.CreaturePod1;
        saveState.selectedCreaturePodTwo = BreedingManager.Instance.CreaturePod2;
        saveState.breedingCreatureResult = BreedingManager.Instance.Result;
        saveState.selectedCreatureReconfigure = InventoryManager.Instance.SelectedCreatureForReConfigure;

        saveState.selectedCreatureMaterial_1 = InventoryManager.Instance.SelectedCreatureForMaterial_1;
        saveState.selectedCreatureMaterial_2 = InventoryManager.Instance.SelectedCreatureForMaterial_2;
        saveState.selectedCreatureMaterial_3 = InventoryManager.Instance.SelectedCreatureForMaterial_3;
        saveState.selectedCreatureMaterial_4 = InventoryManager.Instance.SelectedCreatureForMaterial_4;

        saveState.achievement = AchievementManager.Instance.AchievementJson;

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };
        
        string json = JsonConvert.SerializeObject(saveState, settings);
        
        SaveSlot(json);

    }

    private void SaveSlot(string jsonString, int slotIndex = -1)
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
            SaveGameToIndexedDB(jsonString, slotIndex);
        #else
            SaveGameToFile(jsonString, slotIndex);
        #endif
    }

    private void SaveGameToFile(string jsonString, int slotIndex = -1)
    {
        if (slotIndex == -1)
        {
            slotIndex = _saveIndex;
        }
        
        File.WriteAllText(_savePath+"/SaveSlot"+slotIndex+".json", jsonString);
        Debug.Log("Saved!" + _savePath);
    }

    private void SaveGameToIndexedDB(string jsonString, int slotIndex = -1)
    {
        if (slotIndex == -1) slotIndex = _saveIndex;
        JS_SaveGameToIndexedDB(jsonString, slotIndex);
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
        InventoryManager.Instance.MaterialA = loadedSaveState.material_1;
        InventoryManager.Instance.MaterialB = loadedSaveState.material_2;
        InventoryManager.Instance.MaterialC = loadedSaveState.material_3;
        InventoryManager.Instance.MaterialD = loadedSaveState.material_4;

        MaterialManager.Instance.MaterialStatsA = loadedSaveState.materialA;
        MaterialManager.Instance.MaterialStatsB = loadedSaveState.materialB;
        MaterialManager.Instance.MaterialStatsC = loadedSaveState.materialC;
        MaterialManager.Instance.MaterialStatsD = loadedSaveState.materialD;

        InventoryManager.Instance.SelectedCreatureForBattle = loadedSaveState.selectedCreatureBattle;
        BreedingManager.Instance.CreaturePod1 = loadedSaveState.selectedCreaturePodOne;
        BreedingManager.Instance.CreaturePod2 = loadedSaveState.selectedCreaturePodTwo;
        BreedingManager.Instance.Result = loadedSaveState.breedingCreatureResult;
        InventoryManager.Instance.SelectedCreatureForReConfigure = loadedSaveState.selectedCreatureReconfigure;

        InventoryManager.Instance.SelectedCreatureForMaterial_1 = loadedSaveState.selectedCreatureMaterial_1;
        InventoryManager.Instance.SelectedCreatureForMaterial_2 = loadedSaveState.selectedCreatureMaterial_2;
        InventoryManager.Instance.SelectedCreatureForMaterial_3 = loadedSaveState.selectedCreatureMaterial_3;
        InventoryManager.Instance.SelectedCreatureForMaterial_4 = loadedSaveState.selectedCreatureMaterial_4;

        // Clear for you

        InventoryManager.Instance.CreatureInspectorLeft = null;
        InventoryManager.Instance.CreatureInspectorRight = null;

        BreedingManager.Instance.UpdatePrice();
        
        //
        AchievementManager.Instance.UnsubscribeAll();
        AchievementManager.Instance.AchievementJson = loadedSaveState.achievement;
        AchievementManager.Instance.SubscribeAll();
        
        Debug.Log("Game loaded successfully from slot " + _saveIndex);
    }

    public void DeleteSlot(int slotNumber)
    {
        bool deletedSuccess = false;
        
        #if !UNITY_EDITOR && UNITY_WEBGL
            deletedSuccess = DeleteSaveFromIndexedDB(slotNumber);
        #else
            deletedSuccess = DeleteSaveFromFiles(slotNumber);
        #endif


        if (deletedSuccess)
        {
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

    private bool DeleteSaveFromFiles(int slotNumber)
    {
        string filePath = $"{_savePath}/SaveSlot{slotNumber}.json";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"Save slot {slotNumber} deleted successfully.");
            return true;
        }

        return false;
    }
    
    private bool DeleteSaveFromIndexedDB(int slotNumber)
    {
        return JS_DeleteSaveFromIndexedDB(slotNumber);
    }


    public void SelectSlot(int slotNumber)
    {
        
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
            
            SaveSlot(json, slotNumber);

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
            
            SaveState loadedSaveState = LoadSaveSlot(slotNumber);
            
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
        #if !UNITY_EDITOR && UNITY_WEBGL
            return SaveStateExistsInIndexedDB(slotNumber);
        #else
            return SaveStateExistsInFile(slotNumber);
        #endif

    }

    private bool SaveStateExistsInFile(int slotNumber)
    {
        string filePath = $"{_savePath}/SaveSlot{slotNumber}.json";

        if (File.Exists(filePath))
        {
            return true ;
        }
        return false ;
    }

    private bool SaveStateExistsInIndexedDB(int slotNumber)
    { 
        int t = JS_SaveStateExistsInIndexedDB(slotNumber);
        Debug.Log("Unity Call Exists: "+t);
        return t == 1;
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
        tempSaveState.lastUpdate = DateTime.Now;
        
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };
        
        string json = JsonConvert.SerializeObject(tempSaveState, settings);
        
        SaveSlot(json, slotIndex);
        
    }

    private void OnDestroy()
    {
        if (_saveIndex != -1)
        {
            SaveGame();
        }
    }
}
