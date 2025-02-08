using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalletSwap : MonoBehaviour
{
    public Color color1 = Color.red;
    public Color color2 = Color.green;
    public Color color3 = Color.blue;
    public Color color4 = Color.yellow;
    
    private Material mat;

    void Awake() {
        // Get the SpriteRenderer and create an instance of its material.
        var sr = GetComponent<SpriteRenderer>();
        mat = Instantiate(sr.material);
        sr.material = mat;
    }

    public void SwapPallet()
    {
        mat.SetColor("_Color1", color1);
        mat.SetColor("_Color2", color2);
        mat.SetColor("_Color3", color3);
        mat.SetColor("_Color4", color4);
    }
    
}
