using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;


[Serializable]
public class BaseColor
{
    public Color BaseColor1;
    public Color BaseColor2;
    public Color BaseColor3;
    public Color BaseColor4;
}
[Serializable]
public class AddOnColor
{
    public Color addOnColor1;
    public Color addOnColor2;
    public Color addOnColor3;
}


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
        return baseColors[UnityEngine.Random.Range(0, baseColors.Length)];
    }

    public AddOnColor RandomAddOnColor()
    {
        return addOnColors[UnityEngine.Random.Range(0, addOnColors.Length)];
    }
}
