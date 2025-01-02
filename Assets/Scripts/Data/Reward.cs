using System;
using System.Collections.Generic;

namespace Data
{
    public enum RewardType
    {
        Currency,
        Material,
        BodyParts,
        Bonus,
        Misc
    }

    public enum BonusType
    {
        Money
    }
    
    [Serializable]
    public class Reward
    {
        public BigDecimal amount;
        public String description;
        public RewardType type;
        public MaterialType material;
        public List<String> bodyParts;
        public BonusType bonus;
    }
}