using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UI_PlayerStatManager : MonoBehaviour
{
    
    
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI slotCountText;
    public void FixedUpdate()
    {
        moneyText.text = StoreManager.Instance.PlayerMoney.ToString();
        InventoryManager temp = InventoryManager.Instance;
        slotCountText.text = temp.InventoryCreatures.Count.ToString() + "/" + temp.InventorySpace.ToString();
        
    }
}
