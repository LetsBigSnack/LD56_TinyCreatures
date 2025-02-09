using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Import for UI components
using Newtonsoft.Json;
using System;

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

    public PalletSwap(BaseColor baseColor, AddOnColor addOnColor)
    {
        this.baseColor = baseColor;
        this.addOnColor = addOnColor;
    }

    void OnEnable()
    {
        mat = ColorManager.Instance.BaseMaterial;
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

        newMat.SetColor("_Accent1", addOnColor.addOnColor1);
        newMat.SetColor("_Accent2", addOnColor.addOnColor2);
        newMat.SetColor("_Accent3", addOnColor.addOnColor3);

        return newMat;
    }
}
