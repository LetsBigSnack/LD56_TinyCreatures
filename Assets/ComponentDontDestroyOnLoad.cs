using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentDontDestroyOnLoad : MonoBehaviour
{
    public static ComponentDontDestroyOnLoad Instance;

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
        DontDestroyOnLoad(gameObject);
    }
}
