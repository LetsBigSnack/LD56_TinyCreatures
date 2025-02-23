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
            amountTextObj.text = amount.ToNumberSuffix(false);
        }
        else
        {
            amountTextObj.text = amount.ToNumberSuffix(false) + "!!";
        }

        SetImage();
    }

    public void SetImage()
    {
        switch (effectType)
        {
            case EffectType.Damage:
                iconObj.sprite = damageSprite;
                break;
            case EffectType.Heal:
                iconObj.sprite = healSprite;
                break;
            case EffectType.Shield:
                iconObj.sprite = shieldSprite;
                break;
        }
    }

    public void DestroyEffect()
    {
        Destroy(gameObject);
    }
}
