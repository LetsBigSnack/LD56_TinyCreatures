using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FuseManager : MonoBehaviour
{
   [SerializeField] private UICreatureButton creatureButton;
   [SerializeField] private UI_CreatureSprite fuseSprite;
   [SerializeField] private UI_CreatureDetailsText fuseDetailsText;

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
         SoundManager.Instance.PlaySFX("Click");
         UI_ToggleManager.Instance.SwitchState("Inspector");
      }
      else
      {
         SoundManager.Instance.PlaySFX("Error");
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
         SoundManager.Instance.PlaySFX("Transaction");
       }
      else
      {
         SoundManager.Instance.PlaySFX("Error");
      }
      
   }

   public void Refuse()
   {
      Creature creature = BreedingManager.Instance.Result;
      if (creature != null && BreedingManager.Instance.Breed(true))
      {
         StoreManager.Instance.EarnMoney(creature.CreatureStats.PowerLevel);

            SoundManager.Instance.PlaySFX("Transaction");
         
      }
      else
      {
            SoundManager.Instance.PlaySFX("Error");
      }
   }
}
