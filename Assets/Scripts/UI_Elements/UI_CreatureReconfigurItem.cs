using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CreatureReconfigurItem : MonoBehaviour
{
    [SerializeField] private Image currentPart;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI partText;
    [SerializeField] private TextMeshProUGUI typeText;

    public Image CurrentPart
    {
        get => currentPart;
        set => currentPart = value;
    }

    public TextMeshProUGUI NameText
    {
        get => nameText;
        set => nameText = value;
    }
    public TextMeshProUGUI PartText
    {
        get => partText;
        set => partText = value;
    }
    public TextMeshProUGUI TypeText
    {
        get => typeText;
        set => typeText = value;
    }
}
