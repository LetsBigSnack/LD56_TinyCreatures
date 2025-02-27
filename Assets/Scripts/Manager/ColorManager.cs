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

    [SerializeField] private List<UI_CreatureSprite> creatureRepresentations;

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

    public Material CreateNewColoredMaterial(BaseColor baseColor, AddOnColor addOnColor)
    {
        Material newMat = Instantiate(baseMat);
        newMat.SetColor("_Color1", baseColor.BaseColor1);
        newMat.SetColor("_Color2", baseColor.BaseColor2);
        newMat.SetColor("_Color3", baseColor.BaseColor3);
        newMat.SetColor("_Color4", baseColor.BaseColor4);

        newMat.SetColor("_Color5", addOnColor.addOnColor1);
        newMat.SetColor("_Color6", addOnColor.addOnColor2);
        newMat.SetColor("_Color7", addOnColor.addOnColor3);
        newMat.SetColor("_Color8", addOnColor.addOnColor4);

        return newMat;
    }
}
