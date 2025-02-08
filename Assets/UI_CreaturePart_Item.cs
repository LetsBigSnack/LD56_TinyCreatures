using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CreaturePart_Item : MonoBehaviour
{
    [SerializeField] private BodyPart bodyPart;
    [SerializeField] private BodyPartType bodyPartType;
    [SerializeField] private Image bodyPartImage;

    public BodyPart BodyPart
    {
        get { return bodyPart; }
        set { bodyPart = value; }
    }

    public BodyPartType BodyPartType
    {
        get { return bodyPartType; }
        set { bodyPartType = value; }
    }

    public Image BodyPartImage
    {
        get { return bodyPartImage; }
        set { bodyPartImage = value; }
    }

    private void Start()
    {
        bodyPartImage.sprite = bodyPart.bodyPartSprite;
    }

    public void OnClick()
    {
        UI_CreatureReconfigureManager.Instance.PickPart(bodyPart, bodyPartType);
    }

    public void OnHover()
    {
        UI_CreatureReconfigureManager.Instance.UpdateBodyPartStatPreview(bodyPart.bodyPartSprite, bodyPartType, bodyPart);
    }

    public void OffHover()
    {
        UI_CreatureReconfigureManager.Instance.ResetBodyPartStatPreview();
    }
}

