using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System;
using Data;

public class PalletSwap : MonoBehaviour
{
    [SerializeField] private BaseColor baseColor;
    [SerializeField] private AddOnColor addOnColor;

    [JsonIgnore]
    [SerializeField] private Image[] creatureImages;
    [JsonIgnore]
    [SerializeField] private Material mat;

    public BaseColor BaseColor
    {
        get { return baseColor; }
        set { baseColor = value; }
    }

    public AddOnColor AddOnColor
    {
        get { return addOnColor; }
        set { addOnColor = value; }
    }

    public void Awake()
    {
        mat = ColorManager.Instance.BaseMaterial;
    }

    public void Start()
    {
        if (gameObject.GetComponent<UICreatureButton>() == null || gameObject.GetComponent<UICreatureButton>().Creature == null) return;
        baseColor = gameObject.GetComponent<UICreatureButton>().Creature.Representation.BaseColor;
        addOnColor = gameObject.GetComponent<UICreatureButton>().Creature.Representation.AddOnColor;
        GetAllImageComponentsInChildren();
        ApplyNewMaterial();
    }

    public void GetAllImageComponentsInChildren()
    {
        creatureImages = GetComponentsInChildren<Image>();
    }

    public void ApplyNewMaterial()
    {
        foreach (Image img in creatureImages)
        {
            img.material = SwapPallet(mat);
        }
    }

    public Material SwapPallet(Material mat)
    {
        Material newMat = Instantiate(mat);
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
