using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FuseManager : MonoBehaviour
{
   [SerializeField] private UI_CreatureSprite fuseSprite;
   [SerializeField] private UI_CreatureDetailsText fuseDetailsText;
   
   private SoundManager _soundManager;

   private void Awake()
   {
      _soundManager = FindObjectOfType<SoundManager>();
   }


   private void FixedUpdate()
   {
      fuseSprite.Reset();
      fuseDetailsText.Reset();
      Creature creature = BreedingManager.Instance.Result;
      if (BreedingManager.Instance.Result != null)
      {
         fuseSprite.SetupRepresentation(creature);
         fuseDetailsText.SetupRepresentation(creature);
      }
   }

   public void CollectCreature()
   {
      if (BreedingManager.Instance.Collect())
      {
         UI_ToggleManager.Instance.SwitchState("Inspector");
      }
      else
      {
         _soundManager.PlaySFX("Error");
      }
      UI_InventoryManager.Instance.RefreshInventory();
   }

   public void Sell()
   {
      if (BreedingManager.Instance.Result != null)
      {
         StoreManager.Instance.SellOwnedCreature(BreedingManager.Instance.Result);
         BreedingManager.Instance.Result = null;
         UI_ToggleManager.Instance.SwitchState("Inspector");
      }
      else
      {
         _soundManager.PlaySFX("Error");
      }
      
   }

   public void Refuse()
   {
      Creature creature = BreedingManager.Instance.Result;
      if (creature != null && BreedingManager.Instance.Breed(false))
      {
         StoreManager.Instance.EarnMoney(creature.PowerLevel);
         
         Destroy(creature.gameObject);
         _soundManager.PlaySFX("Click");
         
      }
      else
      {
         _soundManager.PlaySFX("Error");
      }
   }
}
