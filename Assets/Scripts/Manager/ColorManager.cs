using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;

    private Color32[] targetColorsBase;
    private Color32[] targetColorsAddOn;

    public Color base_1;
    public Color base_2;
    public Color base_3;
    public Color base_4;
    public Color base_5;

    public Color addOn_1;
    public Color addOn_2;
    public Color addOn_3;
    public Color addOn_4;

    public Color palette_A_Base_1;
    public Color palette_A_Base_2;
    public Color palette_A_Base_3;
    public Color palette_A_Base_4;
    public Color palette_A_Base_5;

    public Color palette_A_addOn_1;
    public Color palette_A_addOn_2;
    public Color palette_A_addOn_3;
    public Color palette_A_addOn_4;

    public Color palette_B_Base_1;
    public Color palette_B_Base_2;
    public Color palette_B_Base_3;
    public Color palette_B_Base_4;
    public Color palette_B_Base_5;

    public Color palette_B_addOn_1;
    public Color palette_B_addOn_2;
    public Color palette_B_addOn_3;
    public Color palette_B_addOn_4;

    public Color palette_C_Base_1;
    public Color palette_C_Base_2;
    public Color palette_C_Base_3;
    public Color palette_C_Base_4;
    public Color palette_C_Base_5;

    public Color palette_C_addOn_1;
    public Color palette_C_addOn_2;
    public Color palette_C_addOn_3;
    public Color palette_C_addOn_4;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            CreateColorSets();
        }
    }

    private void CreateColorSets()
    {
        // Base colors
        ColorUtility.TryParseHtmlString("#282C3C", out base_1);
        ColorUtility.TryParseHtmlString("#464762", out base_2);
        ColorUtility.TryParseHtmlString("#696682", out base_3);
        ColorUtility.TryParseHtmlString("#9A97B9", out base_4);
        ColorUtility.TryParseHtmlString("#C5C7DD", out base_5);

        // Add-on colors
        ColorUtility.TryParseHtmlString("#64364B", out addOn_1);
        ColorUtility.TryParseHtmlString("#B25266", out addOn_2);
        ColorUtility.TryParseHtmlString("#E27285", out addOn_3);
        ColorUtility.TryParseHtmlString("#F6A2A8", out addOn_4);

        // Palette 1
        ColorUtility.TryParseHtmlString("#1F3728", out palette_A_Base_1);
        ColorUtility.TryParseHtmlString("#2E5A41", out palette_A_Base_2);
        ColorUtility.TryParseHtmlString("#3E7E59", out palette_A_Base_3);
        ColorUtility.TryParseHtmlString("#56A077", out palette_A_Base_4);
        ColorUtility.TryParseHtmlString("#A5D2B2", out palette_A_Base_5);

        ColorUtility.TryParseHtmlString("#6B5400", out palette_A_addOn_1);
        ColorUtility.TryParseHtmlString("#B58E00", out palette_A_addOn_2);
        ColorUtility.TryParseHtmlString("#F6C100", out palette_A_addOn_3);
        ColorUtility.TryParseHtmlString("#FFE066", out palette_A_addOn_4);

        // Palette 2
        ColorUtility.TryParseHtmlString("#2D1E3E", out palette_B_Base_1);
        ColorUtility.TryParseHtmlString("#4A3571", out palette_B_Base_2);
        ColorUtility.TryParseHtmlString("#6C59A3", out palette_B_Base_3);
        ColorUtility.TryParseHtmlString("#8E7CC8", out palette_B_Base_4);
        ColorUtility.TryParseHtmlString("#B9A8E8", out palette_B_Base_5);

        ColorUtility.TryParseHtmlString("#5D2549", out palette_B_addOn_1);
        ColorUtility.TryParseHtmlString("#923F6E", out palette_B_addOn_2);
        ColorUtility.TryParseHtmlString("#C55A90", out palette_B_addOn_3);
        ColorUtility.TryParseHtmlString("#E37CAF", out palette_B_addOn_4);

        // Palette 3
        ColorUtility.TryParseHtmlString("#2C2C2C", out palette_C_Base_1);
        ColorUtility.TryParseHtmlString("#474747", out palette_C_Base_2);
        ColorUtility.TryParseHtmlString("#6D6D6D", out palette_C_Base_3);
        ColorUtility.TryParseHtmlString("#9A9A9A", out palette_C_Base_4);
        ColorUtility.TryParseHtmlString("#CACACA", out palette_C_Base_5);

        ColorUtility.TryParseHtmlString("#2D4F2E", out palette_C_addOn_1);
        ColorUtility.TryParseHtmlString("#4D7B4F", out palette_C_addOn_2);
        ColorUtility.TryParseHtmlString("#75A478", out palette_C_addOn_3);
        ColorUtility.TryParseHtmlString("#A4D3A5", out palette_C_addOn_4);
    }

    private bool ColorsMatch(Color32 a, Color32 b)
    {
        return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
    }

    public Color32[] GetRandomBaseColorArray()
    {
        int random = UnityEngine.Random.Range(1, 4);
        Debug.Log(random);
        Color32[] randomPal = new Color32[] { };

        if (random == 1)
        {
            randomPal = new Color32[]
            {
            (Color32)palette_A_Base_1,
            (Color32)palette_A_Base_2,
            (Color32)palette_A_Base_3,
            (Color32)palette_A_Base_4,
            (Color32)palette_A_Base_5
            };
        }

        if (random == 2)
        {
            randomPal = new Color32[]
            {
            (Color32)palette_B_Base_1,
            (Color32)palette_B_Base_2,
            (Color32)palette_B_Base_3,
            (Color32)palette_B_Base_4,
            (Color32)palette_B_Base_5
            };
        }

        if (random == 3)
        {
            randomPal = new Color32[]
            {
            (Color32)palette_C_Base_1,
            (Color32)palette_C_Base_2,
            (Color32)palette_C_Base_3,
            (Color32)palette_C_Base_4,
            (Color32)palette_C_Base_5
            };
        }
        return randomPal;
    }

    public Color32[] GetRandomAddOnColorArray()
    {
        int random = UnityEngine.Random.Range(1, 4);
        Color32[] randomPal = new Color32[] { };

        if (random == 1)
        {
            randomPal = new Color32[]
            {
            (Color32)palette_A_addOn_1,
            (Color32)palette_A_addOn_2,
            (Color32)palette_A_addOn_3,
            (Color32)palette_A_addOn_4,
            };
        }

        if (random == 2)
        {
            randomPal = new Color32[]
            {
            (Color32)palette_B_addOn_1,
            (Color32)palette_B_addOn_2,
            (Color32)palette_B_addOn_3,
            (Color32)palette_B_addOn_4,
            };
        }

        if (random == 3)
        {
            randomPal = new Color32[]
            {
            (Color32)palette_C_addOn_1,
            (Color32)palette_C_addOn_2,
            (Color32)palette_C_addOn_3,
            (Color32)palette_C_addOn_4,
            };
        }
        return randomPal;
    }

    public Dictionary<BodyPartType, BodyPart> CreateNewCreatureColorSprites(Dictionary<BodyPartType, BodyPart> bodyParts, Color32[] randomBaseColor, Color32[] randomAddOnColor)
    {
        Dictionary<BodyPartType, BodyPart> repaintedParts = new Dictionary<BodyPartType, BodyPart>();

        foreach (var part in bodyParts)
        {
            BodyPart copiedBodyPart = new BodyPart
            {
                bodyPartName = part.Value.bodyPartName,
                bodyPartSprite = part.Value.bodyPartSprite,
            };

            if (copiedBodyPart.bodyPartName != "Empty")
            {
                Sprite sprite = copiedBodyPart.bodyPartSprite;

                int x = Mathf.RoundToInt(sprite.rect.x);
                int y = Mathf.RoundToInt(sprite.rect.y);
                int width = Mathf.RoundToInt(sprite.rect.width);
                int height = Mathf.RoundToInt(sprite.rect.height);

                Texture2D subTexture = new Texture2D(width, height);
                subTexture.SetPixels(sprite.texture.GetPixels(x, y, width, height));
                subTexture.filterMode = FilterMode.Point;
                subTexture.Apply();

                Color32[] pixels = subTexture.GetPixels32();
                for (int i = 0; i < pixels.Length; i++)
                {
                    if (ColorsMatch(pixels[i], base_2)) pixels[i] = randomBaseColor[1];
                    else if (ColorsMatch(pixels[i], base_3)) pixels[i] = randomBaseColor[2];
                    else if (ColorsMatch(pixels[i], base_4)) pixels[i] = randomBaseColor[3];
                    else if (ColorsMatch(pixels[i], base_5)) pixels[i] = randomBaseColor[4];
                    else if (ColorsMatch(pixels[i], addOn_1)) pixels[i] = randomAddOnColor[0];
                    else if (ColorsMatch(pixels[i], addOn_2)) pixels[i] = randomAddOnColor[1];
                    else if (ColorsMatch(pixels[i], addOn_3)) pixels[i] = randomAddOnColor[2];
                    else if (ColorsMatch(pixels[i], addOn_4)) pixels[i] = randomAddOnColor[3];
                }
                subTexture.SetPixels32(pixels);
                subTexture.Apply();
                Sprite newSprite = Sprite.Create(subTexture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
                copiedBodyPart.bodyPartSprite = newSprite;
            }

            repaintedParts.Add(part.Key, copiedBodyPart);
        }

        return repaintedParts;
    }

    public Sprite RepaintSprite(Sprite sprite, Color32[] randomBaseColor, Color32[] randomAddOnColor)
    {
        int x = Mathf.RoundToInt(sprite.rect.x);
        int y = Mathf.RoundToInt(sprite.rect.y);
        int width = Mathf.RoundToInt(sprite.rect.width);
        int height = Mathf.RoundToInt(sprite.rect.height);

        Texture2D subTexture = new Texture2D(width, height);
        subTexture.SetPixels(sprite.texture.GetPixels(x, y, width, height));
        subTexture.filterMode = FilterMode.Point;
        subTexture.Apply();

        Color32[] pixels = subTexture.GetPixels32();
        for (int i = 0; i < pixels.Length; i++)
        {
            if (ColorsMatch(pixels[i], base_2)) pixels[i] = randomBaseColor[1];
            else if (ColorsMatch(pixels[i], base_3)) pixels[i] = randomBaseColor[2];
            else if (ColorsMatch(pixels[i], base_4)) pixels[i] = randomBaseColor[3];
            else if (ColorsMatch(pixels[i], base_5)) pixels[i] = randomBaseColor[4];
            else if (ColorsMatch(pixels[i], addOn_1)) pixels[i] = randomAddOnColor[0];
            else if (ColorsMatch(pixels[i], addOn_2)) pixels[i] = randomAddOnColor[1];
            else if (ColorsMatch(pixels[i], addOn_3)) pixels[i] = randomAddOnColor[2];
            else if (ColorsMatch(pixels[i], addOn_4)) pixels[i] = randomAddOnColor[3];
        }
        subTexture.SetPixels32(pixels);
        subTexture.Apply();
        Sprite newSprite = Sprite.Create(subTexture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
        return newSprite;
    }  
}
