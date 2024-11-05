using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureDexManager : MonoBehaviour
{

    public static CreatureDexManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
