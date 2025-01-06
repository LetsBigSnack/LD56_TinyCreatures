using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ToastItem : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI titelText;
    [SerializeField] private TextMeshProUGUI rewardText;

    public Image Image
    {
        get { return image; }
        set { image = value; }
    }

    public TextMeshProUGUI TitelText
    {
        get { return titelText; }
        set { titelText = value; }
    }

    public TextMeshProUGUI RewardText
    {
        get { return rewardText; }
        set { rewardText = value; }
    }

    public void SetupRepresentation()
    {

    }

    void Start()
    {
        Destroy(gameObject,3);
    }
}
