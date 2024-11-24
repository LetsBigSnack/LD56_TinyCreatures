using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FuseButtonHelper : MonoBehaviour
{
   public void FuseCreatures()
    {
        UI_BreedingManager.Instance.FuseCreature();
    }
}
