using System;
using System.Collections.Generic;
using System.Linq;
using Manager;
using Newtonsoft.Json;

namespace Data
{
    [Serializable]
    public class TransientBodyPart
    {
        public string bodyPartName; // Use names or IDs to identify parts
        public bool collected;
    }

    [Serializable]
    public class TransientBodyPartSet
    {
        public string setName; // Use names or IDs to identify sets
        public bool unlocked;
        public List<TransientBodyPart> bodyParts;
    }
    
    [Serializable]
    public class SaveState
    {
        public string saveName;
        public string gameVersion;
        public DateTime lastUpdate;
        
        // Tutorial and Achievements sections
        public TutorialData tutorialData;
        
        // Unlocks
        public List<TransientBodyPartSet> savedSets;

        // Battle
        public BigDecimal playerWins;
        public Creature enemyCreature;
        public bool autoBattle;
        
        // Shop
        public BigDecimal playerMoney;
        public BigDecimal boughtSlots;
        public List<Creature> soledCreatures;
        
        // Inventory
        public int inventorySpace;
        public BigDecimal material_1;
        public BigDecimal material_2;
        public BigDecimal material_3;
        public BigDecimal material_4;

        //Material
        public MaterialStats materialA;
        public MaterialStats materialB;
        public MaterialStats materialC;
        public MaterialStats materialD;

        // Creatures
        public List<Creature> inventory;
        public Creature selectedCreatureBattle;
        public Creature selectedCreaturePodOne;
        public Creature selectedCreaturePodTwo;
        public Creature breedingCreatureResult;
        public Creature selectedCreatureReconfigure;
        public Creature selectedCreatureMaterial_1;
        public Creature selectedCreatureMaterial_2;
        public Creature selectedCreatureMaterial_3;
        public Creature selectedCreatureMaterial_4;
        
        public List<AchievementJSON> achievement;
        
        // Constructor to set default values

        public void InitializeDefaults()
        {
            saveName = "Save";
            gameVersion = "1.0.0";
            lastUpdate = DateTime.Now;
            autoBattle = true;
            playerWins = new BigDecimal(0, 0);
            playerMoney = new BigDecimal(70, 0);
            boughtSlots = new BigDecimal(0, 0);
            soledCreatures = new List<Creature>();
            inventorySpace = 8;
            material_1 = 0;
            material_2 = 0;
            material_3 = 0;
            material_4 = 0;
            tutorialData = new TutorialData();
            materialA = new MaterialStats(60, 0, 0);
            materialB = new MaterialStats(60, 0, 0);
            materialC = new MaterialStats(60, 0, 0);
            materialD = new MaterialStats(60, 0, 0);
#if UNITY_EDITOR
            tutorialData.BattleDone = true;
            tutorialData.ConfigDone = true;
            tutorialData.EntryDone = true;
            tutorialData.FusionDone = true;
            tutorialData.InspectorDone = true;
            tutorialData.ShopDone = true;
#endif
            inventory = new List<Creature>
            {
                CreatureManager.Instance.CreateBasicCreature(),
                CreatureManager.Instance.CreateBasicCreature(),
                CreatureManager.Instance.CreateBasicCreature()
            };

            foreach (Creature creature in inventory)
            {
                CreatureManager.Instance.CheckCollectedParts(creature);
            }
            
            savedSets = CreatureManager.Instance.BodyPartSets
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

            achievement = AchievementManager.Instance.TemplateAchievement;
        }
    }
}