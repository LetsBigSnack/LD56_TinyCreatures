using System.Collections.Generic;
using System.Data;
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
        

        private float _speed;
        private float _attack;
        private float _defense;
        private float _dexterity;
        private int _powerLevel;
        
        
        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public float Attack
        {
            get => _attack;
            set => _attack = value;
        }

        public float Defense
        {
            get => _defense;
            set => _defense = value;
        }

        public float Dexterity
        {
            get => _dexterity;
            set => _dexterity = value;
        }

        public int PowerLevel
        {
            get => _powerLevel;
            set => _powerLevel = value;
        }

        public CreatureStats(float speed, float attack, float defense, float dexterity)
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
            float weightedSpeed = _speed * _weights.GetValueOrDefault(StatNames.Speed);
            float weightedAttack = _attack * _weights.GetValueOrDefault(StatNames.Attack);
            float weightedDefense = _defense * _weights.GetValueOrDefault(StatNames.Defense);
            float weightedDexterity = _dexterity * _weights.GetValueOrDefault(StatNames.Dexterity);

            // Calculate the total power level by summing all weighted stats
            float totalPowerLevel = weightedSpeed + weightedAttack + weightedDefense + weightedDexterity;

            // Assign the rounded value to powerLevel
            _powerLevel = Mathf.RoundToInt(totalPowerLevel);
        }
        
        
    }
}