using System;
using System.Collections.Generic;
using System.Linq;
using Manager;
using Newtonsoft.Json;
using UnityEngine;

namespace Data
{
    
    [Serializable]
    public class AchievementRequirement
    {
        public int id;
        public AchievementType type;
        public bool completed;
        public bool trackValues;
        public BigDecimal neededAmount;
        public BigDecimal currentAmount;



        public void AddSubscription(ref Action<BigDecimal> action)
        {
            Debug.Log("Like and Sub!");
            action += OnChanges;
        }

        public void RemoveSubscription(ref Action<BigDecimal> action)
        {
            Debug.Log("Cancle Culture");
            action -= OnChanges;
        }

        private void OnChanges(BigDecimal amount)
        {
            Debug.Log("OnChange"+amount);
            if (completed)
            {
                return;
            }

            if (!trackValues)
            {
                completed = true;
            }
            else
            {
                currentAmount = amount;
                if (currentAmount >= neededAmount)
                {
                    currentAmount = neededAmount;
                    completed = true;
                }
            }

            if (completed) 
            {
                AchievementManager.Instance.EvaluateAchievement(id);
            }
        }
    }
}