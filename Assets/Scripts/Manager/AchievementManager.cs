using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;
using Newtonsoft.Json;
using UnityEngine;

namespace Manager
{
    [Serializable]
    public struct AchievementImages
    {
        public string name;
        public Sprite sprite;
    }
    public class AchievementManager : MonoBehaviour
    {
        public static AchievementManager Instance;
        
        private string _filePath;
        private List<AchievementJSON> _templateAchievement;
        private List<AchievementJSON> _achievementJson;

        [SerializeField] private List<AchievementImages> achievementSprites;

        public  List<AchievementJSON> TemplateAchievement
        {
            get { return _templateAchievement; }
        }

        public  List<AchievementJSON> AchievementJson
        {
            get { return _achievementJson; }
            set { _achievementJson = value; }
        }
    
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _filePath = $"{Application.persistentDataPath}/Achievements.json";
                LoadTemplate();
            }
            else
            {
                Destroy(this);
            }
        }
        
        private void LoadTemplate()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _templateAchievement = JsonConvert.DeserializeObject<List<AchievementJSON>>(json);
            }
        }



        public void SubscribeAll()
        {
            if (_achievementJson == null)
            {
                return;
            }
            
            foreach (AchievementJSON achievement in _achievementJson)
            {
                List<AchievementRequirement> requirements = achievement.requirements.ToList();
                foreach (AchievementRequirement requirement in requirements)
                {
                    switch (requirement.type)
                    {
                        case AchievementType.Wins:
                            requirement.AddSubscription(ref BattleManager.OnPlayerWinsChanged);
                            break;
                    }
                }
            }
           
        }

        public void UnsubscribeAll()
        {
            if (_achievementJson == null)
            {
                return;
            }
            
            foreach (AchievementJSON achievement in _achievementJson)
            {
                List<AchievementRequirement> requirements = achievement.requirements.ToList();
                foreach (AchievementRequirement requirement in requirements)
                {
                    switch (requirement.type)
                    {
                        case AchievementType.Wins:
                            requirement.RemoveSubscription(ref BattleManager.OnPlayerWinsChanged);
                            break;
                    }
                }
            }
        }

        public void EvaluateAchievement(int id)
        {
            AchievementJSON achievement = _achievementJson.Find(x => x.id == id);

            if (achievement == null || achievement.unlocked)
            {
                return;
            }
            
            bool achieved = true;
            
            List<AchievementRequirement> requirements = achievement.requirements.ToList();
            foreach (AchievementRequirement requirement in requirements)
            {
                if (!requirement.completed)
                {
                    achieved = false;
                }
            }
            
            achievement.unlocked = achieved;

            if (achieved)
            {
                achievement.date = DateTime.Now.ToString("dd/MM/yyyy");
                List<Reward> rewards = achievement.rewards.ToList();

                foreach (Reward reward in rewards)
                {
                    RewardManager.Instance.GetRewards(reward);
                }
                if(UI_AchievementManager.Instance != null) { 
                    //TODO:think of better solution!
                    UI_AchievementManager.Instance.SortAchievements();
                }
                UI_ToastManager.Instance.CreateToast(achievement);
                UI_ToastManager.Instance.CreateToast("1", "cheese");
                UI_ToastManager.Instance.CreateToast("2", "burger");
                UI_ToastManager.Instance.CreateToast("3", "pizza");
                UI_ToastManager.Instance.CreateToast("4", "bread");
                UI_ToastManager.Instance.CreateToast("5", "beer");
            }
        }

        public Sprite GetReferancedImage(string imageName)
        {
            Sprite sprite = achievementSprites.Where(x => x.name == imageName).Select(y => y.sprite).FirstOrDefault();
            return sprite;
        }
    }
}
