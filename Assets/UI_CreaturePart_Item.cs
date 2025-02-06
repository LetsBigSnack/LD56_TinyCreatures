using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CreaturePart_Item : MonoBehaviour
{
    [SerializeField] private BodyPart bodyPart;
    [SerializeField] private BodyPartType bodyPartType;
    [SerializeField] private Image bodyPartImage;

    private void OnEnable()
    {
        bodyPartImage.sprite = bodyPart.bodyPartSprite;
    }

    public void OnClick()
    {
        UI_CreatureReconfigureManager.Instance.PickPart(bodyPart, bodyPartType);
    }

    public void OnHover()
    {
        UI_CreatureReconfigureManager.Instance.UpdateBodyPartStatPreview(bodyPart.bodyPartSprite, bodyPartType);
    }

    public void OffHover()
    {
        UI_CreatureReconfigureManager.Instance.ResetBodyPartStatPreview();
    }
}

