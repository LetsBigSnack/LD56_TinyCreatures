using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;
    
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(this);
        }
    }

    public void GetRewards(Reward reward)
    {
        switch (reward.type)
        {
            case RewardType.Currency:
                RewardCurrency(reward.amount);
                break;
            case RewardType.Material:
                RewardMaterial(reward.material, reward.amount);
                break;
        }
    }

    private void RewardMaterial(MaterialType material, BigDecimal amount)
    {
        InventoryManager.Instance.AddMaterialToInventory(material, amount);
    }

    private void RewardCurrency(BigDecimal reward)
    {
        StoreManager.Instance.EarnMoney(reward);
    }
}
