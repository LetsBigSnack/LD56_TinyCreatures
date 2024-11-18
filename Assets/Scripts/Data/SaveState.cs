using System;
using System.Collections.Generic;
using System.Linq;
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
        
        // Creatures
        public List<Creature> inventory;
        public Creature selectedCreatureBattle;
        public Creature selectedCreaturePodOne;
        public Creature selectedCreaturePodTwo;
        public Creature breedingCreatureResult;
        public Creature selectedCreatureReconfigure;

        // Constructor to set default values
        
        public void InitializeDefaults()
        {
            saveName = "Save";
            gameVersion = "1.0.0";
            autoBattle = true;
            playerWins = new BigDecimal(0, 0);
            playerMoney = new BigDecimal(70, 0);
            boughtSlots = new BigDecimal(0, 0);
            soledCreatures = new List<Creature>();
            inventorySpace = 8;
            tutorialData = new TutorialData();
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
            
        }
    }
}