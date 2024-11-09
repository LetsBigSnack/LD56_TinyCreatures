using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace Data
{
    public enum StatNames
    {
        Speed,
        Attack,
        Defense,
        Dexterity
    }
    
    [Serializable]
    public class CreatureStats
    {
        //"constants for the weights"
        private static Dictionary<StatNames, float> _weights = new Dictionary<StatNames, float>()
        {
            {StatNames.Speed, 0.25f},
            {StatNames.Defense, 0.25f},
            {StatNames.Attack, 0.25f},
            {StatNames.Dexterity, 0.25f},
        };
        

        

        private BigDecimal _speed;

        private BigDecimal _attack;

        private BigDecimal _defense;
 
        private BigDecimal _dexterity;

        [JsonIgnore]
        private BigDecimal _powerLevel;
        
        
        public BigDecimal Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public BigDecimal Attack
        {
            get => _attack;
            set => _attack = value;
        }

        public BigDecimal Defense
        {
            get => _defense;
            set => _defense = value;
        }

        public BigDecimal Dexterity
        {
            get => _dexterity;
            set => _dexterity = value;
        }
    
        [JsonIgnore]
        public BigDecimal PowerLevel
        {
            get => _powerLevel;
            set => _powerLevel = value;
        }

        public CreatureStats(BigDecimal speed, BigDecimal attack, BigDecimal defense, BigDecimal dexterity)
        {
            _speed = speed;
            _attack = attack;
            _defense = defense;
            _dexterity = dexterity;

            CalculatePowerLevel();
        }
        
        // Example method to calculate the power level based on stats
        private void CalculatePowerLevel()
        {
            
            // Weighted sum of stats to determine the power level
            BigDecimal weightedSpeed = _speed * _weights.GetValueOrDefault(StatNames.Speed);
            BigDecimal weightedAttack = _attack * _weights.GetValueOrDefault(StatNames.Attack);
            BigDecimal weightedDefense = _defense * _weights.GetValueOrDefault(StatNames.Defense);
            BigDecimal weightedDexterity = _dexterity * _weights.GetValueOrDefault(StatNames.Dexterity);

            // Calculate the total power level by summing all weighted stats
            BigDecimal totalPowerLevel = weightedSpeed + weightedAttack + weightedDefense + weightedDexterity;

            // Assign the rounded value to powerLevel
            _powerLevel = totalPowerLevel.Round(0);
        }
    }
}