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
         _soundManager.PlaySFX("Click");
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
         _soundManager.PlaySFX("Click");
         StoreManager.Instance.SellOwnedCreature(BreedingManager.Instance.Result);
         BreedingManager.Instance.Result = null;
      }
      else
      {
         _soundManager.PlaySFX("Error");
      }
      
   }

   public void Refuse()
   {
      if (BreedingManager.Instance.Result != null &&  StoreManager.Instance.RefuseCreature(BreedingManager.Instance.Result))
      {
         Creature creature = BreedingManager.Instance.Result;
         BreedingManager.Instance.Result = null;
         Destroy(creature.gameObject);
         
         if (BreedingManager.Instance.Breed(false))
         {
            _soundManager.PlaySFX("Click");
         }
         else
         {
            _soundManager.PlaySFX("Error");
         }
      }
      else
      {
         _soundManager.PlaySFX("Error");
      }
   }
}
