using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class UI_CreatureDexEntryItem : MonoBehaviour
{
    //BodypartSet
    [SerializeField] private BodyPartSet bodyPartSet;

    //list of entries
    [SerializeField] private Dictionary<string, BodyPartEntry> bodyPartEntries = new Dictionary<string, BodyPartEntry>();

    //Ui images to fill
    [SerializeField] private Image headImage;
    [SerializeField] private Image bodyImage;
    [SerializeField] private Image armsImage;
    [SerializeField] private Image legsImage;
    [SerializeField] private Image tailImage;
    [SerializeField] private Image backImage;
    [SerializeField] private Image topHeadImage;


    //name
    [SerializeField] private TextMeshProUGUI nameText;


    public BodyPartSet BodySet 
    {

        get => bodyPartSet;
        set => bodyPartSet = value;
  
    }

    // Start is called before the first frame update
    void Start()
    {
        nameText.text = bodyPartSet.setName;
        AddtoDictonary();
        SetDisplay();
    }

    private void AddtoDictonary()
    {
        foreach(BodyPartEntry bodyPart in bodyPartSet.bodyPartEntries)
        {
            bodyPartEntries.Add(bodyPart.bodyPartType.ToString(), bodyPart);
        }
    }

    private void SetDisplay()
    {
        bodyPartEntries.ToList().ForEach(bodyPart => SetImage(bodyPart.Value));
    }

    private void SetImage(BodyPartEntry part)
    {

        Image imageToManipulate = null;

        switch (part.bodyPartType)
        {
            case BodyPartType.Head:
                imageToManipulate = headImage;
                break;

            case BodyPartType.Body:
                imageToManipulate = bodyImage;
                break;

            case BodyPartType.Arms:
                imageToManipulate = armsImage;
                break;

            case BodyPartType.Legs:
                imageToManipulate = legsImage;
                break;

            case BodyPartType.TopHead:
                imageToManipulate = topHeadImage;
                break;

            case BodyPartType.Tail:
                imageToManipulate = tailImage;
                break;

            case BodyPartType.Back:
                imageToManipulate = backImage;
                break;
        }

        imageToManipulate.sprite = part.bodyPart.bodyPartSprite;
        Color imageColor = imageToManipulate.color;
        imageColor.a = part.bodyPart.collected ?  1f : 0.5f;
        imageToManipulate.color = imageColor;
    }
}
