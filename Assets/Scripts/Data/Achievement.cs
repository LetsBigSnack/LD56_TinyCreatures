using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public enum AchievementType
{
    Wins,
    Fusions,
    Speed
}

namespace Data
{
    [Serializable]
    public class AchievementJSON
    {
        //Representation
        public int id;
        public string name;
        public string description;
        public AchievementType type;
        public string sprite;
        public string date;
        public List<Reward> rewards;
        //Logic
        public bool unlocked;
        public List<AchievementRequirement> requirements;
        public int nextAchievement;
    }
}