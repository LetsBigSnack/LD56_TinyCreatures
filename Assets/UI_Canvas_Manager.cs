using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Canvas_Manager : MonoBehaviour
{
    public static UI_Canvas_Manager Instance;

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
