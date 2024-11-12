using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FusionButtonManager : MonoBehaviour
{
    private static UI_FusionButtonManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void FuseCreatures()
    {
        UI_BreedingManager.Instance.FuseCreature();
    }
}
