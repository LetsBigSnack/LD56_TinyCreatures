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
        UI_ToastManager.Instance.CurrToast = gameObject;
        Destroy(gameObject,3);
    }

    private void OnDestroy()
    {
        Debug.Log("I have been destroyed");
        UI_ToastManager.Instance.CurrToast = null;
        UI_ToastManager.Instance.PushNextToast();
    }
}
