using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Data;

public enum EffectType
{
    Shield,
    Heal,
    Damage
}

public class UI_EffectItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountTextObj;
    [SerializeField] private Image iconObj;
    [SerializeField] private EffectType effectType;

    [SerializeField] private Sprite damageSprite;
    [SerializeField] private Sprite shieldSprite;
    [SerializeField] private Sprite healSprite;

    [SerializeField] private Material dmgMaterial;
    [SerializeField] private Material healMaterial;
    [SerializeField] private Material shieldMaterial;

    public EffectType EffectType
    {
        get { return effectType; }
        set { effectType = value; }
    }

    public string AmountText
    {
        get { return amountTextObj.text; }
        set { amountTextObj.text = value; }
    }

    public void SetEffect(BigDecimal amount, EffectType type, bool isCritical)
    {
        effectType = type;
        if (!isCritical)
        {
            amountTextObj.text = type == EffectType.Damage? "-"+amount.ToNumberSuffix(false) : amount.ToNumberSuffix(false);
        }
        else
        {
            amountTextObj.text = type == EffectType.Damage ? "-" + amount.ToNumberSuffix(false) + "!!" : amount.ToNumberSuffix(false) + "!!";
        }

        SetImage();
    }

    public void SetImage()
    {
        switch (effectType)
        {
            case EffectType.Damage:
                iconObj.sprite = damageSprite;
                amountTextObj.fontSharedMaterial = dmgMaterial;
                break;
            case EffectType.Heal:
                iconObj.sprite = healSprite;
                amountTextObj.fontSharedMaterial = healMaterial;
                break;
            case EffectType.Shield:
                iconObj.sprite = shieldSprite;
                amountTextObj.fontSharedMaterial = shieldMaterial;
                break;
        }
    }

    public void DestroyEffect()
    {
        Destroy(gameObject);
    }
}
