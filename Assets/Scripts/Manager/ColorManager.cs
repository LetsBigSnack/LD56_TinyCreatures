using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Data;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;

    [SerializeField] private BaseColor[] baseColors;
    [SerializeField] private AddOnColor[] addOnColors;
    [SerializeField] private Material baseMat;
    public Material BaseMaterial
    {
        get { return baseMat; }
    }

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

    public BaseColor RandomBaseColor()
    {
        BaseColor newBaseColor = new BaseColor();
        return newBaseColor = baseColors[UnityEngine.Random.Range(0, baseColors.Length)];
    }

    public AddOnColor RandomAddOnColor()
    {
        AddOnColor newAddOnColor = new AddOnColor();
        return newAddOnColor = addOnColors[UnityEngine.Random.Range(0, addOnColors.Length)];
    }
}
