using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InventoryManager : MonoBehaviour
{
    public static UI_InventoryManager Instance;
    [SerializeField] private GameObject uiCreaturePrefabs;
    [SerializeField] private List<GameObject> uiCreatures;
    [SerializeField] private Transform uiCreatureContainer;
    
    
    public void Awake()
    {
        if(Instance != null) {
            Destroy(gameObject);
        }else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        RefreshInventory();
    }
    
    public void RefreshInventory()
    {
        
        foreach (GameObject uiCrt in uiCreatures)
        {
            Destroy(uiCrt);
        }
        
        uiCreatures = new List<GameObject>();
        
        foreach (Creature creature in InventoryManager.Instance.InventoryCreatures)
        {
            GameObject uiCreature = Instantiate(uiCreaturePrefabs, uiCreatureContainer.position, Quaternion.identity);
            uiCreature.transform.SetParent(uiCreatureContainer);
            UICreatureButton uiCreatureButton = uiCreature.GetComponentInChildren<UICreatureButton>();
            uiCreatureButton.Creature = creature;
            uiCreatureButton.GetComponent<UI_CreatureSprite>().SetupRepresentation(creature);
            uiCreatures.Add(uiCreature);
        }
    }
}
